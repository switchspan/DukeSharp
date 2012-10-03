using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    /// <summary>
    /// Match listener which prints events to standard out. Used by the
    /// command-line client.
    /// </summary>
    public class PrintMatchListener : AbstractMatchListener
    {
        #region Private member variables

        private int _matches;
        private int _records;
        private int _nonmatches; // only counted in record linkage mode
        private bool _showmaybe;
        private bool _showmatches;
        private bool _progress;
        private bool _linkage; // means there's a separate indexing step

        #endregion

        #region Constructors
        public PrintMatchListener(bool showmatches, bool showmaybe, bool progress, bool linkage)
        {
            _matches = 0;
            _records = 0;
            _showmatches = showmatches;
            _showmatches = showmaybe;
            _progress = progress;
            _linkage = linkage;
        }
        #endregion

        #region Member methods
        public int GetMatchCount()
        {
            return _matches;
        }

        public new void BatchReady(int size)
        {
            if (_linkage)
                _records += size; // no EndRecord() call in linnkage mode
            if (_progress)
                Console.WriteLine(String.Format("Records: {0}", _records));
        }

        public new void Matches(IRecord r1, IRecord r2, double confidence)
        {
            _matches++;
            if (_showmatches)
            {
                Show(r1, r2, confidence, "\nMATCH");
            }
            if (_matches % 1000 == 0 && _progress)
            {
                Console.WriteLine(String.Format("{0} matches", _matches));
            }
        }

        public new void MatchesPerhaps(IRecord r1, IRecord r2, double confidence)
        {
            if (_showmaybe)
            {
                Show(r1, r2, confidence, "\nMAYBE MATCH");
            }
        }

        public new void EndRecord()
        {
            _records++;
        }

        public new void EndProcessing()
        {
            if (_progress)
            {
                Console.WriteLine(String.Format("{0}",Environment.NewLine));
                Console.WriteLine(String.Format("Total records: {0}", _records));
                Console.WriteLine(String.Format("Total matches: {0}", _matches));
                if (_nonmatches > 0) //FIXME: this ain't right. we should know the mode
                {
                    Console.WriteLine(String.Format(""));
                }
            }
        }

        public new void NoMatchFor(IRecord record)
        {
            _nonmatches++;
            if (_showmatches)
            {
                Console.WriteLine(String.Format("{0}NO MATCH FOR:{1}{2}",Environment.NewLine,Environment.NewLine,RecordToString(record)));
            }
        }

        public static void Show(IRecord r1, IRecord r2, double confidence, string heading)
        {
            Console.WriteLine(String.Format("{0} {1}", heading, confidence));
            Console.WriteLine(String.Format("{0}", RecordToString(r1)));
            Console.WriteLine(String.Format("{0}", RecordToString(r2)));
        }

        public static String RecordToString(IRecord r)
        {
            var sb = new StringBuilder();
            foreach (var p in r.GetProperties())
            {
                var vs = r.GetValues(p);
                if (vs == null || vs.Count == 0)
                    continue;

                sb.Append(p + ": ");
                foreach (var v in vs)
                {
                    sb.Append(String.Format("'{0}', ", v));
                }
            }

            return sb.ToString();
        }
        #endregion


    }
}
