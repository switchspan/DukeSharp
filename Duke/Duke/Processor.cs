using System.Collections.Generic;
using System.IO;
using Duke.Comparators;
using Duke.Matchers;
using Duke.Utils;
using Lucene.Net.Index;

namespace Duke
{
    /// <summary>
    /// The class that implements the actual deduplication and record
    /// linkage logic.
    /// </summary>
    public class Processor
    {
        #region Private member variables

        private static int _defaultBatchSize = 40000;
        private readonly double[] _accprob;
        private readonly Configuration _config;
        private readonly List<IMatchListener> _listeners;

        private readonly List<Property> _proporder;
        private IMatchListener _choosebest;
        protected IDatabase _database;
        private IMatchListener _passthrough;

        #endregion

        #region Constructors

        public Processor(Configuration config) : this(config, true)
        {
        }

        public Processor(Configuration config, bool overwrite) : this(config, config.CreateDatabase(overwrite))
        {
        }

        public Processor(Configuration config, IDatabase database)
        {
            _config = config;
            _database = database;
            _listeners = new List<IMatchListener>();

            _passthrough = new PassThroughFilter();
            _choosebest = new ChooseBestFilter();

            // precomputing for later optimizations
            _proporder = new List<Property>();
            foreach (Property p in _config.GetProperties())
            {
                if (!p.IsIdProperty)
                    _proporder.Add(p);
            }

            _proporder.Sort(new PropertyComparator());

            // still precomputing
            double prob = 0.5;
            _accprob = new double[_proporder.Count];
            for (int ix = _proporder.Count - 1; ix >= 0; ix--)
            {
                prob = StandardUtils.ComputeBayes(prob, _proporder[ix].HighProbability);
                _accprob[ix] = prob;
            }
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Registers a match listener
        /// </summary>
        /// <param name="listener"></param>
        public void AddMatchListener(IMatchListener listener)
        {
            _listeners.Add(listener);
        }

        /// <summary>
        /// Returns all registered listeners
        /// </summary>
        /// <returns></returns>
        public List<IMatchListener> GetListeners()
        {
            return _listeners;
        }

        public IDatabase GetDatabase()
        {
            return _database;
        }

        // deduplication
        public void Deduplicate()
        {
            Deduplicate(_config.GetDataSources(), _defaultBatchSize);
        }

        public void Deduplicate(int batchSize)
        {
            Deduplicate(_config.GetDataSources(), batchSize);
        }


        public void Deduplicate(List<IDataSource> sources, int batchSize)
        {
            var batch = new List<IRecord>();
            int count = 0;

            foreach (IDataSource dataSource in sources)
            {
                //dataSource.SetLogger();
                RecordIterator it2 = dataSource.GetRecords();
                try
                {
                    IEnumerator<IRecord> recEnumeration = it2.GetEnumerator();
                    while (recEnumeration.MoveNext())
                    {
                        IRecord record = recEnumeration.Current;
                        batch.Add(record);
                        count++;
                        if (count%batchSize == 0)
                        {
                            foreach (IMatchListener matchListener in _listeners)
                            {
                                matchListener.BatchReady(batch.Count);
                            }
                            //Deduplicated(batch);
                            it2.BatchProcessed();
                            batch = new List<IRecord>();
                        }
                    }
                }
                finally
                {
                    it2.Close();
                }
            }

            if (batch.Count != 0)
            {
                foreach (IMatchListener matchListener in _listeners)
                {
                    matchListener.BatchReady(batch.Count);
                }
                Deduplicate(batch);
            }

            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.EndProcessing();
            }
        }

        public void Deduplicate(List<IRecord> records)
        {
            try
            {
                // prepare
                foreach (var record in records)
                {
                    _database.Index(record);
                }

                _database.Commit();

                // then match
                foreach (var record in records)
                {
                    //Match(record, _passthrough);
                }

                foreach (var matchListener in _listeners)
                {
                    matchListener.BatchDone();
                }
            }
            catch (CorruptIndexException e)
            {
                throw new DukeException(e.Message);
            }
            catch (IOException e)
            {
                throw new DukeException(e.Message);
            }
        }

        public void Link()
        {
            //TODO: Finish here...
        }

        // Commits all state to disk and frees up resources
        public void Close()
        {
            _database.Close();
        }

        // Internals
        private bool IsSameAs(IRecord r1, IRecord r2)
        {
            foreach (Property idp in _config.GetIdentityProperties())
            {
                List<string> vs2 = r2.GetValues(idp.Name);
                List<string> vs1 = r1.GetValues(idp.Name);
                if (vs1 == null)
                    continue;
                foreach (string v1 in vs1)
                {
                    if (vs2.Contains(v1)) return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Notify all listeners that we started on this record.
        /// </summary>
        /// <param name="record"></param>
        private void RegisterStartRecord(IRecord record)
        {
            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.StartRecord(record);
            }
        }

        /// <summary>
        /// Notify all listeners that the two records match
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="confidence"></param>
        private void RegisterMatch(IRecord r1, IRecord r2, double confidence)
        {
            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.Matches(r1, r2, confidence);
            }
        }

        /// <summary>
        /// Notify listeners that the two records MAY match
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="confidence"></param>
        private void RegisterMatchPerhaps(IRecord r1, IRecord r2, double confidence)
        {
            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.Matches(r1, r2, confidence);
            }
        }

        /// <summary>
        /// Notifies listeners that we found no matches for this record.
        /// </summary>
        /// <param name="current"></param>
        private void RegisterNoMatchFor(IRecord current)
        {
            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.NoMatchFor(current);
            }
        }

        /// <summary>
        /// Notifies listeners that we finished this record.
        /// </summary>
        private void RegisterEndRecord()
        {
            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.EndRecord();
            }
        }

        #endregion
    }
}