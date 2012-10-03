using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Holds the configuration details for a dataset.
    /// </summary>
    public class Configuration
    {
        #region Private member variables
        // there are two modes: deduplication and record linkage. in
        // deduplication mode all sources are in 'datasources'. in record
        // linkage mode they are in 'group1' and 'group2'. couldn't think
        // of a better solution. sorry.
        private List<IDataSource> _datasources;
        private List<IDataSource> _group1;
        private List<IDataSource> _group2;

        private String _path;
        private double _threshold;
        private double _thresholdMaybe;

        private Dictionary<String, Property> _properties;
        private List<Property> _proplist; // duplicate to preserve order
        private List<Property> _lookups; // subset of properties

        private DatabaseProperties _dbprops;

        #endregion

        #region Constructors
        public Configuration()
        {
            _datasources = new List<IDataSource>();
            _group1 = new List<IDataSource>();
            _group2 = new List<IDataSource>();
            _dbprops = new DatabaseProperties();
        }
        #endregion

        #region Member methods
        /// <summary>
        /// Returns the data sources to use (in deduplication mode; don't use
        /// this method in record linkage mode).
        /// </summary>
        /// <returns></returns>
        public List<IDataSource> GetDataSources()
        {
            return _datasources;
        } 

        /// <summary>
        /// Returns the data sources belonging to a particular group of data
        /// sources. Data sources are grouped in record linkage mode, but not
        /// in deduplication mode, so only use this method in record linkage
        /// mode.
        /// </summary>
        /// <param name="groupno"></param>
        /// <returns></returns>
        public List<IDataSource> GetDataSources(int groupno)
        {
            if (groupno == 1)
            {
                return _group1;
            }

            if (groupno == 2)
            {
                return _group2;
            }

            throw new Exception(String.Format("Invalid group number: {0}", groupno));
        } 

        /// <summary>
        /// Adds a data source to the configuration. If in deduplication mode
        /// groupno == 0, otherwise it gives the number of the group to which
        /// the data source belongs.
        /// </summary>
        /// <param name="groupno"></param>
        /// <param name="datasource"></param>
        public void AddDataSource(int groupno, IDataSource datasource)
        {
            // the load takes care of validation
            if (groupno == 0)
            {
                _datasources.Add(datasource);
            }
            if (groupno == 1)
            {
                _group1.Add(datasource);
            }
            if (groupno == 2)
            {
                _group2.Add(datasource);
            }
        }

        /// <summary>
        /// The path to the Lucene index directory. If null or not set, it
        /// means the Lucene index is kept in-memory.
        /// </summary>
        /// <param name="path"></param>
        public void SetPath(String path)
        {
            _path = path;
        }

        /// <summary>
        /// FIXME: means we can create multiple ones. not a good idea.
        /// </summary>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        //public IDatabase CreateDatabase(Boolean overwrite)
        //{
        //    if (_dbprops.GetDatabaseImplementation() ==
        //        DatabaseImplementation.IN_MEMORY_DATABASE)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}

        //TODO: Finish code here.
        #endregion


    }
}
