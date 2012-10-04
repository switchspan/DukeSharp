using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Duke
{
    /// <summary>
    /// An equivalence class database which maintains the entire structure
    /// in memory.
    /// </summary>
    public class InMemoryClassDatabase : IEquivalenceClassDatabase
    {
        #region Private member variables

        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        // Index from record ID to class ID.
        private Dictionary<string, int> _recordix;
        // the actual collection of classes.
        private Dictionary<int, List<String>> _classix;
        private int _nextid;

        #endregion

        #region Constructors
        public InMemoryClassDatabase()
        {
            _recordix = new Dictionary<string, int>();
            _classix = new Dictionary<int, List<string>>();
        }
        #endregion

        #region Member methods
        public int GetClassCount()
        {
            return _classix.Count;
        }

        public List<List<string>> GetClasses()
        {
            //var klasses = new List<string>();
            //foreach (var klass in _classix.Values)
            //{
            //    klasses.Add(klass.ToString());
            //}

            //return klasses;
            return new List<List<string>>();
        }

        public List<string> GetClass(string id)
        {
            int cid = 0;
            if (_recordix.TryGetValue(id,out cid))
            {
                return _classix[cid];
            } else
            {
                return null;
            }
        }

        public void AddLink(string id1, string id2)
        {
            int cid1 = 0;
            int cid2 = 0;
            bool hasId1 = _recordix.TryGetValue(id1, out cid1);
            bool hasId2 = _recordix.TryGetValue(id2, out cid2);

            if (!hasId1 && !hasId2)
            {
                // need to make a new class
                Int32 cid = _nextid;
                List<String> klass = new List<string>();
                klass.Add(id1);
                klass.Add(id2);
                _classix.Add(cid, klass);
                _recordix.Add(id1, cid);
                _recordix.Add(id2, cid);
            }
            else if (!hasId1 || !hasId2)
            {
                // only one has a class, so add the other to the same class, and we're done.
                int cid = hasId1? cid1 : cid2; 
                string id = hasId1? id1 : id2;

                List<String> klass = _classix[cid];
                klass.Add(id);
                _recordix.Add(id, cid);
            }
            else
            {
                // both records already have a class
                if (cid1.Equals(cid2))
                    return; // it's the same class, so nothing new learned
                // okay, we need to merge the classes
                //Merge(cid1, cid2)
            }
        }
        
        public void Commit()
        {
            // nothing to commit
        }

        public void Merge(int cid1, int cid2)
        {
            var klass1 = _classix[cid1];
            var klass2 = _classix[cid2];

            // if klass1 is the smaller, swap the two
            if (klass1.Count < klass2.Count)
            {
                var tmp = klass2;
                klass2 = klass1;
                klass1 = tmp;

                int itmp = cid2;
                cid2 = cid1;
                cid1 = itmp;
            }

            // now perform the actual merge
            foreach (var id in klass2)
            {
                klass1.Add(id);
                _recordix.Add(id, cid1);
            }

            // delete the smaller class, and we're done
            _classix.Remove(cid2);

        }
        #endregion

        
    }
}
