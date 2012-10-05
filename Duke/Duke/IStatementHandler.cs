using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Event-handler which receives parsed statements
    /// </summary>
    public interface IStatementHandler
    {
        void Statement(string subject, string property, string obj, bool literal);
    }
}
