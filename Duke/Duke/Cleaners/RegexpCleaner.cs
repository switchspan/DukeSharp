using System;
using System.Text.RegularExpressions;

namespace Duke.Cleaners
{
    public class RegexpCleaner : ICleaner
    {
        #region Private member variables

        private int _groupno;
        private Regex _pattern;

        #endregion

        #region Constructors
        //TODO: Check to see if we need to have an alternate for patterns with no grouping (i.e. without '( )' in the string.
        public RegexpCleaner() : this("", 1)
        {
        }


        public RegexpCleaner(string regexPattern) : this(regexPattern, 1)
        {
        }

        public RegexpCleaner(string regexPattern, int groupno)
        {
            _pattern = new Regex(regexPattern, RegexOptions.Compiled);
            _groupno = groupno;
        }

        #endregion

        #region Member methods

        public string Clean(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            Match match = _pattern.Match(value);

            if (match.Success)
            {
                return match.Groups[_groupno].Value;
            }

            return null;
        }

        public void SetRegExp(String regexp)
        {
            _pattern = new Regex(regexp, RegexOptions.Compiled);
        }

        public void SetGroup(int groupno)
        {
            _groupno = groupno;
        }

        #endregion
    }
}