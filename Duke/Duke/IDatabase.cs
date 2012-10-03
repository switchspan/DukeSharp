using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Used to store and index records for later matching.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Returns true if the database is held entirely in memory, and
        /// thus is not persistent.
        /// </summary>
        /// <returns></returns>
        bool IsInMemory();

        /// <summary>
        /// Add the record to the index.
        /// </summary>
        /// <param name="record"></param>
        void Index(IRecord record);

        /// <summary>
        /// Flushes all changes to disk. For in-memory databases this is a
        /// no-op.
        /// </summary>
        void Commit();

        /// <summary>
        /// Look up record by identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IRecord FindRecordById(String id);

        /// <summary>
        /// Look up potentially matching records.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        List<IRecord> FindCandidateMatches(IRecord record);

        /// <summary>
        /// Stores state to disk and closes all open resources.
        /// </summary>
        void Close();
    }
}
