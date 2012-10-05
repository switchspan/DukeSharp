using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Documents;

namespace Duke
{
    /// <summary>
    /// These objects are used to estimate the size of the query result
    /// we should ask Lucene for. This parameter is the single biggest influence 
    /// on matching performance, but setting it too low causes matches to be missed. 
    /// We therefore try hard to estimate it as correctly as possible.
    /// 
    /// The reason this is a separate class is that we used to need one of these 
    /// for every property because the different properties will behave differently.
    /// 
    /// FIXME: the class is badly named
    /// FIXME: the class is not thread-safe
    /// </summary>
    public class QueryResultTracker
    {
        #region Private member variables

        private readonly Configuration _config;
        private readonly Analyzer _analyzer;
        private readonly IndexSearcher _searcher;
        private readonly int _maxSearchHits;
        private readonly float _minRelevance;

        private int _limit;
        // Ring buffer containing n last search result sizes, except for
        // searches which found nothing.
        private readonly int[] _prevsizes;
        private int _sizeix; // position in prevsizes

        private const int SEARCH_EXPANSION_FACTOR = 1;

        #endregion

        #region Constructors

        public QueryResultTracker(Configuration config, Analyzer analyzer, IndexSearcher searcher, int maxSearchHits, float minRelevance)
        {
            _limit = 100;
            _prevsizes = new int[10];
            _config = config;
            _analyzer = analyzer;
            _maxSearchHits = maxSearchHits;
            _searcher = searcher;
            _minRelevance = minRelevance;
        }

        #endregion

        #region Member methods

        public List<IRecord> Lookup(IRecord record)
        {
            // first we build the combined query for all lookup properties
            var query = new BooleanQuery();
            foreach (var lookupProperty in _config.GetLookupProperties())
            {
                var values = record.GetValues(lookupProperty.GetName());
                if (values == null)
                    continue;
                foreach (var value in values)
                {
                    ParseTokens(query, lookupProperty.GetName(), value);
                }
            }

            // then we perform the actual search
            return DoQuery(query);
        }

        public List<IRecord> Lookup(Property property, string value)
        {
            var v = CleanLucene(value);
            if (v.Length == 0) 
                return new List<IRecord>();

            var query = ParseTokens(property.GetName(), v);
            return DoQuery(query);
        } 

        private List<IRecord> DoQuery(Query query)
        {
            List<IRecord> matches;
            try
            {
                ScoreDoc[] hits;
                
                int thislimit = Math.Min(_limit, _maxSearchHits);

                while (true)
                {
                    hits = _searcher.Search(query, null, thislimit).ScoreDocs;
                    if (hits.Length < thislimit || thislimit == _maxSearchHits)
                        break;
                    thislimit = thislimit * 5;
                }

                matches = new List<IRecord>(Math.Min(hits.Length, _maxSearchHits));
                for (int ix = 0; ix < hits.Length && hits[ix].Score >= _minRelevance; ix++)
                {
                    matches.Add(new DocumentRecord(hits[ix].Doc, _searcher.Doc(hits[ix].Doc)));
                }

                if (hits.Length > 0)
                {
                    _prevsizes[_sizeix++] = matches.Count;
                    if (_sizeix == _prevsizes.Length)
                    {
                        _sizeix = 0;
                        _limit = Math.Max((int)(Average() * SEARCH_EXPANSION_FACTOR), _limit);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return matches;
        } 

        /// <summary>
        /// Parses the query. Using this instead of a QueryParser in order to avoid 
        /// thread-safety issues with Lucene's query parser.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private Query ParseTokens(string fieldName, string param)
        {
            var searchQuery = new BooleanQuery();
            if (param != null)
            {
                var tokenStream = _analyzer.TokenStream(fieldName, new StringReader(param));
                var attr = tokenStream.GetAttribute(typeof (CharTokenizer));

                try
                {
                    while (tokenStream.IncrementToken())
                    {
                        string term = attr.ToString();
                        Query termQuery = new TermQuery(new Term(fieldName, term));
                        searchQuery.Add(termQuery, BooleanClause.Occur.SHOULD);
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception(String.Format("Error parsing input string '{0}' in field {1}", param, fieldName));
                }
            }

            return searchQuery;
        }

        private void ParseTokens(BooleanQuery parent, string fieldName, string value)
        {
           value = CleanLucene(value);
          if (value.Length == 0)
            return;
      
          var tokenStream = _analyzer.TokenStream(fieldName, new StringReader(value));
          var attr = tokenStream.GetAttribute(typeof (CharTokenizer));
                        
          try {
            while (tokenStream.IncrementToken()) {
              String term = attr.ToString();
              Query termQuery = new TermQuery(new Term(fieldName, term));
              parent.Add(termQuery, BooleanClause.Occur.SHOULD);
            }
          } catch (System.Exception ex) {
            //throw new RuntimeException("Error parsing input string '"+value+"' "+
            //                           "in field " + fieldName);
              throw new Exception(String.Format("Error parsing input string '{0}' in field {1}", value, fieldName));
          }
        }

        private double Average()
        {
            int sum = 0;
            int ix = 0;
            for (; ix < _prevsizes.Length && _prevsizes[ix] != 0; ix++)
                sum += _prevsizes[ix];
            return sum / (double)ix;
        }

        private string CleanLucene(string query)
        {
            char[] tmp = new char[query.Length];
            int count = 0;
            for (int ix = 0; ix < query.Length; ix++)
            {
                char ch = query[ix];
                if (ch != '*' && ch != '?' && ch != '!' && ch != '&' && ch != '(' &&
                    ch != ')' && ch != '-' && ch != '+' && ch != ':' && ch != '"' &&
                    ch != '[' && ch != ']' && ch != '~' && ch != '{' && ch != '}' &&
                    ch != '^' && ch != '|')
                    tmp[count++] = ch;
            }

            return new String(tmp, 0, count).Trim();

        }
        #endregion
    }
}