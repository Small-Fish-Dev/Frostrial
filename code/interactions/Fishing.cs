using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Fishing { get; set; } = false;
		public bool FishBaited { get; set; } = false;
		public Fish CaughtFish { get; set; }
		RealTimeUntil removeTools { get; set; }

		public void HandleFishing()
		{

			if ( !Drilling )
			{

				SetClothing( "tool", (Fishing || removeTools >= 0) ? ( UpgradedRod ? "models/tools/better_fishingrod.vmdl" : "models/tools/basic_fishingrod.vmdl" ) : "none" );

			}

			SetAnimBool( "fishing", Fishing );

			if ( Input.Released( InputButton.Attack2 ) )
			{

				if ( Fishing )
				{

					if ( FishBaited )
					{

						if ( CaughtFish.IsValid )
						{

							CaughtFish.Catch();
							FishBaited = false;

						}

					}

					Fishing = false;
					BlockMovement = false;

					removeTools = 0.66f;

					Hole hole = CurrentHole as Hole;
					hole.Bobber = false;

					CurrentHole = PhysicsWorld.WorldBody.Entity;

				}

			}

		}


		Particles fishingParticle { get; set; }
		Sound fishingSound { get; set; }

		[ClientRpc]
		public void HandleFishingEffects( bool fxState, Vector3 fxPosition )
		{

			if ( fxState )
			{

				Sound.FromWorld( "", fxPosition ); // TODO Play the drilling
				fishingParticle = Particles.Create( "particles/drilling_particle.vpcf", fxPosition );

			}
			else
			{

				if ( fishingParticle != null )
				{

					fishingParticle.Destroy();

				}

			}

		}

	}

}
