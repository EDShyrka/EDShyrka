namespace EDShyrka.EDJournal.Models
{
    /// <summary>
    /// FileHeadser event written when game starts.
    /// </summary>
    public class Fileheader : EDJournalEntry
    {
        /// <summary>
        /// Unknown information.
        /// </summary>
        public int Part { get; set; }

        /// <summary>
        /// The game's language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// True if game is Odyssey, false otherwise.
        /// </summary>
        public bool Odyssey { get; set; }

        /// <summary>
        /// The game's version.
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// The game's build information.
        /// </summary>
        public string Build { get; set; }

        #region samples
        //{ "timestamp":"2022-01-18T09:24:42Z", "event":"Fileheader", "part":1, "language":"English\\UK", "Odyssey":false, "gameversion":"3.8.0.403", "build":"r279702/r0 " }
        #endregion samples
    }
}
