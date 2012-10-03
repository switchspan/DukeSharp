using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    public class TestFileListener : AbstractMatchListener
    {
        #region Private member variables

        private List<Property> _idprops;
        private Dictionary<String, Link> _links;
        private int _notintest;
        private int _missed; // RL mode only
        private bool _debug;
        private bool _quiet; // true means no output whatever (default: false)
        

        #endregion

        #region Constructors

        #endregion

        #region Member methods

        #endregion
    }
}
