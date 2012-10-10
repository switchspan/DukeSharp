using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Duke.Datasources
{
    public class RecordHandler : IStatementHandler
    {
        #region Private member variables
        private Dictionary<string, RecordImpl> _records;
        private List<string> _types;
        private ColumnarDataSource _columnarDataSource;

        private const string RDF_TYPE = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";

        #endregion

        #region Constructors
        public RecordHandler(List<string> types, ColumnarDataSource columnarDataSource)
        {
            _records = new Dictionary<string, RecordImpl>();
            _types = types;
            _columnarDataSource = columnarDataSource;
        }
        #endregion

        #region Member methods
        public void FilterByTypes()
        {
            // this is fairly ugly. if types has values we add an extra property
            // RDF_TYPE to records during build, then filter out records of the
            // wrong types here. finally, we strip away the RDF_TYPE property here.

            if (_types.Count == 0)
            {
                return;
            }

            var recordKey = new List<string>(_records.Keys);
            foreach (var uri in recordKey)
            {
                RecordImpl r = _records[uri];
                if (!FilterByType(r))
                {
                    _records.Remove(uri);
                }
                else
                {
                    r.Remove(RDF_TYPE);
                }
            }
        }

        private bool FilterByType(IRecord record)
        {
            if (_types.Count == 0) // there is no filtering
                return true;

            bool found = false;
            return record.GetValues(RDF_TYPE).Any(value => _types.Contains(value));
        }

        public Dictionary<string, RecordImpl> GetRecords()
        {
            return _records;
        }
 
        //TODO: refactor this so that we share code with addStatement()
        public void Statement(string subject, string property, string obj, bool literal)
        {
            var record = (_records.ContainsKey(subject)) ? _records[subject] : null;
            if (record == null)
            {
                record = new RecordImpl();
                _records.Add(subject, record);
            }
            //_columnarDataSource
        }
        #endregion

        
    }
}
