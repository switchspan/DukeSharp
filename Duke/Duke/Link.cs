using System;

namespace Duke
{
    /// <summary>
    /// Immutable representation o fa link between two identities.
    /// </summary>
    public class Link
    {
        #region Private member variables

        private static readonly DateTime Jan1St1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region Member properties

        public string Id1 { get; private set; } // ID of record, lexiographically lowest
        public string Id2 { get; private set; } // ID of record
        public LinkKind Kind { get; private set; }
        public LinkStatus Status { get; private set; }
        public long Timestamp { get; private set; }

        #endregion

        #region Public Enums

        #region LinkKind enum

        public enum LinkKind
        {
            Same = 1,
            MaybeSame = 2,
            Different = 3
        }

        #endregion

        #region LinkStatus enum

        public enum LinkStatus
        {
            Retracted = 0,
            Inferred = 1,
            Asserted = 2
        }

        #endregion

        #endregion

        #region Constructors

        public Link(string id1, string id2, LinkStatus status, LinkKind kind)
            : this(id1, id2, status, kind, CurrentTimeMillis())
        {
        }

        public Link(string id1, string id2, LinkStatus status, LinkKind kind, long timestamp)
        {
            if (String.CompareOrdinal(id1, id2) < 0)
            {
                Id1 = id1;
                Id2 = id2;
            }
            else
            {
                Id1 = id2;
                Id2 = id1;
            }
            Status = status;
            Kind = kind;
            Timestamp = timestamp;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Changes the link status to retracted, and updates the timestamp.
        /// Does <em>not</em> write to the database.
        /// </summary>
        public void Retract()
        {
            Status = LinkStatus.Retracted;
            Timestamp = CurrentTimeMillis();
        }

        public bool Overrides(Link other)
        {
            if (other.Status == LinkStatus.Asserted && Status != LinkStatus.Asserted)
                return false;
            if (Status == LinkStatus.Asserted && other.Status != LinkStatus.Asserted)
                return true;

            // the two links are from equivalent sources of information, so we
            // believe the most recent

            return (Timestamp > other.Timestamp);
        }

        public new bool Equals(Object other)
        {
            if (!(other.GetType() == typeof (Link)))
                return false;

            var olink = (Link) other;
            return (olink.Id1.Equals(Id1) && olink.Id2.Equals(Id2));
        }

        private static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
        }

        #endregion
    }
}