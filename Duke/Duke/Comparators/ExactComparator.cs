namespace Duke.Comparators
{
    /// <summary>
    /// Comparator which compares two values exactly. It returns 1.0 if
    /// they are equal, and 0.0 if they are different.
    /// </summary>
    public class ExactComparator : IComparator
    {
        #region Member methods

        public bool IsTokenized()
        {
            return false;
        }

        public double Compare(string v1, string v2)
        {
            return v1.Equals(v2) ? 1.0 : 0.0;
        }

        #endregion
    }
}