using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    public class PassThroughFilter : AbstractMatchListener
    {
        #region Private member variables

        private bool _match_found;
        private IRecord _current;

        #endregion

        #region Constructors

        #endregion

        #region Member methods
        public new void StartRecord(IRecord r)
        {
            _match_found = false;
            _current = r;
            //RegisterStartRecord(r);
        }

        public new void Matches(IRecord r1, IRecord r2, double confidence)
        {
            _match_found = true;
            //RegisterMatch(r1, r2, confidence);
        }

        public new void MatchesPerhaps(IRecord r1, IRecord r2, double confidence)
        {
            _match_found = true;
            //RegisterMatchPerhaps(r1, r2, confidence);
        }

        public void EndRecord()
        {
            //if (!_match_found)
                //RegisterNoMatchFor(_current);
            //RegisterEndRecord();
        }
        #endregion
    }
}
