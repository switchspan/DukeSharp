using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using NLog;
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Private member variables

        private static int SEARCH_EXPANSION_FACTOR = 1;
        private readonly Analyzer _analyzer;
        private readonly Configuration _config;
        private readonly QueryResultTracker _maintracker;
        private Directory _directory;
        private IndexWriter _iwriter;
        private int _maxSearchHits;
        private float _minRelevance;
        private IndexSearcher _searcher;

        #endregion

        #region Constructors

        public LuceneDatabase(Configuration config, bool overwrite, DatabaseProperties dbprops)
        {
            _config = config;
            _analyzer = new StandardAnalyzer(Version.LUCENE_29);
            _maintracker = new QueryResultTracker(config, _analyzer, _searcher, dbprops.MaxSearchHits,
                                                  dbprops.MinRelevance);
            _maxSearchHits = dbprops.MaxSearchHits;
            _minRelevance = dbprops.MinRelevance;

            try
            {
                OpenIndexes(overwrite);
                OpenSearchers();
            }
            catch (Exception ex)
            {
                logger.Error("Error initializing object: {0}", ex.Message);
            }
        }

        #endregion

        #region Member methods

        public bool IsInMemory()
        {
            return (_directory.GetType() == typeof (RAMDirectory));
        }

        public void Index(IRecord record)
        {
            var doc = new Document();

            foreach (string propname in record.GetProperties())
            {
                Property prop = _config.GetPropertyByName(propname);
                if (prop == null)
                {
                    throw new Exception(String.Format("Record has property {0} for which there is no configuration.",
                                                      propname));
                }

                Field.Index ix; //TODO: could cache this. or get it from property
                ix = prop.IsIdProperty ? Field.Index.NOT_ANALYZED : Field.Index.ANALYZED;

                foreach (string v in record.GetValues(propname))
                {
                    if (v.Equals(""))
                        continue; //FIXME: not sure if this is necessary

                    doc.Add(new Field(propname, v, Field.Store.YES, ix));
                }
            }

            try
            {
                _iwriter.AddDocument(doc);
            }
            catch (Exception ex)
            {
                logger.Error("Error adding document to index writer: {0}", ex.Message);
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
                logger.Error("Error flushing changes to disk: {0}", ex.Message);
            }
        }

        public IRecord FindRecordById(string id)
        {
            Property idprop = _config.GetIdentityProperties()[0];
            foreach (IRecord r in _maintracker.Lookup(idprop, id))
            {
                if (r.GetValue(idprop.Name) == id)
                    return r;
            }

            return null; // not found
        }

        /// <summary>
        /// Look up potentially matching records.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
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
                logger.Error("Error closing database: {0}", ex.Message);
            }
        }

        // ------- INTERNALS


        private void OpenIndexes(bool overwrite)
        {
            if (_directory == null)
            {
                try
                {
                    if (_config.Path == null)
                    {
                        _directory = new RAMDirectory();
                    }
                    else
                    {
                        _directory = FSDirectory.Open(new DirectoryInfo(_config.Path));
                    }

                    _iwriter = new IndexWriter(_directory, _analyzer, overwrite, new IndexWriter.MaxFieldLength(25000));
                    _iwriter.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("Error opening indexes: {0}", ex.Message);
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
                logger.Error("Error opening searchers: {0}", ex.Message);
            }
        }

        #endregion
    }
}