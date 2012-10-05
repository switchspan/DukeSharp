using System;
using System.Text;

namespace Duke.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// Replaces any of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="chars">The chars.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static String ReplaceAnyOf(String value, String chars, char replacement)
        {
            var tmp = new char[value.Length];
            int pos = 0;
            foreach (char ch in value)
            {
                if (chars.IndexOf(ch) != -1)
                {
                    tmp[pos++] = replacement;
                }
                else
                {
                    tmp[pos++] = ch;
                }
            }

            Array.Resize(ref tmp, pos);

            return new string(tmp);
        }

        /// <summary>
        /// Removes trailing and leading whitespace, and also reduces each 
        /// sequence of internal whitespace to a single space.
        /// </summary>
        /// <param name="value">The string to trim whitespace from</param>
        /// <returns></returns>
        public static String NormalizeWs(String value)
        {
            //TODO: See if this or the previous implementation in ReplaceAnyOf is faster
            var tmp = new char[value.Length];
            int pos = 0;
            bool prevws = false;
            for (int ix = 0; ix < tmp.Length; ix++)
            {
                char ch = value[ix];
                if (ch != ' ' && ch != '\t' && ch != '\n' && ch != '\r' && ch != '\0')
                {
                    if (prevws && pos != 0)
                        tmp[pos++] = ' ';

                    tmp[pos++] = ch;
                    prevws = false;
                }
                else
                {
                    prevws = true;
                }
            }

            Array.Resize(ref tmp, pos); // resize the array so we get rid of the null characters...

            //var result = new string(tmp).Replace("\0", string.Empty); // remove the null characters.
            var result = new string(tmp);

            return result;
        }

        /// <summary>
        /// Splits the specified string.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="charToSplitOn">The char to split on.</param>
        /// <returns></returns>
        public static String[] Split(String str, char charToSplitOn = ' ')
        {
            //var tokens = new String[(str.Length / 2) + 1];
            //int start = 0;
            //int tcount = 0;
            //bool prevws = false;
            //int ix;
            //for (ix = 0; ix < str.Length; ix++)
            //{
            //    if (str[ix] == ' ')
            //    {
            //        if (!prevws && ix > 0)
            //            tokens[tcount++] = str.Substring(start, ix);
            //        prevws = true;
            //        start = ix + 1;
            //    }
            //    else
            //    {
            //        prevws = false;
            //    }
            //}

            //if (!prevws && start != ix)
            //    tokens[tcount++] = str.Substring(start);

            //var tmp = new String[tcount];
            //for (ix = 0; ix < tcount; ix++)
            //    tmp[ix] = tokens[ix];

            //return tmp;

            return str.Split(charToSplitOn);
        }

        /// <summary>
        /// Joins the specified pieces.
        /// </summary>
        /// <param name="pieces">The pieces.</param>
        /// <returns></returns>
        public static String Join(String[] pieces)
        {
            var tmp = new StringBuilder();
            for (int ix = 0; ix < pieces.Length; ix++)
            {
                if (ix != 0)
                    tmp.Append(" ");
                tmp.Append(pieces[ix]);
            }

            return tmp.ToString();
        }
    }
}