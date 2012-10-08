using System;
using System.Collections.Generic;
using System.Linq;

namespace Duke.Datasources
{
    /// <summary>
    /// Helper class for building records, to avoid having to copy all the
    /// cleaning logic etc in each single data source.
    /// </summary>
    public class RecordBuilder
    {
        #region Private member variables

        private readonly ColumnarDataSource _source;
        private Dictionary<string, List<string>> _record;

        #endregion

        #region Constructors

        public RecordBuilder(ColumnarDataSource source)
        {
            _source = source;
        }

        #endregion

        #region Member methods

        public void NewRecord()
        {
            _record = new Dictionary<string, List<string>>();
        }

        public bool IsRecordEmpty()
        {
            return (_record == null || _record.Count == 0);
        }

        public void AddValue(string column, string value)
        {
            List<Column> cols = _source.GetColumn(column);
            if (cols != null || !(cols.Count == 0))
            {
                Column col = cols.Last();
                AddValue(col, value);
            }
        }

        public void AddValue(Column col, string value)
        {
            if (String.IsNullOrEmpty(value))
                return;

            if (col.GetCleaner() != null)
                value = col.GetCleaner().Clean(value);

            if (String.IsNullOrEmpty(value))
                return; // nothing here, move on

            String prop = col.GetProperty();
            List<String> values = _record[prop];
            if (values == null)
            {
                values = new List<string>();
                _record.Add(prop, values);
            }

            values.Add(value);
        }

        public void SetValue(string column, string value)
        {
            var cols = _source.GetColumn(column);
            Column col = cols.Last();
            SetValue(col, value);
        }

        public void SetValue(Column col, string value)
        {
            if (col.GetCleaner() != null)
                value = col.GetCleaner().Clean(value);
            if (String.IsNullOrEmpty(value))
                return; // nothing here, move on

            _record.Add(col.GetProperty(), new List<string> {value});
        }

        public IRecord GetRecord()
        {
            return new RecordImpl(_record);
        }

        #endregion
    }
}