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
		[Net] public bool Baited { get; set; } = false;
		[Net] public bool Trapped { get; set; } = false;
		[Net] public Entity Fisherman { get; set; }
		[Net] internal RealTimeUntil growth { get; set; } = 1;

		public Rotation WishRotation { get; set; }
		public FishSpawner Spawner { get; set; }
		public IList<Fish> FishList { get; set; }

		RealTimeUntil freeFromBait = 0f;
		Vector3 baitPosition = Vector3.Zero;

		RealTimeUntil nextAction = 0f;
		RealTimeSince lerpPosition = 0f;
		Vector3 originalPosition;
		Rotation originalRotation;

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/fishes/fishshadow.vmdl" );
			EnableShadowCasting = false;

			Velocity = Vector3.Forward * 50f;

		}

		[Event.Tick.Server]
		public void OnTick()
		{

			Scale = Size * 3 * Math.Min( 1 - growth, 1 );

			if ( Baited )
			{
				///////// During action

				if ( nextAction <= 0f )
				{

					lerpPosition = 0;

					nextAction = Rand.Float( 1.5f, 2.5f );

				}

				///////// During every tick

				if ( Trapped )
				{
					Rotation = originalRotation + Rotation.FromYaw( Time.Now * 300 ) * ((float)Math.Cos( Time.Now * 15 ) * 0.05f);

					if ( freeFromBait <= 0 )
					{

						Baited = false;
						Trapped = false;
						Velocity = new Vector3( Rand.Float( 2f ) - 1f, Rand.Float( 2f ) - 1f, 0f ).Normal * Rand.Float( 120f, 200f );
						Rotation = originalRotation;

						Player player = Fisherman as Player;

						player.FishBaited = false;
						player.CaughtFish = null;

						nextAction = Rand.Float( 2f, 3f );

					}

				}
				else
				{

					Position = Vector3.Lerp( originalPosition, baitPosition, lerpPosition );
					Rotation = Rotation.Slerp( Rotation, WishRotation, Time.Delta * 4 );

				}

			}
			else
			{

				///////// During action
				
				if ( nextAction <= 0f )
				{

					Entity nearPlayer = Game.NearestPlayer( Position, 200f ); // TODO Bigger search if you put bait and have rod

					if ( nearPlayer is Player )
					{

						Player player = nearPlayer as Player;
						Entity nearHole = player.CurrentHole;

						if ( nearHole is Hole )
						{

							var holePosition = nearHole.Position.WithZ( Position.z );
							Baited = true;
							Fisherman = player;
							WishRotation = Rotation.LookAt( holePosition - Position, Vector3.Up );
							originalPosition = Position;
							baitPosition = holePosition;
							lerpPosition = 0;

						}

					}

					if ( Position.Distance( Spawner.Position ) > Spawner.Range )
					{

						// Go back towards the center of your zone
						Velocity = (Spawner.Position - Position).Normal * 100f;

					}
					else
					{

						// Go to random direction
						Velocity = new Vector3( Rand.Float( 2f ) - 1f, Rand.Float( 2f ) - 1f, 0f ).Normal * Rand.Float( 40f, 100f );

					}

					nextAction = Rand.Float( 1.5f, 3.5f );

				}

				///////// During every tick
				
				Position += Rotation.Forward * Velocity.Length * Time.Delta;

				WishRotation = Velocity.EulerAngles.ToRotation();
				Rotation = Rotation.Slerp( Rotation, WishRotation, Time.Delta * 2 );

			}

		/*	if ( Baited )
			{

				

				if ( Position.Distance( baitPosition ) <= 40f / Scale )
				{

					Player player = Fisherman as Player;

					if ( player.Fishing ) //Check if he's still fishing hahaa
					{

						TryBait();

					}
					else
					{

						Baited = false;

					}

				}


			}*/

		}
		/*
		public void TryBait()
		{

			if ( nextBaitTest <= 0f )
			{

				//TODO Play the sound bait check here

				float random = Rand.Float( 10 ); // Set back to 10

				if( random <= 1f / Rarity ) // TODO Better with tools and bait
				{

					originalRotation = Rotation;
					freeFromBait = 0.6f / Rarity;
					Trapped = true;

					Player player = Fisherman as Player;

					player.FishBaited = true;
					player.CaughtFish = this;
					
					//TODO PLAY TRAPPED SOUND and particles

				}

				Velocity = -Velocity;

				nextBaitTest = nextMove;

			}

		}

		public void Catch()
		{

			CreateEffects( this );

			Vector3 throwDirection = (Fisherman.Position - baitPosition).Normal * 140  ;

			var ragdoll = new DeadFish( Species, Size, Variant, Rarity )
			{

				Position = baitPosition + Vector3.Up * Size * 60,
				Rotation = Rotation.LookAt( throwDirection )

			};
			ragdoll.PhysicsGroup.Velocity = throwDirection + Vector3.Up * 400f;
			ragdoll.PhysicsGroup.AngularVelocity = Vector3.Cross( throwDirection, Vector3.Up).Normal * -10 ;

			FishList.Remove( this );
			Delete();

		}*/

		[ClientRpc]
		public static void CreateEffects( Fish fish )
		{

			//TODO Sounds and particles
			Particles.Create( "particles/splash_particle.vpcf", fish.baitPosition + Vector3.Up * 10 );
			


		}

	}

}
