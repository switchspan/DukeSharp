using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Datasources
{
    public class CsvRecordEnumerator : IEnumerator<IRecord>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IRecord Current
        {
            get { throw new NotImplementedException(); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
