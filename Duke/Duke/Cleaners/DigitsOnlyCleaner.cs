using System.Text.RegularExpressions;

namespace Duke.Cleaners
{
    /// <summary>
    /// Cleaner which removes all characters except the digits 0-9.
    /// </summary>
    public class DigitsOnlyCleaner : ICleaner
    {
        private readonly Regex _pattern = new Regex("[0-9]+", RegexOptions.Compiled);

        #region ICleaner Members

        public string Clean(string value)
        {
            //var tmp = new char[value.Length];
            //int pos = 0;
            //for (int ix = 0; ix < tmp.Length; ix++)
            //{
            //    char ch = value[ix];
            //    if (ch >= '0' && ch <= '9')
            //        tmp[pos++] = ch;
            //}

            //return tmp.ToString();

            Match matches = _pattern.Match(value);
            return matches.Value;
        }

        #endregion
    }
}