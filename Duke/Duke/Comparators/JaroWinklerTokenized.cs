using System;
using System.Collections.Generic;
using Duke.Utils;

namespace Duke.Comparators
{
    /// <summary>
    /// A tokenized approach to string similarity, based on Jaccard
    /// equivalence and the Jaro-Winkler metric.
    /// 
    /// FIXME: Do we actually need this, or is DiceCoefficientComparator
    /// better? I guess Dice is probably better. However, the code for not
    /// allowing same token to be matched twice is unique to this comparator.
    /// Should we reuse in Dice, or just support more methods than just Dice?
    /// </summary>
    public class JaroWinklerTokenized : IComparator
    {
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

            // compute all comparisons
            var matches = new List<Match>(t1.Length*t2.Length);
            for (int ix1 = 0; ix1 < t1.Length; ix1++)
            {
                for (int ix2 = 0; ix2 < t2.Length; ix2++)
                {
                    matches.Add(new Match(JaroWinkler.Similarity(t1[ix1], t2[ix2]), ix1, ix2));
                }
            }

            // sort
            matches.Sort();

            // now pick the best matches, never allowing the same token to be
            // included twice. we mark a token as used by nulling it in t1|t2.
            double sum = 0.0;
            foreach (Match match in matches)
            {
                if (t1[match.Ix1] != null && t2[match.Ix2] != null)
                {
                    sum += match.Score;
                    t1[match.Ix1] = null;
                    t2[match.Ix2] = null;
                }
            }

            return sum/t1.Length;
        }

        #endregion
    }
}