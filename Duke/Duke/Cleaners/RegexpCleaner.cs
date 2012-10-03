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

        public RegexpCleaner()
        {
            _groupno = 1; //default
        }

        #endregion

        #region Member methods

        public string Clean(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            _pattern = new Regex(value);
            Match matcher = _pattern.Match(value);

            if (!matcher.Success)
                return null;

            return matcher.Groups[_groupno].ToString();
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