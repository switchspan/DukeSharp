using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Comparators
{
    /// <summary>
    /// Sorts properties so that the properties with the lowest low
    /// probabilities come first.
    /// </summary>
    public static class PropertyComparator
    {
        public static int Compare(Property p1, Property p2)
        {
            double diff = p1.LowProbability = p2.LowProbability;
            
            if (diff < 0)
            {
                return -1;
            }
            
            if (diff > 0)
            {
                return 1;
            }

            return 0;
        }

        public static Boolean IsTokenized()
        {
            return false;
        }
    }
}
