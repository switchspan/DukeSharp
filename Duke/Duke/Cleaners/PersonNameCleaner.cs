using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Duke.Utils;
using NLog;

namespace Duke.Cleaners
{
    public class PersonNameCleaner : ICleaner
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private LowerCaseNormalizeCleaner _sub;
        private Dictionary<String, String> _mapping; 

        public PersonNameCleaner()
        {
            _sub = new LowerCaseNormalizeCleaner();

            // load token translation _mapping (FIXME: move to static init?)
            try
            {
                _mapping = LoadMapping();
            }
            catch (System.Exception ex)
            {
               logger.Error("Error initializing object: {0}", ex.Message);
            }
        }


        public string Clean(string value)
        {
            // do basic cleaning 
            value = _sub.Clean(value);
            if (String.IsNullOrEmpty(value))
                return value;

            // tokenize, then map tokens, then rejoin
            String[] tokens = StringUtils.Split(value);
            for (int ix = 0; ix < tokens.Length; ix++)
            {
                if (_mapping.ContainsKey(tokens[ix]))
                    tokens[ix] = _mapping[tokens[ix]];
            }

            return StringUtils.Join(tokens);
        }

        private Dictionary<String, String> LoadMapping()
        {
            const string mapfile = "name-mappings.txt";

            var mapping = new Dictionary<string, string>();
            //ClassLoader cloader = Thread.currentThread().getContextClassLoader();

            using (var reader = new StreamReader(mapfile, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int pos = line.IndexOf(',');
                    mapping.Add(line.Substring(0, pos), line.Substring(pos + 1));
                }
            }

            return mapping;
        } 
    }
}
