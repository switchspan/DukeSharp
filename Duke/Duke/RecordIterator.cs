using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Special Iterator class for Record collections, in orde3r to add some 
    /// extra methods for resource management.
    /// </summary>
    public abstract class RecordIterator : IEnumerable<IRecord>
    {
        public IEnumerator<IRecord> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases any resources held by this iterator, and cleans up any temp storage
        /// </summary>
        public virtual void Close() {}

        /// <summary>
        /// Informs the iterator taht the lates batch of records retrieved from teh iterator 
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
