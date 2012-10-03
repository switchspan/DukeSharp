using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Matchers
{
    /// <summary>
    /// Interface implemented by code which can receive notifications that
    /// two records are considered to match.
    /// </summary>
    public interface IMatchListener
    {
        /// <summary>
        ///  Notification that the processor starts to match this record.
        /// </summary>
        /// <param name="r"></param>
        void StartRecord(IRecord r);

        /// <summary>
        ///  Notification that Duke is about to process a new batch of records.
        /// </summary>
        /// <param name="size"></param>
        void BatchReady(int size);

        /// <summary>
        /// Notification that Duke has finished processing a batch of records.
        /// </summary>
        void BatchDone();

        /// <summary>
        /// Notification that the two records match. There will have
        /// been a previous startRecord(r1) notification.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="confidence"></param>
        void Matches(IRecord r1, IRecord r2, double confidence);

        /// <summary>
        /// Notification that the two records might match. There will have
        /// been a previous startRecord(r1) notification.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="confidence"></param>
        void MatchesPerhaps(IRecord r1, IRecord r2, double confidence);

        /// <summary>
        /// Called in record linkage mode if no link is found for the record.
        /// </summary>
        /// <param name="record"></param>
        void NoMatchFor(IRecord record);

        /// <summary>
        /// Notification that processing of the current record (the one in
        /// the last startRecord(r) call) has ended.
        /// </summary>
        void EndRecord();

        /// <summary>
        /// Notification that this processing run is over.
        /// </summary>
        void EndProcessing();

    }
}
