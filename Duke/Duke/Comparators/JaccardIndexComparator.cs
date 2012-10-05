using System;
using Duke.Utils;

namespace Duke.Comparators
{
    public class JaccardIndexComparator : IComparator
    {
        #region Private member variables

        private IComparator _subcomp;

        #endregion

        #region Constructors

        public JaccardIndexComparator()
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

            //FIXME: we assume t1 and t2 do not have internal duplicates

            // ensure that t1 is shorter than or same length as t2
            if (t1.Length > t2.Length)
            {
                String[] tmp = t2;
                t2 = t1;
                t1 = tmp;
            }

            // find best matches for each token in t1
            double intersection = 0;
            double union = t1.Length + t2.Length;
            for (int ix1 = 0; ix1 < t1.Length; ix1++)
            {
                double highest = 0;
                for (int ix2 = 0; ix2 < t2.Length; ix2++)
                {
                    highest = Math.Max(highest, _subcomp.Compare(t1[ix1], t2[ix2]));
                }

                // INV: the best match for t1[ix1] in t2 is has similarity highest
                intersection += highest;
                union -= highest; // we reduce the union by this similarity
            }

            return intersection/union;
        }

        public void SetComparator(IComparator comp)
        {
            _subcomp = comp;
        }

        #endregion
    }
}