using Microsoft.Extensions.Logging;

namespace EDShyrka.EDJournal.FileWatchers;

/// <summary>
/// Watch for 'Journal.' prefixed files for changes.
/// The files contains game events and are updated while playing.
/// </summary>
public class EDJournalLogsWatcher : EDFileWatcherBase
{
	#region fields
	/// <summary>
	/// Represents the file prefix used for journal log files.
	/// </summary>
	private const string _journalPrefix = "Journal.";

	/// <summary>
	/// Represents the file extension used for journal log files.
	/// </summary>
	private const string _journalExtension = ".??.log";

	/// <summary>
	/// Legacy journal date time pattern.
	/// </summary>
	/// <example>Journal.211231235959.01.log</example>
	private const string _journalPattern01 = "yyMMddHHmmss";

	/// <summary>
	/// Updated journal date time pattern.
	/// </summary>
	/// <example>Journal.2025-09-10T222316.01.log</example>
	private const string _journalPattern02 = "yyyy-MM-ddTHHmmss";
	#endregion fields

	#region ctor
	/// <summary>
	/// Initializes a new instance of the <see cref="EDJournalLogsWatcher"/> class with the specified journal folder.
	/// </summary>
	/// <param name="logger">ILogger instance.</param>
	/// <param name="journalFolder">Provides folder path to monitor for file changes.</param>
	public EDJournalLogsWatcher(ILogger<EDJournalLogsWatcher> logger, EDJournalFolder journalFolder)
		: base(logger, journalFolder)
	{
	}
	#endregion ctor

	#region properties
	/// <summary>
	/// Gets the filter string used to identify files to be watched.
	/// </summary>
	protected override string WatchedFileFilter { get; } = $"{_journalPrefix}*{_journalExtension}";
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
