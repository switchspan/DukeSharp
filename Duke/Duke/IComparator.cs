using System;

namespace Duke
{
    /// <summary>
    /// An operator which compares two values for similarity, and returns a number in the range 0.0 to 1.0 
    /// indicating the degree of similarity.
    /// </summary>
    public interface IComparator
    {
        /// <summary>
        /// Determines whether this instance is tokenized.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is tokenized (comparator breaks string values up into
        /// tokens when comparing. Necessary because this impacts indexing of values.; otherwise, <c>false</c>.
        /// </returns>
        Boolean IsTokenized();

        Double Compare(String v1, String v2);
    }
}