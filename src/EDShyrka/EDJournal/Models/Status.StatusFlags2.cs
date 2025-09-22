using System;

namespace EDShyrka.EDJournal.Models
{
	/// <summary>
	/// Flags indicating status of different parts of the ship or Cmdr.
	/// </summary>
	[Flags]
	public enum StatusFlags2 : uint
	{
		/// <summary>
		/// Cmdr is onfoot.
		/// </summary>
		OnFoot = 1U << 0,

		/// <summary>
		/// Cmdr is in taxi, can be a shuttle or a dropship.
		/// </summary>
		Taxi = 1U << 1,

		/// <summary>
		/// Cmdr is in another Cmdr ship.
		/// </summary>
		InMulticrew = 1U << 2,

		/// <summary>
		/// Cmdr is onfoot in a station.
		/// </summary>
		OnFootInStation = 1U << 3,

		/// <summary>
		/// Cmdr is onfoot on a planet.
		/// </summary>
		OnFootOnPlanet = 1U << 4,

		AimDownSight = 1U << 5,

		/// <summary>
		/// Oxygen is below 25%.
		/// </summary>
		LowOxygen = 1U << 6,

		/// <summary>
		/// Health is below 25%.
		/// </summary>
		LowHealth = 1U << 7,

		/// <summary>
		/// Cmdr is in a cold place.
		/// </summary>
		Cold = 1U << 8,

		/// <summary>
		/// Cmdr is in a hot place.
		/// </summary>
		Hot = 1U << 9,

		/// <summary>
		/// Cmdr is in a very cold place.
		/// </summary>
		VeryCold = 1U << 10,

		/// <summary>
		/// Cmdr is in a very hot place.
		/// </summary>
		VeryHot = 1U << 11,

		/// <summary>
		/// Ship is gliding to a planet surface.
		/// </summary>
		GlideMode = 1U << 12,

		OnFootInHangar = 1U << 13,

		OnFootSocialSpace = 1U << 14,

		OnFootExterior = 1U << 15,

		BreathableAtmosphere = 1U << 16,

		TelepresenceMulticrew = 1U << 17,

		PhysicalMulticrew = 1U << 18,

		FsdHyperdriveCharging = 1U << 19,
	}
}
