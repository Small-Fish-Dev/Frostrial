using Sandbox;
using System;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float Warmth { get; set; } = 1f;
		[Net] public float ColdMultiplier { get; set; } = 1f; // Negative will recover warmth
		[Net] public float BaseColdSpeed { get; set; } = 30f; // Total seconds to perish in neutral conditions ( Standing on Dirt and not moving )

		public void HandleWarmth()
		{

			Warmth = Math.Clamp( Time.Delta * ColdMultiplier / BaseColdSpeed, 0, 1 );

		}

	}

}
