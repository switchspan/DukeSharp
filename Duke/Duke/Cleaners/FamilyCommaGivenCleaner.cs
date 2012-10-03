namespace Duke.Cleaners
{
    /// <summary>
    /// *Experimental* cleaner for person names of the form "Smith,
    /// John". Based on PersonNameCleaner. It also normalizes periods 
    /// in initials, so that "J.R. Ackerley" becomes "J. R. Ackerley".
    /// </summary>
    public class FamilyCommaGivenCleaner : ICleaner
    {
        #region Private member variables
        private readonly PersonNameCleaner _sub;
        #endregion

        #region Constructors
        public FamilyCommaGivenCleaner()
        {
            _sub = new PersonNameCleaner();
        }
        #endregion

        #region Member methods
        public string Clean(string value)
        {
            int i = value.IndexOf(',');
            if (i != -1)
                value = value.Substring(i + 1) + " " + value.Substring(0, i);

            var tmp = new char[value.Length * 2];
            int pos = 0;
            for (int ix = 0; ix < value.Length; ix++)
            {
                tmp[pos++] = value[ix];
                if (value[ix] == '.' &&
                    ix + 1 < value.Length &&
                    value[ix + 1] != ' ')
                    tmp[pos++] = ' ';
            }

            return _sub.Clean(new string(tmp, 0, pos));
        }
        #endregion

        
    }
}
