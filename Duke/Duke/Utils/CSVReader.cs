using System.IO;

namespace Duke.Utils
{
    public class CsvReader
    {
        #region Private member variables

        private readonly LumenWorks.Framework.IO.Csv.CsvReader _reader;
        private readonly StreamReader _sr;

        #endregion

        #region Constructors

        public CsvReader(StreamReader sr)
        {
            _sr = sr;
            _reader = new LumenWorks.Framework.IO.Csv.CsvReader(_sr, false);
        }

        #endregion

        #region Member methods

        public string[] Next()
        {
            if (_reader.ReadNextRecord())
            {
                int fieldCount = _reader.FieldCount;
                var row = new string[fieldCount];

                for (int i = 0; i < fieldCount; i++)
                {
                    row[i] = _reader[i];
                }

                return row;
            }
            return null;
        }


        public void Close()
        {
            _sr.Close();
        }

        #endregion
    }
}