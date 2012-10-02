using System;
using System.Text.RegularExpressions;

namespace Duke
{
    public class Transform
    {
        #region Private member variables

        private int _groupno;
        private Regex _regex;
        private String _replacement;

        #endregion

        #region Constructors

        public Transform(String regex, String replacement) : this(regex, replacement, 1)
        {
        }

        public Transform(String regex, String replacement, int groupno)
        {
            _regex = new Regex(regex, RegexOptions.Compiled);
            _replacement = replacement;
            _groupno = groupno;
        }

        #endregion

        #region Member methods

        public String TransformValue(String value)
        {
            Match match = _regex.Match(value);

            if (!match.Success) return value;

            return value.Substring(0, match.Groups[_groupno].Index) +
                   _replacement +
                   value.Substring((match.Groups[_groupno].Length - 1), value.Length);
            
            //TODO: Need to look and see if the is the most effective way to do this with c# regex expressions...
        }

        #endregion
    }
}