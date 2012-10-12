using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Any class which implements this interface can be used as a data
    /// source, so you can plug in your own data sources. Configuration
    /// properties are received as bean setter calls via reflection.
    /// </summary>
    public interface IDataSource
    {
        Records GetRecords();

        void SetLogger(); //TODO: Change this over to NLog for logging...

    }
}
