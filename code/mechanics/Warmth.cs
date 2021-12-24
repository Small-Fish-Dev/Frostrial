using Sandbox;
using System;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float Warmth { get; set; } = 1f;
		[Net] public float ColdMultiplier { get; set; } = 1f; // Negative will recover warmth
		[Net] public float BaseColdSpeed { get; set; } = 60f; // Total seconds to perish in neutral conditions ( Standing on Dirt and not moving )
		[Net] public bool SuffersCold { get; set; } = true;

		public void HandleWarmth()
		{

			if ( IsClient ) return;

			float hutDistance = Position.Distance( Game.HutEntity.Position );

			if ( UpgradedCoat )
			{

				ColdMultiplier = 0.5f;

			}

			if ( Game.IsInside( Position, new Vector3( -1395, -2745, 0 ), new Vector3( -1164, -2394, 40 ) ) )
			{

				ColdMultiplier -= 7;

			}

			if ( Game.IsOnIce( Position ) )
			{

				ColdMultiplier += UpgradedCoat ? 0.5f : 2f;

			}

			ColdMultiplier -= (1f - Game.CampfireDistance( Position, 240 ) / 240) * 7;

			ColdMultiplier += Velocity.Length / 150f / ( UpgradedCoat ? 2 : 1 ) ; // The faster you move, the colder you get. So people don't venture out too far without upgrading

			Warmth = SuffersCold ? Math.Clamp( Warmth - Time.Delta * ColdMultiplier / BaseColdSpeed, 0, 1 ) : 1f;

			if ( Warmth == 0 )
			{

				Client.Kick(); // TODO: Don't haha :-)

			}

			ColdMultiplier = 1f;

		}

	}

}
