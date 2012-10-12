using System;
using System.Collections.Generic;
using System.IO;
using Duke.Comparators;
using Duke.Matchers;
using Duke.Utils;
using Lucene.Net.Index;
using NLog;

namespace Duke
{
    /// <summary>
    /// The class that implements the actual deduplication and record
    /// linkage logic.
    /// </summary>
    public class Processor
    {
        #region Private member variables

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static int _defaultBatchSize = 40000;
        private readonly double[] _accprob;
        private readonly IMatchListener _choosebest;
        private readonly Configuration _config;
        private readonly List<IMatchListener> _listeners;
        private readonly IMatchListener _passthrough;

        private readonly List<Property> _proporder;
        protected IDatabase _database;

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
                Records it2 = dataSource.GetRecords();
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
                foreach (IRecord record in records)
                {
                    _database.Index(record);
                }

                _database.Commit();

                // then match
                foreach (IRecord record in records)
                {
                    //Match(record, _passthrough);
                }

                foreach (IMatchListener matchListener in _listeners)
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
            Link(_config.GetDataSources(1), _config.GetDataSources(2), _defaultBatchSize);
        }

        public void Link(List<IDataSource> sources1, List<IDataSource> sources2, int batch_size)
        {
            // first, index up group 1
            //Index(sources1, batch_size);

            // second, traverse group 2 to look for matches with group 1
            //LinkRecords(sources2, _choosebest);
        }

        public void LinkRecords(List<IDataSource> sources)
        {
            LinkRecords(sources, _passthrough);
        }

        public void LinkRecords(List<IDataSource> sources, bool matchall)
        {
            LinkRecords(sources, matchall ? _passthrough : _choosebest);
        }

        private void LinkRecords(List<IDataSource> sources, IMatchListener filter)
        {
            foreach (IDataSource dataSource in sources)
            {
                dataSource.SetLogger();

                IEnumerator<IRecord> it = dataSource.GetRecords().GetEnumerator();

                while (it.MoveNext())
                {
                    IRecord record = it.Current;
                    //Match(record, filter);
                }
            }

            foreach (IMatchListener matchListener in _listeners)
            {
                matchListener.EndProcessing();
            }
        }

        /// <summary>
        /// Index all new records from the given data sources. 
        /// This method does <em>not</em> do any matching. @since 0.4
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="batch_size"></param>
        public void Index(List<IDataSource> sources, int batch_size)
        {
            int count = 0;

            foreach (IDataSource dataSource in sources)
            {
                //dataSource.SetLogger();

                IEnumerator<IRecord> it2 = dataSource.GetRecords().GetEnumerator();
                while (it2.MoveNext())
                {
                    IRecord record = it2.Current;
                    _database.Index(record);
                    count++;
                    if (count%batch_size == 0)
                    {
                        foreach (IMatchListener matchListener in _listeners)
                        {
                            matchListener.BatchReady(batch_size);
                        }
                    }
                }
            }

            if (count%batch_size == 0)
            {
                foreach (IMatchListener matchListener in _listeners)
                {
                    matchListener.BatchReady(count%batch_size);
                }
            }

            _database.Commit();
        }

        private void Match(IRecord record, IMatchListener filter)
        {
            List<IRecord> candidates = _database.FindCandidateMatches(record);
            logger.Debug("Match record {0} found {0} candidates", PrintMatchListener.RecordToString(record),
                         candidates.Count);
            CompareCandidates(record, candidates, filter);
        }

        protected void CompareCandidates(IRecord record, List<IRecord> candidates, IMatchListener filter)
        {
            filter.StartRecord(record);
            foreach (IRecord candidate in candidates)
            {
                if (IsSameAs(record, candidate))
                    continue;

                double prob = Compare(record, candidate);
                if (prob > _config.Threshold)
                {
                    filter.Matches(record, candidate, prob);
                }
                else if ((_config.ThresholdMaybe != 0.0) && (prob > _config.ThresholdMaybe))
                {
                    filter.MatchesPerhaps(record, candidate, prob);
                }
            }

            filter.EndRecord();
        }

        public double Compare(IRecord r1, IRecord r2)
        {
            double prob = 0.5;
            foreach (string propname in r1.GetProperties())
            {
                Property prop = _config.GetPropertyByName(propname);
                if (prop.IsIdProperty || prop.IsIgnoreProperty())
                    continue;

                List<string> vs1 = r1.GetValues(propname);
                List<string> vs2 = r2.GetValues(propname);
                if ((vs1.Count == 0) || (vs2.Count == 0))
                    continue; // no values to compare, so skip

                double high = 0.0;
                foreach (string v1 in vs1)
                {
                    if (v1.Equals("")) //TODO: These values shouldn't be here at all.
                        continue;

                    foreach (string v2 in vs2)
                    {
                        if (v2.Equals("")) //TODO: These values shouldn't be here at all.
                            continue;

                        try
                        {
                            double p = prop.Compare(v1, v2);
                            high = Math.Max(high, p);
                        }
                        catch (Exception e)
                        {
                            throw new DukeException(String.Format("Comparison of values {0} and {1} failed. {2}", v1, v2,
                                                                  e.Message));
                        }
                    }
                }

                prob = StandardUtils.ComputeBayes(prob, high);
            }

            return prob;
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