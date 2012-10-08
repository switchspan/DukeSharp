using System;
using System.IO;
using System.Text;

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

        public RecordIterator GetRecords()
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
                    if (FileEncoding == null)
                    {
                        sr = new StreamReader(File);
                    }
                    else
                    {
                        sr = new StreamReader(File, FileEncoding);
                    }
                }

                return new 
            }
        }

        protected override string GetSourceName()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}