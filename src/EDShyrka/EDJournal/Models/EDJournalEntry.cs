using System;

namespace EDShyrka.EDJournal.Models
{
    /// <summary>
    /// Base class for all entries in journal
    /// </summary>
    public class EDJournalEntry
    {
		/// <summary>
		/// The event's raw JSON data.
		/// </summary>
		public string RawData { get; set; }

        /// <summary>
        /// The entry timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The event type.
        /// </summary>
        public string Event { get; set; }
    }
}
