using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Cleaners
{
    /// <summary>
    /// A cleaner which simply returns the input string.
    /// </summary>
    public class NoCleaningCleaner : ICleaner
    {
        public string Clean(string value)
        {
            return value;
        }
    }
}
