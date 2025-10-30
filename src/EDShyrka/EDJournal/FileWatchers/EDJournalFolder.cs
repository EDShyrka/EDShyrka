using System;
using System.IO;

namespace EDShyrka.EDJournal.FileWatchers;

/// <summary>
/// Locates the Elite Dangerous journals folder and store the value.
/// </summary>
public class EDJournalFolder
{
	private static readonly string[] _subFolderElements = ["Saved Games", "Frontier Developments", "Elite Dangerous"];

	/// <summary>
	/// Initializes a new instance of the <see cref="EDJournalFolder"/> class.
	/// </summary>
	public EDJournalFolder()
	{
		Value = BuildSubFolderPath();
	}

	/// <summary>
	/// The Elite Dangerous journals folder.
	/// </summary>
	public string Value { get; init; }

	/// <summary>
	/// Builds the absolute path to the journals sub folder.
	/// </summary>
	/// <remarks>Locate journal folder using special folder enum.</remarks>
	/// <returns>The absolute path to the journals sub folder.</returns>
	private static string BuildSubFolderPath()
	{
		var userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		var subFolder = Path.Combine(_subFolderElements);
		return Path.Combine(userProfileFolder, subFolder);
	}
}
