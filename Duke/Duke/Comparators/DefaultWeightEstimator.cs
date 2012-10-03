using System;

namespace Duke.Comparators
{
    public class DefaultWeightEstimator : IWeightEstimator
    {
        #region Private member variables

        private readonly double _digits;
        private readonly double _letters;
        private readonly double _other;
        private readonly double _punctuation;

        #endregion

        #region Constructors

        public DefaultWeightEstimator()
        {
            _digits = 2.0;
            _letters = 1.0;
            _punctuation = 0.1;
            _other = 1.0;
        }

        #endregion

        #region Member methods

        public double Substitute(char ch1, char ch2)
        {
            return Math.Max(Insert(ch1), Insert(ch2));
        }

        public double Delete(char ch)
        {
            return Insert(ch);
        }

        public double Insert(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') ||
                (ch >= 'A' && ch <= 'Z'))
                return _letters;
            if (ch >= '0' && ch <= '9')
                return _digits;
            if (ch == ' ' || ch == '\'' || ch == ',' || ch == '-' || ch == '/' ||
                ch == '\\' || ch == '.')
                return _punctuation;
            return _other;
        }

        #endregion
    }
}