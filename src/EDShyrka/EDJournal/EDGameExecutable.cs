using System;
using System.Diagnostics;
using System.Linq;

namespace EDShyrka.EDJournal;

/// <summary>
/// Provide information about the Elite Dangerous game executable.
/// </summary>
public class EDGameExecutableInfo
{
	#region fields
	private const string _exeName = "EliteDangerous64";
	private Process? _edProcess;
	#endregion fields

	#region properties
	public bool IsRunning
	{
		get
		{
			var process = GetProcess();
			return process != null && process.HasExited == false;
		}
	}

	public TimeSpan UpTime
	{
		get
		{
			var process = GetProcess();
			return process != null
				? DateTime.Now - process.StartTime
				: TimeSpan.Zero;
		}
	}
	#endregion properties

	#region methods
	private Process? GetProcess()
	{
		if (_edProcess == null || _edProcess.HasExited)
		{
			var processes = Process.GetProcessesByName(_exeName);
			_edProcess = processes.FirstOrDefault();
		}
		return _edProcess;
	}
	#endregion methods
}
