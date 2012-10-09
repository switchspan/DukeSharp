using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Duke
{
    /// <summary>
    /// Can read XML configuration files and return a fully set up configuration.
    /// </summary>
    public class ConfigLoader
    {
        //Note that if file starts with 'classpath:' the resource is looked
        // up on the classpath instead.

        public static Configuration Load(string file)
        {
            var cfg = new Configuration();

            using (var sr = new StreamReader(file))
            {
                var reader = new XmlTextReader(sr);


            }

            

        }
    }
}
