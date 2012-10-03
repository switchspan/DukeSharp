using System;
using Duke.Utils;

namespace Duke.Cleaners
{
    public class NorwegianCompanyNameCleaner : AbstractRuleBasedCleaner
    {
        #region Private member variables

        private readonly LowerCaseNormalizeCleaner _sub;

        #endregion

        #region Constructors
        public NorwegianCompanyNameCleaner()
        {
            _sub = new LowerCaseNormalizeCleaner();

            Add("\\s(a/s)(\\s|$)", "as");
            Add("\\s(a\\\\s)(\\s|$)", "as");
            Add("^(a/s)\\s", "as");
            Add("^(a\\\\s)\\s", "as");
            Add("\\s(a/l)(\\s|$)", "al");
            Add("^(a/l)\\s", "al");

        }
        #endregion

        #region Member methods
        public new String Clean(String value)
        {
            // get rid of commas
            value = StringUtils.ReplaceAnyOf(value, ",().-_", ' ');

            // do basic cleaning
            value = _sub.Clean(value);
            if (String.IsNullOrEmpty(value))
                return "";

            // perform pre-registered transforms
            value = base.Clean(value);

            // renormalize whitespace, since being able to replace tokens with spaces
            // makes writing transforms easier
            value = StringUtils.NormalizeWs(value);

            // transforms:
            // "as foo bar" -> "foo bar as"
            // "al foo bar" -> "foo bar al"
            if (value.StartsWith("as ") || value.StartsWith("al "))
                value = value.Substring(3) + ' ' + value.Substring(0, 2);

            return value;
        }

        #endregion
    }
}
