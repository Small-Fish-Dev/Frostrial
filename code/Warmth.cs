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

			Log.Info( Warmth );
			Log.Info( ColdMultiplier );

			if ( IsClient ) return;

			Game current = Game.Current as Game;
			float hutDistance = Position.Distance( current.Hut.Position );

			if ( hutDistance <= 400f )
			{

				ColdMultiplier -= 6f;

			}

			Warmth = Math.Clamp( Warmth - Time.Delta * ColdMultiplier / BaseColdSpeed, 0, 1 );

			ColdMultiplier = 1f;

		}

	}

}
