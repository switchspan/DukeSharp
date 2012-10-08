using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Thrown when there is an error in the configuration of Duke.
    /// </summary>
    public class DukeConfigException : Exception
    {
        public DukeConfigException()
        {}

        public DukeConfigException(string message) : base(message) {}

    }
}
