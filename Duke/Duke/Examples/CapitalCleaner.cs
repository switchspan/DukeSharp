using System;
using Duke.Cleaners;

namespace Duke.Examples
{
    public class CapitalCleaner : ICleaner
    {
        #region Private member variables

        private readonly LowerCaseNormalizeCleaner _sub;

        #endregion

        #region Constructors

        public CapitalCleaner()
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
            int ix = value.IndexOf(',');
            if (ix != -1)
            {
                value = value.Substring(0, ix);
            }

            return value;
        }

        #endregion
    }
}