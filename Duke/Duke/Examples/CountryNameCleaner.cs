using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duke.Cleaners;

namespace Duke.Examples
{
    public class CountryNameCleaner : ICleaner
    {
        #region Private member variables

        private LowerCaseNormalizeCleaner _sub;

        #endregion

        #region Constructors
        public CountryNameCleaner()
        {
            _sub = new LowerCaseNormalizeCleaner();
        }
        #endregion

        #region Member methods
        public string Clean(string value)
        {
            // do basic cleaning
            value = _sub.Clean(value);
            if (String.IsNullOrEmpty(value))
                return "";

            // do our stuff
            if (value.StartsWith("the "))
                value = value.Substring(4);

            return value;
        }
        #endregion


        
    }
}
