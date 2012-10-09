using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    public class ChooseBestFilter : AbstractMatchListener
    {
        private IRecord _current;
        private IRecord _best;
        private double _max;

        public new void StartRecord(IRecord r)
        {
            //RegisterStartRecord(r);
            _max = 0;
            _best = null;
            _current = r;
        }

        public new void Matches(IRecord r1, IRecord r2, double confidence)
        {
            if (confidence > _max)
            {
                _max = confidence;
                _best = r2;
            }
        }

        public new void MatchesPerhaps(IRecord r1, IRecord r2, double confidence)
        {
            
        }
    }
}
