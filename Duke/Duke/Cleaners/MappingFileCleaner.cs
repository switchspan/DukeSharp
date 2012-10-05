using System;
using System.Collections.Generic;
using System.IO;
using Duke.Utils;

namespace Duke.Cleaners
{
    public class MappingFileCleaner : ICleaner
    {
        private Dictionary<string, string> _mapping; 

        public string Clean(string value)
        {
            if (_mapping.ContainsKey(value))
            {
                return _mapping[value];
            }
            else
            {
                return value;
            }
        }

        public void SetMappingFile(string filename)
        {
            _mapping = new Dictionary<string, string>();

            //TODO: Fix character encoding?
            try
            {
                var csv = new CsvReader(new StreamReader(filename));

                String[] row = csv.Next();
                while (row != null)
                {
                    _mapping.Add(row[0], row[1]);
                    row = csv.Next();
                }

                csv.Close();
            }
            catch (System.Exception ex)
            {
                //TODO: Add better exception handling...
                throw;
            }
        }
    }
}
