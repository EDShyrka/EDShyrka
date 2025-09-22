namespace EDShyrka.EDJournal.Models
{
	/// <summary>
	/// Amount of fuel in the main tank and reservoir.
	/// </summary>
	public class StatusFuel
	{
		/// <summary>
		/// Amount of fuel in the main tank, in tons.
		/// </summary>
		public float FuelMain { get; set; }

		/// <summary>
		/// Amount of fuel in the reservoir, in tons.
		/// </summary>
		public float FuelReservoir { get; set; }
	}
}
