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
		[Net] public float Rarity { get; set; } = 0;
		public bool Baited { get; set; } = false;
		public bool Trapped { get; set; } = false;
		RealTimeUntil freeFromBait = 0f;
		public Vector3 BaitPosition { get; set; } = Vector3.Zero;
		public FishSpawner Spawner { get; set; }
		public IList<Fish> FishList;
		public Entity Fisherman { get; set; }
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

				Entity nearPlayer = Game.NearestPlayer( Position, 200f ); // TODO Bigger search if you put bait and have rod

				if ( nearPlayer is Player )
				{

					Player player = nearPlayer as Player;
					Entity nearHole = player.CurrentHole;

					if ( nearHole is Hole )
					{

						var holePosition = nearHole.Position.WithZ( Position.z );
						Velocity = (holePosition - Position).Normal * 120f;
						Baited = true;
						BaitPosition = holePosition;
						Fisherman = player;

					}

				}
				else
				{

					Baited = false;

				}

				if ( !Baited )
				{

					if ( Position.Distance( Spawner.Position ) > Spawner.Range )
					{

						Velocity = (Spawner.Position - Position).Normal * 70f;

					}
					else
					{

						Velocity = new Vector3( Rand.Float( 2f ) - 1f, Rand.Float( 2f ) - 1f, 0f ).Normal * Rand.Float( 20f, 60f );

					}

				}

				nextMove = Rand.Float( 1, 4 );

			}

			if ( Baited )
			{

				if ( Position.Distance( BaitPosition ) <= 30f )
				{

					TryBait();

				}

			}

			Scale = Size * 3 * Math.Min( 1 - growth, 1 );

			Position += Rotation.Forward * Velocity.Length * Time.Delta;

			Rotation rotation = Velocity.EulerAngles.ToRotation();
			Rotation = Rotation.Slerp( Rotation, rotation, Time.Delta * ( Baited ? 40 : 2 ) );

		}

		RealTimeUntil nextBaitTest = 0f;

		public void TryBait()
		{

			if ( nextBaitTest <= 0f )
			{

				//TODO Play the sound bait check here

				float random = Rand.Float( 10 );

				if( random <= 1f / Rarity ) // TODO Better with tools and bait
				{

					FishList.Remove( this );
					Delete();

				}

				nextBaitTest = 1f;

			}

		}

	}

}
