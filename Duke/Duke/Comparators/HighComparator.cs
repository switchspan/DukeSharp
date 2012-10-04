namespace Duke.Comparators
{
    /// <summary>
    /// This comparator is only currently used by the Configuration class to
    /// compare properties. It mimics the typical interface used by the Comparators...
    /// </summary>
    public static class HighComparator
    {
        #region Member methods

        /// <summary>
        /// Don't know if we need this...
        /// </summary>
        /// <returns></returns>
        public static bool IsTokenized()
        {
            return false;
        }

        public static int Compare(Property p1, Property p2)
        {
            if (p1.GetHighProbability() < p2.GetHighProbability())
            {
                return -1;
            }
            if (p1.GetHighProbability() == p2.GetHighProbability())
            {
                return 0;
            }
            return 1;
        }

        #endregion
    }
}