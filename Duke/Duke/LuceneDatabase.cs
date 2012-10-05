using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace Duke
{
    /// <summary>
    /// Represents the Lucene index, and implements record linkage services
    /// on top of it.
    /// </summary>
    public class LuceneDatabase : IDatabase
    {
        #region Private member variables

        private static int SEARCH_EXPANSION_FACTOR = 1;
        private readonly Analyzer _analyzer;
        private readonly Configuration _config;
        private readonly QueryResultTracker _maintracker;
        private Directory _directory;
        private IndexWriter _iwriter;
        private IndexSearcher _searcher;
        private int max_search_hits;
        private float min_relevance;

        #endregion

        #region Constructors

        public LuceneDatabase(Configuration config, bool overwrite, DatabaseProperties dbprops)
        {
            _config = config;
            _analyzer = new StandardAnalyzer(Version.LUCENE_29);
            _maintracker = new QueryResultTracker(config, _analyzer, _searcher, dbprops.MaxSearchHits,
                                                  dbprops.MinRelevance);
            max_search_hits = dbprops.MaxSearchHits;
            min_relevance = dbprops.MinRelevance;
        }

        #endregion

        #region Member methods

        public bool IsInMemory()
        {
            throw new NotImplementedException();
        }

        public void Index(IRecord record)
        {
            Document doc = new Document();

            foreach (var propname in record.GetProperties())
            {
                Property prop = _config.GetPropertyByName(propname);
                if (prop == null)
                {
                    throw new Exception(String.Format("Record has property {0} for which there is no configuration.", propname));
                }

                Field.Index ix; //TODO: could cache this. or get it from property
                if (prop.IsIdProperty())
                {
                    ix = Field.Index.NOT_ANALYZED; // so FindRecordById will work
                }
                else // if (prop.IsAnalyzedProperty())
                {
                    ix = Field.Index.ANALYZED;
                    // FIXME: it turns out that with the StandardAnalyzer you can't have a
                    // multi-token value that's not analyzed if you want to find it again...
                    // else
                    //   ix = Field.Index.NOT_ANALYZED;
                }

                foreach (var v in record.GetValues(propname))
                {
                    if (v.Equals(""))
                        continue; //FIXME: not sure if this is necessary

                    doc.Add(new Field(propname, v, Field.Store.YES, ix));
                }
            }


        }

        /// <summary>
        /// Flushes all changes to disk.
        /// </summary>
        public void Commit()
        {
            try
            {
                if (_searcher != null)
                    _searcher.Close();

                // it turns out that IndexWriter.optimize actually slows
                // searches down, because it invalidates the cache. therefore
                // not calling it any more.
                // http://www.searchworkings.org/blog/-/blogs/uwe-says%3A-is-your-reader-atomic
                // iwriter.optimize();
                _iwriter.Commit();
                OpenSearchers();
            }
            catch (Exception ex)
            {
                throw; //TODO: log this...
            }
        }

        public IRecord FindRecordById(string id)
        {
            Property idprop = _config.GetIdentityProperties()[0];
            foreach (IRecord r in _maintracker.Lookup(idprop, id))
            {
                if (r.GetValue(idprop.GetName()) == id)
                    return r;
            }

            return null;
        }

        public List<IRecord> FindCandidateMatches(IRecord record)
        {
            return _maintracker.Lookup(record);
        }

        /// <summary>
        /// Stores state to disk and closes all open resources.
        /// </summary>
        public void Close()
        {
            try
            {
                _iwriter.Close();
                _directory.Close();
                if (_searcher != null)
                {
                    _searcher.Close();
                }
            }
            catch (Exception ex)
            {
                throw; //TODO: logging here...
            }
        }

        private void OpenIndexes(bool overwrite)
        {
            if (_directory == null)
            {
                try
                {
                    if (_config.GetPath() == null)
                    {
                        _directory = new RAMDirectory();
                    }
                    else
                    {
                        _directory = FSDirectory.Open(new DirectoryInfo(_config.GetPath()));
                    }

                    _iwriter = new IndexWriter(_directory, _analyzer, overwrite, new IndexWriter.MaxFieldLength(25000));
                    _iwriter.Commit();
                }
                catch (Exception ex)
                {
                    throw; //TODO: log this...
                }
            }
        }

        public void OpenSearchers()
        {
            try
            {
                _searcher = new IndexSearcher(_directory, true);
            }
            catch (Exception ex)
            {
                throw; //TODO: log this...
            }
        }

        #endregion
    }
}