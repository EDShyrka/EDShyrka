using System;

namespace EDShyrka.EDJournal.Models
{
	/// <summary>
	/// Flags indicating status of different parts of the ship or SRV.
	/// </summary>
	[Flags]
	public enum StatusFlags1 : uint
	{
		/// <summary>
		/// Docked, (on a landing pad).
		/// </summary>
		Docked = 1U << 0,

		/// <summary>
		/// Landed (on surface planet).
		/// </summary>
		Landed = 1U << 1,

		/// <summary>
		/// Landing gear is deployed.
		/// </summary>
		LandingGearDown = 1U << 2,

		/// <summary>
		/// Fields are up.
		/// </summary>
		ShieldsUp = 1U << 3,

		/// <summary>
		/// Ship is navigating in supercruise.
		/// </summary>
		Supercruise = 1U << 4,

		/// <summary>
		/// Flight assist is disabled.
		/// </summary>
		FlightAssistOff = 1U << 5,

		/// <summary>
		/// Weapons are deployed.
		/// </summary>
		HardpointsDeployed = 1U << 6,

		/// <summary>
		/// Cmdr is in a wing.
		/// </summary>
		InWing = 1U << 7,

		/// <summary>
		/// Lights are on (ship or SRV).
		/// </summary>
		LightsOn = 1U << 8,

		/// <summary>
		/// Cargo scoop is open.
		/// </summary>
		CargoScoopDeployed = 1U << 9,

		/// <summary>
		/// Silent running is enabled.
		/// </summary>
		SilentRunning = 1U << 10,

		/// <summary>
		/// Currently scooping fuel.
		/// </summary>
		ScoopingFuel = 1U << 11,

		/// <summary>
		/// SRV handbreak is activated.
		/// </summary>
		SrvHandbrake = 1U << 12,

		/// <summary>
		/// SRV turret is in use.
		/// </summary>
		SrvTurret = 1U << 13,

		/// <summary>
		/// SRV turret is retracted.
		/// </summary>
		SrvUnderShip = 1U << 14,

		/// <summary>
		/// SRV drive assist is enabled.
		/// </summary>
		SrvDriveAssist = 1U << 15,

		/// <summary>
		/// Ship is mass locked, can't use FSD.
		/// </summary>
		FsdMassLocked = 1U << 16,

		/// <summary>
		/// FSD is charging.
		/// </summary>
		FsdCharging = 1U << 17,

		/// <summary>
		/// FSD is cooling down.
		/// </summary>
		FsdCooldown = 1U << 18,

		/// <summary>
		/// Fuel is below 25%.
		/// </summary>
		LowFuel = 1U << 19,

		/// <summary>
		/// Heat reached 100%.
		/// </summary>
		OverHeating = 1U << 20,

		/// <summary>
		/// Latitude and longitude available.
		/// </summary>
		HasLatLong = 1U << 21,

		/// <summary>
		/// Danger warning.
		/// </summary>
		IsInDanger = 1U << 22,

		/// <summary>
		/// Being interdicted by another ship.
		/// </summary>
		BeingInterdicted = 1U << 23,

		/// <summary>
		/// Cmdr is in main ship.
		/// </summary>
		InMainShip = 1U << 24,

		/// <summary>
		/// Cmdr is in fighter.
		/// </summary>
		InFighter = 1U << 25,

		/// <summary>
		/// Cmdr is in SRV.
		/// </summary>
		InSRV = 1U << 26,

		/// <summary>
		/// Analysis mode active.
		/// </summary>
		HudInAnalysisMode = 1U << 27,

		/// <summary>
		/// Night vision enabled.
		/// </summary>
		NightVision = 1U << 28,

		/// <summary>
		/// Altimeter mode.
		/// If bit is set, altitude value is based on planet's average radius.
		/// If bit is unset, altitude is raycasted from ship to surface.
		/// </summary>
		Altimeter = 1U << 29,

		/// <summary>
		/// Ship is in hyperspace jump.
		/// </summary>
		FsdJump = 1U << 30,

		/// <summary>
		/// SRV high beams are on.
		/// </summary>
		SrvHighBeam = 1U << 31
	}
}
