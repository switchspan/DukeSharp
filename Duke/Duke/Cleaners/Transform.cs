using System;
using System.Text.RegularExpressions;

namespace Duke
{
    public class Transform
    {
        #region Private member variables

        private readonly Regex _regex;
        private readonly String _replacement;

        #endregion

        #region Constructors

        public Transform(String regex, String replacement)
        {
            _regex = new Regex(regex, RegexOptions.Compiled);
            _replacement = replacement;
        }

        #endregion

        #region Member methods

        public String TransformValue(String value)
        {
            string result = _regex.Replace(value, _replacement);
            return result;
        }

        #endregion
    }
}