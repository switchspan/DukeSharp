using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Cleaners
{
    /// <summary>
    /// A cleaner which returns values as they are, but removes specific
    /// values. This is useful in cases where users have entered so-called
    /// "generic values". For example, if the unknown company number is
    /// always set as "999999999", then you can use this cleaner to remove
    /// that specific value.
    /// </summary>
    public class GenericValueCleaner : ICleaner
    {
        private String _generic;
        private ICleaner _sub;

        public string Clean(string value)
        {
            if (_sub != null)
                value = _sub.Clean(value);
            if (_generic.Equals(value))
                return null;

            return value;
        }

        public void SetGeneric(String generic)
        {
            _generic = generic;
        }

        public void SetSubCleaner(ICleaner sub)
        {
            _sub = sub;
        }

    }
}
