using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Cleaners
{
    /// <summary>
    /// A cleaner which removes leading and trailing whitespace, normalized
    /// internal whitespace, lowercases all characters, and (by default)
    /// strips accents. This is the default cleaner for textual data.   
    /// </summary>
    public class LowerCaseNormalizeCleaner : ICleaner
    {
        private bool _stripAccents = true;

        /// <summary>
        /// Controls whether accents are stripped (that is, "é" becomes "e",
        /// and so on). The default is true.
        /// </summary>
        /// <param name="stripAccents"></param>
        public void SetStripAccents(bool stripAccents)
        {
            _stripAccents = stripAccents;
        }


        public string Clean(string value)
        {
            if (_stripAccents)
                value.Normalize(NormalizationForm.FormD);

            var tmp = new char[value.Length];
            int pos = 0;
            bool prevws = false;
            for (int ix = 0; ix < tmp.Length; ix++)
            {
                var ch = value[ix];

                // if character is combining diacritical mark, skip it
                if ((ch >= 0x0300 && ch <= 0x036F) ||
                    (ch >= 0x1DC0 && ch <= 0x1DFF) ||
                    (ch >= 0x20D0 && ch <= 0x20FF) ||
                    (ch >= 0xFE20 && ch <= 0xFE2F))
                    continue;

                // whitespace processing
                if (ch != ' ' && ch != '\t' && ch != '\n' && ch != '\r')
                {
                    if (prevws && pos != 0)
                        tmp[pos++] = ' ';

                    tmp[pos++] = Char.ToLower(ch);

                    prevws = false;
                }
                else
                {
                    prevws = true;
                }
            }

            return tmp.ToString();
        }
    }
}
