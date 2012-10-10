using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Duke.Utils
{
    public class NTriplesParser
    {
        private StreamReader _src;
        private IStatementHandler _handler;
        private int _lineno;
        private int _pos;
        private string line;

        public static void Parse(StreamReader src, IStatementHandler handler)
        {
            
        }
    }
}
