using System;
using Duke.Utils;

namespace Duke.Comparators
{
    /// <summary>
    /// An implementation of the Dice coefficient using exact matching by
    /// default, but can be overriden to use any sub-comparator.
    /// </summary>
    public class DiceCoefficientComparator : IComparator
    {
        #region Private members

        private IComparator _subcomp;

        #endregion

        #region Constructors

        public DiceCoefficientComparator()
        {
            _subcomp = new ExactComparator();
        }

        #endregion

        #region Member methods

        public bool IsTokenized()
        {
            return true;
        }

        public double Compare(string v1, string v2)
        {
            if (v1.Equals(v2))
                return 1.0;

            // tokenize
            String[] t1 = StringUtils.Split(v1);
            String[] t2 = StringUtils.Split(v2);

            // ensure that t1 is shorter than or same length as t2
            if (t1.Length > t2.Length)
            {
                String[] tmp = t2;
                t2 = t1;
                t1 = tmp;
            }

            // find best matches for each token in t1
            double sum = 0;
            for (int ix1 = 0; ix1 < t1.Length; ix1++)
            {
                double highest = 0;
                for (int ix2 = 0; ix2 < t2.Length; ix2++)
                {
                    highest = Math.Max(highest, _subcomp.Compare(t1[ix1], t2[ix2]));
                }
                sum += highest;
            }

            return (sum*2)/(t1.Length + t2.Length);
        }

        public void SetComparator(IComparator comp)
        {
            _subcomp = comp;
        }

        #endregion
    }
}