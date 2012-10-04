using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;

namespace Duke
{
    /// <summary>
    /// Wraps a Lucene Document to provide a representation of it as a Record.
    /// </summary>
    public class DocumentRecord : IRecord
    {
       // * Beware: this document number will change when changes are made to
       // * Lucene index. So while it's safe to use right now, it is not safe
       // * if record objects persist across batch process calls. It might
       // * also not be safe in a multi-threaded setting. So longer-term we
       // * may need a better solution for removing duplicate candidates.
        #region Private member variables

        private readonly int _docno;
        private readonly Document _doc;
        #endregion

        #region Constructors
        public DocumentRecord(int docno, Document doc)
        {
            _doc = doc;
            _docno = docno;
        }
        #endregion

        #region Member methods
        public List<string> GetProperties()
        {
            return _doc.GetFields().Select(f => f.Name()).ToList();
        }

        public List<string> GetValues(string prop)
        {
            var fields = _doc.GetFields(prop);
            if (fields.Length == 1)
                return new List<string> { fields[0].StringValue() };

            var values = new List<String>(fields.Length);
            values.AddRange(fields.Select(t => t.StringValue()));

            return values;
        }

        public string GetValue(string prop)
        {
            return _doc.Get(prop);
        }

        public void Merge(IRecord other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("[DocumentRecord {0} {1}]", _docno, _doc);
        }

        public int HashCode()
        {
            return _docno;
        }

        public new bool Equals(Object other)
        {
            if (!(other.GetType() == typeof(DocumentRecord)))
            {
                return false;
            }

            var theDoc = (DocumentRecord) other;

            return theDoc._docno == _docno;
        }
        #endregion

        
    }
}
