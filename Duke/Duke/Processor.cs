using System.Collections.Generic;
using Duke.Matchers;

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
        private double[] _accprob;
        private IMatchListener _choosebest;
        private Configuration _config;
        protected IDatabase _database;
        private List<IMatchListener> _listeners;

        private IMatchListener _passthrough;
        private List<Property> _proporder;

        #endregion

        #region Constructors
        public Processor(Configuration config) : this(config, true) { }

        public Processor(Configuration config, bool overwrite) : this(config, config.CreateDatabase(overwrite)) { }

        public Processor(Configuration config, IDatabase database)
        {
            _config = config;
            _database = database;
            _listeners = new List<IMatchListener>();
            
            

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

        // Commits all state to disk and frees up resources
        public void Close()
        {
            _database.Close();
        }

        // Internals
        private bool IsSameAs(IRecord r1, IRecord r2)
        {
            foreach (var idp in _config.GetIdentityProperties())
            {
                List<string> vs2 = r2.GetValues(idp.Name);
                List<string> vs1 = r1.GetValues(idp.Name);
                if (vs1 == null)
                    continue;
                foreach (var v1 in vs1)
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
            foreach (var matchListener in _listeners)
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
            foreach (var matchListener in _listeners)
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
            foreach (var matchListener in _listeners)
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
            foreach (var matchListener in _listeners)
            {
                matchListener.NoMatchFor(current);
            }
        }

        /// <summary>
        /// Notifies listeners that we finished this record.
        /// </summary>
        private void RegisterEndRecord()
        {
            foreach (var matchListener in _listeners)
            {
                matchListener.EndRecord();
            }
        }
        #endregion
    }
}