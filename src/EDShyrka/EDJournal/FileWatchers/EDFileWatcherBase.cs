using Microsoft.Extensions.Logging;
using System.IO;

namespace EDShyrka.EDJournal.FileWatchers;

/// <summary>
/// Base class for file watchers.
/// </summary>
public abstract class EDFileWatcherBase
{
	#region fields
	/// <summary>
	/// ILogger instance.
	/// </summary>
	protected readonly ILogger _logger;

	/// <summary>
	/// File changes monitor instance.
	/// </summary>
	private readonly FileSystemWatcher _fileWatcher;
	#endregion fields

	#region ctor
	/// <summary>
	/// Initializes a new instance of the <see cref="EDFileWatcherBase"/> class with the specified journal folder.
	/// </summary>
	/// <param name="logger">ILogger instance.</param>
	/// <param name="journalFolder">Provides folder path to monitor for file changes.</param>
	public EDFileWatcherBase(ILogger logger, EDJournalFolder journalFolder)
	{
		_logger = logger;
		_fileWatcher = StartWatchingFile(journalFolder);
	}
	#endregion ctor

	#region properties
	/// <summary>
	/// Gets the filter string used to identify files to be watched.
	/// </summary>
	protected abstract string WatchedFileFilter { get; }
	#endregion properties

	#region methods
	/// <summary>
	/// This method is triggered whenever a change is detected in a monitored file.
	/// </summary>
	/// <param name="name">The name of the file that has changed.</param>
	/// <param name="fullPath">The full path to the file that has changed.</param>
	protected abstract void WatchedFileChanged(string? name, string fullPath);

	// monitor folder for new files
	// monitor last file for changes
	// raise event on new file or file change
	/// <summary>
	/// Initializes and starts a <see cref="FileSystemWatcher"/> to monitor a specified folder for changes in files.
	/// </summary>
	/// <param name="journalFolder">Provides folder path to monitor for file changes.</param>
	/// <returns>A <see cref="FileSystemWatcher"/> configured to raise events when files in the specified folder are changed.</returns>
	private FileSystemWatcher StartWatchingFile(EDJournalFolder journalFolder)
	{
		var fileWatcher = new FileSystemWatcher(journalFolder.Value, WatchedFileFilter);
		fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
		fileWatcher.Changed += OnFileChanged;
		fileWatcher.EnableRaisingEvents = true;
		return fileWatcher;
	}

	/// <summary>
	/// Handles the event when a file in the monitored directory changes.
	/// </summary>
	/// <param name="sender">The source of the event, typically the file system watcher.</param>
	/// <param name="e">The event data containing information about the file change, including the name and full path of the affected file.</param>
	private void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		WatchedFileChanged(e.Name, e.FullPath);
	}
	#endregion methods
}
