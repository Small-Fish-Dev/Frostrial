using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class Fish : AnimEntity
	{

		[Net] public string Species { get; set; } = "goldfish";
		[Net] public float Size { get; set; } = 0.1f; // Meters
		[Net] public bool Variant { get; set; } = false;
		public FishSpawner Spawner { get; set; }
		public IList<Fish> FishList;
		[Net] RealTimeUntil growth { get; set; }
		RealTimeUntil nextMove = 0f;

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/fishes/fishshadow.vmdl" );
			EnableShadowCasting = false;
			growth = 1;

			Velocity = Vector3.Forward * 50f;

		}

		[Event.Tick.Server]
		public void OnTick()
		{

			if ( nextMove < 0f )
			{

				if ( Position.Distance( Spawner.Position ) > Spawner.Range )
				{

					Velocity = ( Spawner.Position - Position).Normal * 70f;

				}
				else
				{

					Velocity = new Vector3( Rand.Float( 2f ) - 1f, Rand.Float( 2f ) - 1f, 0f ).Normal * Rand.Float( 20f, 60f );

				}

				nextMove = Rand.Float( 2, 7 );

			}

			Scale = Size * 3 * Math.Min( 1 - growth, 1 );

			Position += Rotation.Forward * Velocity.Length * Time.Delta;

			Rotation rotation = Velocity.EulerAngles.ToRotation();
			Rotation = Rotation.Slerp( Rotation, rotation, 2 * Time.Delta );

		}

	}

}
