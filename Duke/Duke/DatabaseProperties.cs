using System;

namespace Duke
{
    /// <summary>
    /// A class representing configurable properties on the record database.
    /// </summary>
    public class DatabaseProperties
    {
        #region Private member variables

        private DatabaseImplementation _dbtype = DatabaseImplementation.LUCENE_DATABASE;

        #endregion

        #region Member Properties

        // Deichman case:
        //  100 = 2 minutes
        //  1000 = 10 minutes
        //  10000 = 50 minutes
        public int MaxSearchHits { get; set; }
        public float MinRelevance { get; set; }

        #endregion

        #region Constructors

        public DatabaseProperties()
        {
            MaxSearchHits = 10000000;
            MinRelevance = 0.0f;
        }

        #endregion

        #region Member methods

        public void SetDatabaseImplementation(String id)
        {
            _dbtype = DatabaseImplementation.GetById(id);
        }

        public DatabaseImplementation GetDatabaseImplementation()
        {
            return _dbtype;
        }

        #endregion
    }

    public class DatabaseImplementation
    {
        public static readonly DatabaseImplementation LUCENE_DATABASE = new DatabaseImplementation("lucene");
        public static readonly DatabaseImplementation IN_MEMORY_DATABASE = new DatabaseImplementation("in_memory");
        private readonly String _id;

        private DatabaseImplementation(string id)
        {
            _id = id.Trim();
        }

        public static DatabaseImplementation GetById(String id)
        {
            if (id.Equals(LUCENE_DATABASE._id))
            {
                return LUCENE_DATABASE;
            }
            if (id.Equals(IN_MEMORY_DATABASE._id))
            {
                return IN_MEMORY_DATABASE;
            }
            throw new Exception(String.Format("Unknown database type: {0}", id));
        }
    }
}