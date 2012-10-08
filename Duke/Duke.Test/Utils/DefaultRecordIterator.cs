using System.Collections.Generic;

namespace Duke.Test.Utils
{
    public class DefaultRecordIterator
    {
        private readonly IEnumerator<IRecord> _it;
        
        public DefaultRecordIterator(IEnumerator<IRecord> it)
        {
            _it = it;
        }

        public bool HasNext()
        {
            return true; //HACK: IEnumerator uses lazy loading, so we don't really have this...
        }

        public IRecord Next()
        {
            _it.MoveNext();
            return _it.Current;
        }
    }
}
