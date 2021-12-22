using Sandbox;
using System;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float Warmth { get; set; } = 1f;
		[Net] public float ColdMultiplier { get; set; } = 1f; // Negative will recover warmth
		[Net] public float BaseColdSpeed { get; set; } = 30f; // Total seconds to perish in neutral conditions ( Standing on Dirt and not moving )
		[Net] public bool SuffersCold { get; set; } = true;

		public void HandleWarmth()
		{

			if ( IsClient ) return;

			Game current = Game.Current as Game;
			float hutDistance = Position.Distance( current.HutEntity.Position );

			if ( hutDistance <= 400f )
			{

				ColdMultiplier -= ( 1f - hutDistance / 400f ) * 7f;

			}

			if ( Game.IsOnIce( Position ) )
			{

				ColdMultiplier += 2f;

			}

			ColdMultiplier -= (1f - Game.CampfireDistance( Position, 240 ) / 240) * 7;

			Warmth = SuffersCold ? Math.Clamp( Warmth - Time.Delta * ColdMultiplier / BaseColdSpeed, 0, 1 ) : 1f;

			if ( Warmth == 0 )
			{

				Client.Kick(); // TODO: Don't haha :-)

			}

			ColdMultiplier = 1f;

		}

	}

}
