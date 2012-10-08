using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Used to signal that something has gone wrong during Duke processing
    /// </summary>
    public class DukeException : Exception
    {

        public DukeException() {}

        public DukeException(string message) : base(message) {}
    }
}
