using System;
using System.IO;

namespace EDShyrka.EDJournal
{
	/// <summary>
	/// Locates the Elite Dangerous journals folder and store the value.
	/// </summary>
	public class EDJournalFolder
	{
		private const string _subFolder = @"Saved Games\Frontier Developments\Elite Dangerous\";

		/// <summary>
		/// Initializes a new instance of the <see cref="EDJournalFolder"/> class.
		/// </summary>
		public EDJournalFolder()
		{
			// locate journal folder using special folder enum
			var userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			Value = Path.Combine(userProfileFolder, _subFolder);
		}

		/// <summary>
		/// The Elite Dangerous journals folder.
		/// </summary>
		public string Value { get; init; }
	}
}
