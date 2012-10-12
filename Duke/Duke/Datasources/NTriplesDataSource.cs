using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Duke.Datasources
{
    /// <summary>
    /// A data source which can read RDF data from NTriples files. By 
    /// default it loads the entire data set into memory, in order to build
    /// complete records. However, if the file is sorted you can call
    /// setIncrementalMode(true) to avoid this.
    /// </summary>
    public class NTriplesDataSource : ColumnarDataSource
    {
        #region Private member variables

        private readonly List<string> _types;

        #endregion

        #region Member properties

        public string InputFile { get; set; }
        public StreamReader DirectReader { get; set; }
        public bool IsIncremental { get; set; }

        #endregion

        #region Constructors

        public NTriplesDataSource()
        {
            _types = new List<string>();
        }

        #endregion

        #region Member methods

        public void SetAcceptTypes(String types)
        {
            _types.Add(types);
        }

        public override Records GetRecords()
        {
            if (DirectReader == null)
            {
                VerifyProperty(InputFile, "input-file");
            }

            try
            {
                StreamReader reader = DirectReader;
                if (reader == null)
                {
                    reader = new StreamReader(InputFile, Encoding.UTF8);
                }

                if (!IsIncremental)
                {
                    // non-incremental mode: everything gets build in memory
                    var handler = new RecordHandler(_types, this);
                    
                }
            }
            catch (Exception e)
            {
                throw new DukeException(e.Message);
            }
            return null;
        }

        public override void SetLogger()
        {
            throw new NotImplementedException();
        }


        protected override string GetSourceName()
        {
            return "NTriples";
        }

        public void AddStatement(RecordImpl record, string subject, string property, string obj)
        {
            //List<Column> cols = 
        }

        #endregion
    }
}