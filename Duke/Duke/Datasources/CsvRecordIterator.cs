using System;
using System.IO;
using Duke.Utils;
using NLog;

namespace Duke.Datasources
{
    public class CsvRecordIterator : RecordIterator
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Private member variables

        private readonly RecordBuilder _builder;
        private readonly Column[] _column; // all the columns, in random order
        private readonly int[] _index; // what index in row to find column[ix] value in
        private readonly CsvReader _reader;
        private IRecord _nextrecord;

        #endregion

        #region Constructors

        public CsvRecordIterator(CsvDataSource datasource, CsvReader reader)
        {
            _reader = reader;
            _builder = new RecordBuilder(datasource);

            // index here is random 0-n. index[0] gives the column no in the CSV
            // file, while colname[0] gives the corresponding column name.
            int columnSize = datasource.GetColumns().Count;
            _index = new int[columnSize];
            _column = new Column[columnSize];

            // skip the required number of lines before getting to the data
            for (int ix = 0; ix < datasource.SkipLines; ix++)
            {
                _reader.Next();
            }

            // learn column indexes from header line (if there is one)
            String[] header;
            if (datasource.HasHeader)
            {
                header = _reader.Next();
            }
            else
            {
                // find highest column number
                int high = 0;
                foreach (Column c in datasource.GetColumns())
                {
                    high = Math.Max(high, Int32.Parse(c.GetName()));
                }

                // build corresponding index
                header = new string[high];
                for (int ix = 0; ix < high; ix++)
                {
                    header[ix] = "" + (ix + 1);
                }
            }

            // build the 'index' and 'column' indexes
            int count = 0;
            foreach (Column column in datasource.GetColumns())
            {
                for (int ix = 0; ix < header.Length; ix++)
                {
                    if (header[ix].Equals(column.GetName()))
                    {
                        _index[count] = ix;
                        _column[count++] = column;
                        break;
                    }
                }
            }

            FindNextRecord();
        }

        #endregion

        #region Member methods

        private void FindNextRecord()
        {
            String[] row;

            try
            {
                row = _reader.Next();
                if (row == null)
                {
                    _nextrecord = null; // there isn't any next record
                    return;
                }

                // build a record from the current row
                _builder.NewRecord();
                for (int ix = 0; ix < _column.Length; ix++)
                {
                    if (_index[ix] >= row.Length)
                        break;

                    _builder.AddValue(_column[ix], row[_index[ix]]);
                }

                _nextrecord = _builder.GetRecord();
            }
            catch (IOException e)
            {
                logger.Error("Error finding next record: {0}", e.Message);
            }
        }

        public bool HasNext()
        {
            return (_nextrecord != null);
        }

        public IRecord Next()
        {
            IRecord thenext = _nextrecord;
            FindNextRecord();
            return thenext;
        }

        #endregion
    }
}