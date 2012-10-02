using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    public class RecordImpl : IRecord
    {
        private Dictionary<String, List<String>> _data;

        public RecordImpl(Dictionary<String, List<String>> data)
        {
            _data = data; //HACK: should we copy?
        }

        public RecordImpl()
        {
            _data = new Dictionary<string, List<string>>();
        }

        public bool IsEmpty()
        {
            return (_data.Count == 0);
        }

        public List<string> GetProperties()
        {
            return _data.Keys.ToList();
        }

        public List<string> GetValues(string prop)
        {
            return _data[prop];
        }

        public string GetValue(string prop)
        {
            string valueToReturn = "";
            var values = GetValues(prop);

            foreach (String value in values)
            {
                if (value != null)
                {
                    valueToReturn = value;
                    break;
                }
            }

            return valueToReturn;
        }

        public void AddValue(String property, String value)
        {
            if (!_data.ContainsKey(property)) // We don't have the key or value, so add it
            {
                _data.Add(property, new List<string> {value});
            }
            else
            {
                _data[property].Add(value);
            }
        }

        public void Remove(String property)
        {
            _data.Remove(property);
        }

        public void Merge(IRecord other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("[RecordImpl {0}]", _data);
        }
    }
}
