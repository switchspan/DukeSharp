using System;

namespace Duke
{
    class Column
    {
        private readonly String _name;
        private readonly String _property;
        private readonly String _prefix;
        private readonly ICleaner _cleaner;

        public Column(String name, String property, String prefix, ICleaner cleaner)
        {
            _name = name;
            _property = property;
            _prefix = prefix;
            _cleaner = cleaner;
        }

        public String GetName()
        {
            return _name;
        }

        public String GetProperty()
        {
            if (_property == null)
            {
                return _name;
            }
            
            return _property;
        }

        public String GetPrefix()
        {
            return _prefix;
        }

        public ICleaner GetCleaner()
        {
            return _cleaner;
        }


    }
}
