using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// A function which can turn a value into a normalized value suitable for comparison.
    /// </summary>
    interface ICleaner
    {
        String Clean(String value);
    }
}
