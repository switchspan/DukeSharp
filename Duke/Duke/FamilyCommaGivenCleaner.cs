using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// *Experimental* cleaner for person names of the form "Smith,
    /// John". Based on PersonNameCleaner. It also normalizes periods 
    /// in initials, so that "J.R. Ackerley" becomes "J. R. Ackerley".
    /// </summary>
    public class FamilyCommaGivenCleaner : ICleaner
    {
        public string Clean(string value)
        {
            throw new NotImplementedException();
        }
    }
}
