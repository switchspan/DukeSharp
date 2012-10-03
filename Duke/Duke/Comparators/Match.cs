using System;

namespace Duke.Comparators
{
    public class Match : IComparable
    {
        #region Member properties

        public double Score { get; set; }
        public int Ix1 { get; set; }
        public int Ix2 { get; set; }

        #endregion

        #region Constructors

        public Match(double score, int ix1, int ix2)
        {
            Score = score;
            Ix1 = ix1;
            Ix2 = ix2;
        }

        #endregion

        #region Member methods

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof (Match))
                return -1;

            double oscore = ((Match) obj).Score;

            if (Score < oscore)
            {
                return 1;
            }

            if (Score > oscore)
            {
                return -1;
            }

            return 0;
        }

        #endregion
    }
}