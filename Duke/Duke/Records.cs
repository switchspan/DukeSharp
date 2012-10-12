using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Special Iterator class for Record collections, in order to add some 
    /// extra methods for resource management.
    /// </summary>
    public abstract class Records : IEnumerable<IRecord>
    {
        private readonly IRecord[] _records;

        protected Records()
        {
            _records = new IRecord[0];
        }

        protected Records(IRecord[] rArray)
        {
            _records = new IRecord[rArray.Length];

            for (int i = 0; i < rArray.Length; i++)
            {
                _records[i] = rArray[i];
            }
        }

        public IEnumerator<IRecord> GetEnumerator()
        {
            return new RecordsEnum(_records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Releases any resources held by this iterator, and cleans up any temp storage
        /// </summary>
        public virtual void Close() {}

        /// <summary>
        /// Informs the iterator that the latest batch of records retrieved from the iterator 
        /// has been processed. This may in some cases allow iterators to free resources,
        /// but iterators are not required to perform any action in response to this call.
        /// </summary>
        public virtual void BatchProcessed() {}

        public void Remove()
        {
            throw new Exception("This operation is not supported.");
        }
    }
}
