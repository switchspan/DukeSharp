using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;


namespace Duke
{
    /// <summary>
    /// Represents the Lucene index, and implements record linkage services
    /// on top of it.
    /// </summary>
    public class LuceneDatabase : IDatabase
    {
        #region Private member variables

        private Configuration _config;
        //private QueryResultTracker _maintracker;
        private IndexWriter _iwriter;
        private Directory _directory;
        private IndexSearcher _searcher;
        private Analyzer _analyzer;
        // Deichman case:
        //  1 = 40 minutes
        //  4 = 48 minutes
        //private final static int SEARCH_EXPANSION_FACTOR = 1;
        private int max_search_hits;
        private float min_relevance;
        #endregion

        #region Constructors

        #endregion

        #region Member methods
        public bool IsInMemory()
        {
            throw new NotImplementedException();
        }

        public void Index(IRecord record)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public IRecord FindRecordById(string id)
        {
            throw new NotImplementedException();
        }

        public List<IRecord> FindCandidateMatches(IRecord record)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
        #endregion

        
    }
}
