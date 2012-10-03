namespace Duke.Comparators
{
    /// <summary>
    /// A comparator which returns 0.0 if two values are exactly equal, and
    /// 1.0 if they are different. The inverse of ExactComparator.
    /// </summary>
    public class DifferentComparator : IComparator
    {
        #region Member methods

        public bool IsTokenized()
        {
            return false;
        }

        public double Compare(string v1, string v2)
        {
            return v1.Equals(v2) ? 0.0 : 1.0;
        }

        #endregion
    }
}