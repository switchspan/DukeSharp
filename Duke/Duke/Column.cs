using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    class Column
    {
        private String _name;
        private String _property;
        private String _prefix;
        private ICleaner _cleaner;

        public Column(String name, String property, String prefix, ICleaner cleaner)
        {
            _name = name;
            _property = property;
            _prefix = prefix;
            _cleaner = cleaner;
        }

        public String getName()
        {
            return _name;
        }

        public String getProperty()
        {
            if (_property == null)
            {
                return _name;
            }
            else
            {
                return _property;
            }
        }

        public String getPrefix()
        {
            return _prefix;
        }

        public ICleaner getCleaner()
        {
            return _cleaner;
        }


    }
}
