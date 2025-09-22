namespace EDShyrka.EDJournal.Models
{
	/// <summary>
	/// Indicates which part of the GUI is currently in focus.
	/// </summary>
	public enum StatusGuiFocus
	{
		/// <summary>
		/// Represents an unknown or undefined value.
		/// </summary>
		UnknowValue = -1,

		/// <summary>
		/// Looking ahead.
		/// </summary>
		NoFocus = 0,

		/// <summary>
		/// Right hand side panel.
		/// </summary>
		InternalPanel = 1,

		/// <summary>
		/// Left hand side panel.
		/// </summary>
		ExternalPanel = 2,

		/// <summary>
		/// Comms panel, top left.
		/// </summary>
		CommsPanel = 3,

		/// <summary>
		/// Role panel, bottom.
		/// </summary>
		RolePanel = 4,

		/// <summary>
		/// Station services screen.
		/// </summary>
		StationServices = 5,

		/// <summary>
		/// Galaxy map is open.
		/// </summary>
		GalaxyMap = 6,

		/// <summary>
		/// Looking to system map.
		/// </summary>
		SystemMap = 7,

		/// <summary>
		/// System map in orrery mode.
		/// </summary>
		Orrery = 8,

		/// <summary>
		/// Full Spectrum Scanner mode.
		/// </summary>
		FSS = 9,

		/// <summary>
		/// Surface Area Analysis view.
		/// </summary>
		SAA = 10,

		/// <summary>
		/// Codex view.
		/// </summary>
		Codex = 11,
	}
}
