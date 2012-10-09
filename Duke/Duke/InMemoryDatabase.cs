using System;
using System.Collections.Generic;

namespace Duke
{
    public class InMemoryDatabase : IDatabase
    {
        #region Private member variables

        private readonly Configuration _config;
        private readonly Dictionary<String, IRecord> _idindex;
        private readonly List<IRecord> _records;

        #endregion

        #region Constructors

        public InMemoryDatabase(Configuration config)
        {
            _config = config;
            _idindex = new Dictionary<string, IRecord>();
            _records = new List<IRecord>();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns true if the database is held entirely in memory, and
        /// this is not persistent.
        /// </summary>
        /// <returns></returns>
        public bool IsInMemory()
        {
            return true;
        }

        /// <summary>
        /// Add the record to the index.
        /// </summary>
        /// <param name="record"></param>
        public void Index(IRecord record)
        {
            foreach (Property p in _config.GetIdentityProperties())
            {
                List<String> values = record.GetValues(p.Name);
                if (values == null)
                    continue;

                foreach (string value in values)
                {
                    _idindex.Add(value, record);
                }
            }

            _records.Add(record);
        }

        /// <summary>
        /// Flushes all changes to disk. For in-memory databases this is a no-op.
        /// </summary>
        public void Commit()
        {
            //Do nothing...
        }

        /// <summary>
        /// Look up record by identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IRecord FindRecordById(string id)
        {
            return _idindex[id];
        }

        public List<IRecord> FindCandidateMatches(IRecord record)
        {
            //HACK: This doesn't even use the param...need to have a look at this...
            return _records;
        }

        /// <summary>
        /// Stores state to disk and closes all open resources.
        /// </summary>
        public void Close()
        {
            // Do nothing...
        }

        #endregion
    }
}