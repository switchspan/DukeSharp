using System;
using System.Collections.Generic;

namespace Duke
{
    public class RecordsEnum : IEnumerator<IRecord>
    {
        public IRecord[] Records;

        // Enumerators are positioned before the first element
        // util the first MoveNext() call.
        private int _position = -1;

        public RecordsEnum(IRecord[] list)
        {
            Records = list;
        }

        public IRecord Current
        {
            get {
                try
                {
                    return Records[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {
            Records = null;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < Records.Length);
        }

        public void Reset()
        {
            _position = -1;
        }
    }
}
