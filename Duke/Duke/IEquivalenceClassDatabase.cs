using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    public interface IEquivalenceClassDatabase
    {
        // Returns the number of equivalence classes in the database.
        int GetClassCount();

        // Returns an iteractor over all classes in the database.
        List<List<string>> GetClasses();

        // Get all records linked to the given record (that is, all records
        // in the same equivalence class as the given record).
        List<String> GetClass(string id);

        //  Add a new link between two records.
        void AddLink(string id1, string id2);

        // Commit changes made to persistent store.
        void Commit();
    }
}
