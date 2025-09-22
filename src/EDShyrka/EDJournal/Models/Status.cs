namespace EDShyrka.EDJournal.Models
{
	/// <summary>
	/// Current status of the ship and commander.
	/// https://jixxed.github.io/ed-journal-schemas/Status.html
	/// https://doc.elitedangereuse.fr/Status%20File/
	/// </summary>
	public partial class Status: EDJournalEntry
	{
		/// <summary>
		/// Flags indicating status of different parts of the ship or SRV.
		/// </summary>
		public StatusFlags1 Flags { get; set; }

		/// <summary>
		/// Flags indicating status of different parts of the ship or Cmdr.
		/// </summary>
		public StatusFlags2	Flags2 { get; set; }

		/// <summary>
		/// Pips levels for systems, engines, and weapons.
		/// </summary>
		public int[] Pips { get; set; }

		/// <summary>
		/// Currently selected firegroup.
		/// </summary>
		public int Firegroup { get; set; }

		/// <summary>
		/// Ship's current fuel status.
		/// </summary>
		public StatusFuel Fuel { get; set; }

		/// <summary>
		/// Indicates which screen is currently focused by Cmdr.
		/// </summary>
		public StatusGuiFocus GuiFocus { get; set; }

		/// <summary>
		/// When near a planetary body, the name of the body.
		/// </summary>
		public string BodyName { get; set; }

		/// <summary>
		/// When near a planetary body, the radius of the body in meters.
		/// </summary>
		public float PlanetRadius { get; set; }

		/// <summary>
		/// When near a planteray body, the latitude of the ship/srv in degrees.
		/// Need to change by 0.02 degrees to trigger an update when flying, or by 0.0005 degrees when in the SRV.
		/// </summary>
		public float Latitude { get; set; }

		/// <summary>
		/// When near a planetary body, the longitude of the ship/srv in degrees.
		/// Need to change by 0.02 degrees to trigger an update when flying, or by 0.0005 degrees when in the SRV.
		/// </summary>
		public float Longitude { get; set; }

		/// <summary>
		/// When near a planetary body, the heading of the ship/srv in degrees.
		/// </summary>
		public int Heading { get; set; }

		/// <summary>
		/// The ship's altitude in meters when close to a planetary body.
		/// If Flags.bit29 is set, the altitude value is based on the planet's average radius (used at higher altitudes).
		/// If Flags.bit29 is not set, the Altitude value is based on a raycast to the actual surface below the ship/srv.
		/// </summary>
		public int Altitude { get; set; }

		/// <summary>
		/// Planet's gravity.
		/// </summary>
		public float Gravity { get; set; }

		/// <summary>
		/// Cargo mass in tons.
		/// </summary>
		public float Cargo { get; set; }

		/// <summary>
		/// The legal state of the ship in the current system.
		/// Can be one of: 
		/// Clean, IllegalCargo, Speeding, Wanted, Hostile,
		/// PassengerWanted, Warrant, Allied, Thargoid
		/// </summary>
		public string LegalState { get; set; }

		/// <summary>
		/// The current balance of the Cmdr in credits.
		/// </summary>
		public ulong Balance { get; set; }

		public StatusDestination Destination { get; set; }

		#region on foot
		/// <summary>
		/// Cmdr health percentage (between 0.00 to 1.00).
		/// </summary>
		public double Health { get; set; }

		/// <summary>
		/// Cmdr oxygen percentage (between 0.00 to 1.00).
		/// </summary>
		public double Oxygen { get; set; }

		/// <summary>
		/// Temperature in kelvin.
		/// </summary>
		public double Temperature { get; set; }

		/// <summary>
		/// Technical name of the currently selected weapon.
		/// </summary>
		/// <example>
		/// $humanoid_fists_name;
		/// $wpn_m_sniper_plasma_charged_name;
		/// </example>
		public string SelectedWeapon { get; set; }

		/// <summary>
		/// Localised name of the currently selected weapon.
		/// Localisation occurs in the game client language.
		/// </summary>
		/// <example>
		/// Unarmed
		/// Manticore Executioner
		/// </example>
		public string SelectedWeapon_Localised { get; set; }
		#endregion on foot
	}
}
