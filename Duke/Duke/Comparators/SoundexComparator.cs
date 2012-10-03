using System;

namespace Duke.Comparators
{
    /// <summary>
    /// An implementation of the Soundex algorithm, and a comparator which
    /// considers strings to have a score of 0.9 if their Soundex values
    /// match.
    /// </summary>
    public class SoundexComparator : IComparator
    {
        #region Private member variables

        private static char[] _number;

        #endregion

        #region Constructors

        public SoundexComparator()
        {
            _number = BuildTable();
        }

        #endregion

        #region Member methods

        public bool IsTokenized()
        {
            return true; // I guess?
        }

        public double Compare(string v1, string v2)
        {
            if (v1.Equals(v2))
                return 1.0;

            if (Soundex(v1).Equals(Soundex(v2)))
                return 0.9;

            return 0.0;
        }

        /// <summary>
        /// Produces the Soundex key for the given string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static String Soundex(String str)
        {
            if (str.Length < 1)
                return ""; // no soundex key for the empty string (could use 000)

            var key = new char[4];
            key[0] = str[0];
            int pos = 1;
            char prev = '0';
            for (int ix = 1; ix < str.Length && pos < 4; ix++)
            {
                char ch = str[ix];
                int charno;
                if (ch >= 'A' && ch <= 'Z')
                    charno = ch - 'A';
                else if (ch >= 'a' && ch <= 'z')
                    charno = ch - 'a';
                else
                    continue;

                if (_number[charno] != '0' && _number[charno] != prev)
                    key[pos++] = _number[charno];
                prev = _number[charno];
            }

            for (; pos < 4; pos++)
                key[pos] = '0';

            return new String(key);
        }

        private static char[] BuildTable()
        {
            var table = new char[26];
            for (int ix = 0; ix < table.Length; ix++)
                table[ix] = '0';
            table['B' - 'A'] = '1';
            table['P' - 'A'] = '1';
            table['F' - 'A'] = '1';
            table['V' - 'A'] = '1';
            table['C' - 'A'] = '2';
            table['S' - 'A'] = '2';
            table['K' - 'A'] = '2';
            table['G' - 'A'] = '2';
            table['J' - 'A'] = '2';
            table['Q' - 'A'] = '2';
            table['X' - 'A'] = '2';
            table['Z' - 'A'] = '2';
            table['D' - 'A'] = '3';
            table['T' - 'A'] = '3';
            table['L' - 'A'] = '4';
            table['M' - 'A'] = '5';
            table['N' - 'A'] = '5';
            table['R' - 'A'] = '6';
            return table;
        }

        #endregion
    }
}