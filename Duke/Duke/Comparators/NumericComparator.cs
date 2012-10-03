using System;

namespace Duke.Comparators
{
    /// <summary>
    /// Comparator which compares two values numerically. The similarity is
    /// the ratio of the smaller number to the greater number.
    /// </summary>
    public class NumericComparator : IComparator
    {
        #region Member properties

        public double MinRatio { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region Member methods

        public bool IsTokenized()
        {
            return false;
        }

        public double Compare(string v1, string v2)
        {
            double d1;
            double d2;

            try
            {
                d1 = Double.Parse(v1);
                d2 = Double.Parse(v2);
            }
            catch (FormatException)
            {
                //TODO: See if there are other exceptions that need to be caught (http://msdn.microsoft.com/en-us/library/7yd1h1be.aspx)
                return 0.5;
            }

            // if they're both zero, they're equal
            if (d1 == 0.0 && d2 == 0.0)
                return 1.0;

            if (d2 < d1)
            {
                double tmp = d2;
                d2 = d1;
                d1 = tmp;
            }

            double ratio = d1/d2;
            if (ratio < MinRatio)
            {
                return 0.0;
            }
            return ratio;
        }

        #endregion
    }
}