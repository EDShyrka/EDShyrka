using System;
using System.IO;
using System.Runtime.InteropServices;

namespace EDShyrka.EDJournal.FileWatchers;

/// <summary>
/// Locates the Elite Dangerous journals folder and store the value.
/// </summary>
public class EDJournalFolder
{
	/// <summary>
	/// subFolderElements = ["Saved Games", "Frontier Developments", "Elite Dangerous"];
	/// </summary>
	private static readonly string[] _subFolderElements = ["Frontier Developments", "Elite Dangerous"];

	/// <summary>
	/// GUID for the Saved Games folder (FOLDERID_SavedGames)
	/// </summary>
	private static readonly Guid KnownFolder_SavedGames = new("{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}");

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
	/// P/Invoke declaration for SHGetKnownFolderPath
	/// </summary>
	/// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
	/// <param name="dwFlags">Flags that specify special retrieval options.</param>
	/// <param name="hToken">An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder for the current user.</param>
	/// <returns></returns>
	[DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken);

	/// <summary>
	/// Builds the absolute path to the journals sub folder.
	/// </summary>
	/// <remarks>Locate journal folder using special folder enum.</remarks>
	/// <returns>The absolute path to the journals sub folder.</returns>
	private static string BuildSubFolderPath()
	{
		var savedGames = SHGetKnownFolderPath(KnownFolder_SavedGames, 0, IntPtr.Zero);
		var subFolder = Path.Combine(_subFolderElements);
		return Path.Combine(savedGames, subFolder);
	}
}
