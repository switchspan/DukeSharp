using System;

namespace Duke.Comparators
{
    public class Levenshtein : IComparator
    {
        #region Private member variables

        #endregion

        #region Constructors

        #endregion

        #region Member methods

        public bool IsTokenized()
        {
            return true;
        }

        public double Compare(string v1, string v2)
        {
            int len = Math.Min(v1.Length, v2.Length);

            // we know that if the outcome here is 0.5 or lower, then the
            // property will return the lower probability. so the moment we
            // learn that probability is 0.5 or lower we can return 0.0 and
            // stop. this optimization makes a perceptible improvement in
            // overall performance.
            int maxlen = Math.Max(v1.Length, v2.Length);
            if (len/(double) maxlen <= 0.5)
                return 0.0;

            // if the strings are equal we can stop right here.
            if (len == maxlen && v1.Equals(v2))
                return 1.0;

            // we couldn't shortcut, so now we go ahead and compute the full
            // matrix
            int dist = Math.Min(CutoffDistance(v1, v2, maxlen), len);

            return 1.0 - ((dist)/((double) len));
        }

        /// <summary>
        /// Garshol's orgininal Levenshtein distance function - this is used in 
        /// the PersonNameComparator class...
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int Distance(String s1, String s2)
        {
            if (s1.Length == 0)
                return s2.Length;
            if (s2.Length == 0)
                return s1.Length;

            int s1Len = s1.Length;
            // we use a flat array for better performance. we address it by
            // s1ix + s1len * s2ix. this modification improves performance
            // by about 30%, which is definitely worth the extra complexity.
            int[] matrix = new int[(s1Len + 1) * (s2.Length + 1)];
            for (int col = 0; col <= s2.Length; col++)
                matrix[col * s1Len] = col;
            for (int row = 0; row <= s1Len; row++)
                matrix[row] = row;

            for (int ix1 = 0; ix1 < s1Len; ix1++)
            {
                char ch1 = s1[ix1];
                for (int ix2 = 0; ix2 < s2.Length; ix2++)
                {
                    int cost;
                    if (ch1 == s2[ix2])
                        cost = 0;
                    else
                        cost = 1;

                    int left = matrix[ix1 + ((ix2 + 1) * s1Len)] + 1;
                    int above = matrix[ix1 + 1 + (ix2 * s1Len)] + 1;
                    int aboveleft = matrix[ix1 + (ix2 * s1Len)] + cost;
                    matrix[ix1 + 1 + ((ix2 + 1) * s1Len)] =
                      Math.Min(left, Math.Min(above, aboveleft));
                }
            }

            // for (int ix1 = 0; ix1 <= s1len; ix1++) {
            //   for (int ix2 = 0; ix2 <= s2.length(); ix2++) {
            //     System.out.print(matrix[ix1 + (ix2 * s1len)] + " ");
            //   }
            //   System.out.println();
            // }

            return matrix[s1Len + (s2.Length * s1Len)];

        }

        /// <summary>
        /// optimizes by returning 0.0 as soon as we know total difference is
        /// larger than 0.5, which happens when the distance is greater than
        /// maxlen.
        /// on at least one use case, this optimization shaves 15% off the
        /// total execution time.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="maxlen"></param>
        /// <returns></returns>
        public static int CutoffDistance(String s1, String s2, int maxlen)
        {
            if (s1.Length == 0)
                return s2.Length;
            if (s2.Length == 0)
                return s1.Length;

            int maxdist = Math.Min(s1.Length, s2.Length)/2;

            int s1Len = s1.Length;
            // we use a flat array for better performance. we address it by
            // s1ix + s1len * s2ix. this modification improves performance
            // by about 30%, which is definitely worth the extra complexity.
            var matrix = new int[(s1Len + 1)*(s2.Length + 1)];
            for (int col = 0; col <= s2.Length; col++)
            {
                matrix[col*s1Len] = col;
            }
            for (int row = 0; row <= s1Len; row++)
            {
                matrix[row] = row;
            }

            for (int ix1 = 0; ix1 < s1Len; ix1++)
            {
                char ch1 = s1[ix1];
                for (int ix2 = 0; ix2 < s2.Length; ix2++)
                {
                    int cost = (ch1 == s2[ix2]) ? 0 : 1;

                    int left = matrix[ix1 + ((ix2 + 1)*s1Len)] + 1;
                    int above = matrix[ix1 + 1 + (ix2*s1Len)] + 1;
                    int aboveleft = matrix[ix1 + (ix2*s1Len)] + cost;
                    int distance = Math.Min(left, Math.Min(above, aboveleft));
                    if (ix1 == ix2 && distance > maxdist)
                        return distance;

                    matrix[ix1 + 1 + ((ix2 + 1)*s1Len)] = distance;
                }
            }

            return matrix[s1Len + (s2.Length*s1Len)];
        }

        #endregion
    }
}