using System;

namespace Duke.Comparators
{
    public class WeightedLevenshtein : IComparator
    {
        #region Private member variables

        private IWeightEstimator _estimator;

        #endregion

        #region Constructors

        public WeightedLevenshtein()
        {
            _estimator = new DefaultWeightEstimator();
        }

        #endregion

        #region Member methods

        public bool IsTokenized()
        {
            return true;
        }

        public double Compare(string v1, string v2)
        {
            // if the strings are equal we can stop right here.
            if (v1.Equals(v2))
                return 1.0;

            // we couldn't shortcut, so now we go ahead and compute the full
            // matrix
            int len = Math.Min(v1.Length, v2.Length);
            double dist = Distance(v1, v2, _estimator);
            return 1.0 - (dist/(len));
        }

        public void SetEstimator(IWeightEstimator estimator)
        {
            _estimator = estimator;
        }

        public static double Distance(String s1, String s2, IWeightEstimator weight)
        {
            int s1Len = s1.Length;
            if (s1Len == 0)
                return EstimateCharacters(s2, weight);
            if (s2.Length == 0)
                return EstimateCharacters(s1, weight);

            // we use a flat array for better performance. we address it by
            // s1ix + s1len * s2ix. this modification improves performance
            // by about 30%, which is definitely worth the extra complexity.
            var matrix = new double[(s1Len + 1)*(s2.Length + 1)];
            for (int col = 0; col <= s2.Length; col++)
                matrix[col*s1Len] = col;
            for (int row = 0; row <= s1Len; row++)
                matrix[row] = row;

            for (int ix1 = 0; ix1 < s1Len; ix1++)
            {
                char ch1 = s1[ix1];
                for (int ix2 = 0; ix2 < s2.Length; ix2++)
                {
                    double cost;
                    char ch2 = s2[ix2];
                    cost = ch1 == ch2 ? 0 : weight.Substitute(ch1, s2[ix2]);

                    double left = matrix[ix1 + ((ix2 + 1)*s1Len)] +
                                  weight.Delete(ch1);
                    double above = matrix[ix1 + 1 + (ix2*s1Len)] +
                                   weight.Insert(ch2);
                    double aboveleft = matrix[ix1 + (ix2*s1Len)] + cost;
                    matrix[ix1 + 1 + ((ix2 + 1)*s1Len)] =
                        Math.Min(left, Math.Min(above, aboveleft));
                }
            }

            // for (int ix1 = 0; ix1 <= s1len; ix1++) {
            //   for (int ix2 = 0; ix2 <= s2.length(); ix2++) {
            //     System.out.print(matrix[ix1 + (ix2 * s1len)] + " ");
            //   }
            //   System.out.println();
            // }

            return matrix[s1Len + (s2.Length*s1Len)];
        }

        private static double EstimateCharacters(String s, IWeightEstimator e)
        {
            double sum = 0.0;
            for (int ix = 0; ix < s.Length; ix++)
                sum += Math.Min(e.Insert(s[ix]), e.Delete(s[ix]));
            return sum;
        }

        #endregion
    }
}