using Microsoft.Extensions.Logging;

namespace EDShyrka.EDJournal.FileWatchers;

/// <summary>
/// Watch json state files for changes.
/// The game overwrites different state files while playing.
/// FCMaterials.json : Fleet Carrier Materials
/// Backpack.json : SRV Backpack contents
/// Market.json : Market information
/// ShipLocker.json : Ship Locker contents
/// ModulesInfo.json : Ship modules information
/// Outfitting.json : Ship outfitting information
/// Shipyard.json : Shipyard information
/// Cargo.json : Ship cargo contents
/// NavRoute.json : Current navigation route
/// Status.json : Current ship status
/// </summary>
public class EDJsonFilesWatcher : EDFileWatcherBase
{
	#region fields
	/// <summary>
	/// Represents the file extension for JSON files.
	/// </summary>
	private const string _jsonFileExtension = ".json";
	#endregion fields

	#region ctor
	/// <summary>
	/// Initializes a new instance of the <see cref="EDJournalLogsWatcher"/> class with the specified journal folder.
	/// </summary>
	/// <param name="logger">ILogger instance.</param>
	/// <param name="journalFolder">Provides folder path to monitor for file changes.</param>
	public EDJsonFilesWatcher(ILogger<EDJsonFilesWatcher> logger, EDJournalFolder journalFolder)
		: base(logger, journalFolder)
	{
	}
	#endregion ctor

	#region properties
	/// <summary>
	/// Gets the filter string used to identify files to be watched.
	/// </summary>
	protected override string WatchedFileFilter { get; } = $"*{_jsonFileExtension}";
	#endregion properties

	#region methods
	/// <summary>
	/// This method is triggered whenever a change is detected in a monitored file.
	/// </summary>
	/// <param name="name">The name of the file that has changed.</param>
	/// <param name="fullPath">The full path to the file that has changed.</param>
	protected override void WatchedFileChanged(string? name, string fullPath)
	{
	}
	#endregion methods
}
