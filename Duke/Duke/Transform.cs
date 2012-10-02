using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Duke
{
    public class Transform
    {
        private Regex _regex;
        private String _replacement;
        private int _groupno;

        public Transform(String regex, String replacement) : this(regex, replacement, 1){}

        public Transform(String regex, String replacement, int groupno)
        {
            _regex = new Regex(regex, RegexOptions.Compiled);
            _replacement = replacement;
            _groupno = groupno;
        }

        public String TransformValue(String value)
        {

            return "";
        }
    }
}
