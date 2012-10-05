using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    public interface ILinkDatabase
    {
        /// <summary>
        /// Returns all links modifed since the given time.
        /// </summary>
        /// <param name="since"></param>
        /// <returns></returns>
        List<Link> GetChangesSince(long since);

        /// <summary>
        /// Get all links
        /// </summary>
        /// <returns></returns>
        List<Link> GetAllLinks();

        /// <summary>
        /// Get all links for this identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Link> GetAllLinksFor(string id);

        /// <summary>
        /// Assert a link.
        /// </summary>
        /// <param name="link"></param>
        void AssertLink(Link link);

        /// <summary>
        /// Can we work out, based on what we know, the relationship between
        /// these two? Returns null if we don't know the relationship.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        Link InferLink(string id1, string id2);

        /// <summary>
        /// Verifies that we still have a connection to the database, and reestablishes it, if not. 
        /// Useful when connections live a long time and are rarely used.
        /// </summary>
        void ValidateConnection();

        /// <summary>
        /// Commit asserted links to persistent store.
        /// </summary>
        void Commit();

        /// <summary>
        /// Shuts down the database, releasing resources.
        /// </summary>
        void Close();
    }
}
