using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    /// <summary>
    /// Convenience implementation with dummy methods, since most
    /// implementations will only implement matches().
    /// </summary>
    public abstract class AbstractMatchListener : IMatchListener
    {
        #region Member methods
        public void StartRecord(IRecord r)
        {
        }

        public void BatchReady(int size)
        {
        }

        public void BatchDone()
        { 
        }

        public void Matches(IRecord r1, IRecord r2, double confidence)
        {
        }

        public void MatchesPerhaps(IRecord r1, IRecord r2, double confidence)
        {
        }

        public void NoMatchFor(IRecord record)
        {
        }

        public void EndRecord()
        {
        }

        public void EndProcessing()
        {
        }
        #endregion

        
    }
}
