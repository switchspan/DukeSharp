using System;
using System.IO;
using System.Text;
using Duke.Utils;

namespace Duke.Datasources
{
    public class CsvDataSource : ColumnarDataSource
    {
        #region Private member variables

        private StreamReader _directreader; // overrides 'file'; used for testing
        
        #endregion

        #region Member Properties

        public Encoding FileEncoding { get; set; }
        public string File { get; set; }
        public int SkipLines { get; set; }
        public bool HasHeader { get; set; }
        #endregion

        #region Constructors
        public CsvDataSource()
        {
            HasHeader = true;
        }
        #endregion

        #region Member methods

        public void SetReader(StreamReader reader)
        {
            _directreader = reader;
        }

        public override RecordIterator GetRecords()
        {
            if (_directreader == null)
            {
                VerifyProperty(File, "input-file");
            }

            try
            {
                StreamReader sr;
                if (_directreader != null)
                {
                    sr = _directreader;
                }
                else
                {
                    sr = FileEncoding == null ? new StreamReader(File) : new StreamReader(File, FileEncoding);
                }

                return new CsvRecordIterator(this, new CsvReader(sr));
            }
            catch (FileNotFoundException e)
            {
                throw new DukeConfigException(String.Format("Couldn't find CSV file '{0}' {1}", File, e.Message));
            }
        }

        public override void SetLogger()
        {
            throw new NotImplementedException();
        }

        protected override string GetSourceName()
        {
            return "CSV";
        }

        #endregion
    }
}