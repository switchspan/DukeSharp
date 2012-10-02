using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    public class DigitsOnlyCleaner : ICleaner
    {

        public string Clean(string value)
        {
            var tmp = value.ToCharArray();
            int pos = 0;
            for (int ix = 0; ix < tmp.Length; ix++)
            {
                char ch = tmp[ix];
                if (ch >= '0' && ch <= '9')
                    tmp[pos++] = ch;
            }

            return tmp.ToString();
        }
    }
}
