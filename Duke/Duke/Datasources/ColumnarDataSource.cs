using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Duke.Datasources
{
    public abstract class ColumnarDataSource : IDataSource
    {
        #region Private member variables

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<String, List<Column>> _columns;

        #endregion

        #region Constructors

        public ColumnarDataSource()
        {
            _columns = new Dictionary<string, List<Column>>();
        }

        #endregion

        #region Member methods

        public abstract RecordIterator GetRecords();

        public abstract void SetLogger();
        
        public void AddColumn(Column column)
        {
            string columnKey = column.GetName();

            if (!_columns.ContainsKey(columnKey))
            {
                var cols = new List<Column>();
                cols.Add(column);
                _columns.Add(columnKey, cols);
            }

            _columns[columnKey].Add(column);
        }

        public List<Column> GetColumn(string name)
        {
            return _columns.ContainsKey(name) ? _columns[name] : null;
        }

        public List<Column> GetColumns()
        {
            List<Column> allColumns = _columns.Values.SelectMany(col => col).ToList();
            return allColumns;
        }

        protected abstract string GetSourceName();

        protected void VerifyProperty(string value, string name)
        {
            if (value == null)
            {
                string errorMessage = String.Format("Missing {0} property to {1} data source.", name,
                                                    GetSourceName());
                _logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }
        }

        #endregion
    }
}