using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    public class Link
    {
     
        #region Member properties

        public String Id1 { get; set; } // this is always lexicographically lower than _id2
        public String Id2 { get; set; }
        public bool Correct { get; set; } // is this link correct?
        public bool Asserted { get; set; } // did Duke assert this link?
        #endregion

        #region Constructors

        public Link(String id1, String id2, bool correct)
        {
            Id1 = id1;
            Id2 = id2;
            Correct = correct;
        }
        #endregion

        #region Member methods
        
        #endregion
    }
}
