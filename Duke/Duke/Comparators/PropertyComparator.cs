using System.Collections.Generic;

namespace Duke.Comparators
{
    /// <summary>
    /// Sorts properties so that the properties with the lowest low
    /// probabilities come first.
    /// </summary>
    public class PropertyComparator : IComparer<Property>
    {
        #region IComparer<Property> Members

        int IComparer<Property>.Compare(Property p1, Property p2)
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

        #endregion
    }
}