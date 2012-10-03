using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Comparators
{
    /// <summary>
    /// An implementation of the Jaro-Winkler string similarity measure.
    /// The implementation follows the description in the paper "Evaluating
    /// String Comparator Performance for Record Linkage", by William 
    /// E. Yancy, RESEARCH REPORT SERIES (Statistics #2005-05), US Bureau
    /// of the Census. http://www.census.gov/srd/papers/pdf/rrs2005-05.pdf
    /// </summary>
    public class JaroWinkler : IComparator
    {
        #region Member methods
        public bool IsTokenized()
        {
            return true; // I guess?
        }

        public double Compare(string v1, string v2)
        {
            return Similarity(v1, v2);
        }

        public static double Similarity(String s1, String s2)
        {
            if (s1.Equals(s2))
                return 1.0;

            // ensure that s1 is shorter than or same length as s2
            if (s1.Length > s2.Length)
            {
                String tmp = s2;
                s2 = s1;
                s1 = tmp;
            }

            // (1) find the number of characters the two strings have in common.
            // note that matching characters can only be half the length of the
            // longer string apart.
            int maxdist = s2.Length/2;

            int c = 0; // count of common characters
            int t = 0; // count of transpositions
            int prevpos = -1;
            for (int ix = 0; ix < s1.Length; ix++)
            {
                var ch = s1[ix];

                // now try to find it in s2
                for (int ix2 = Math.Max(0, ix - maxdist); ix2 < Math.Min(s2.Length, ix + maxdist); ix2++)
                {
                    if (ch == s2[ix2])
                    {
                        c++; // we found a common character
                        if (prevpos != -1 && ix2 < prevpos)
                            t++; // moved back before earlier
                        prevpos = ix2;
                        break;
                    }
                }
            }

            // we don't dived t by 2 because as far as we can tell, the above
            // code counts transpositions directory.

            //Console.WriteLine(String.Format("c: {0}",c));
            //Console.WriteLine(String.Format("t: {0}", t));
            //Console.WriteLine(String.Format("c/m: {0}",(c / (double) s1.Length)));
            //Console.WriteLine(String.Format("c/n: {0}", (c / (double) s2.Length)));
            //Console.WriteLine(String.Format("(c-t)/c: {0}", ((c-t)/(double)c)));

            // we might have to give up right here
            if (c == 0)
                return 0.0;

            // first compute the score
            double score = ((c/(double) s1.Length) +
                            (c/(double) s2.Length) +
                            ((c - t)/(double) c))/3.0;

            // (2) common prefix modification
            int p; // length of prefix
            int last = Math.Min(4, s1.Length);
            for (p = 0; p < last && s1[p] == s2[p]; p++)
            {
                // hmmm...
            }

            score = score + ((p*(1 - score))/10);

            // (3) longer string adjustment
            // I'm confused about this part. Winkler's original source code includes
            // it, and Yancey's 2005 paper describes it. However, Winkler's list of
            // test cases in his 2006 paper does not include this modification. So
            // is this part of Jaro-Winkler, or is it not? Hard to say.
            //
            //   if (s1.length() >= 5 && // both strings at least 5 characters long
            //       c - p >= 2 && // at least two common characters besides prefix
            //       c - p >= ((s1.length() - p) / 2)) // fairly rich in common chars
            //     {
            //     System.out.println("ADJUSTED!");
            //     score = score + ((1 - score) * ((c - (p + 1)) /
            //                                     ((double) ((s1.length() + s2.length())
            //                                                - (2 * (p - 1))))));
            // }

            // (4) similar characters adjustment
            // the same holds for this as for (3) above.

            return score;
        }
        #endregion
        
    }
}
