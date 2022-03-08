using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Fishing { get; set; } = false;
		public Dictionary<string, int> FishTotalCaught = new();
		public Dictionary<string, float> FishHighestRarity = new();
		public Dictionary<string, float> FishBiggest = new();

		public bool FishBaited { get; set; } = false;
		public List<Fish> CaughtFish { get; set; }
		RealTimeUntil removeTools { get; set; }

		public Player()
		{

			FishTotalCaught.Add( "goldfish", 0 );
			FishTotalCaught.Add( "minnow", 0 );
			FishTotalCaught.Add( "herring", 0 );
			FishTotalCaught.Add( "perch", 0 );
			FishTotalCaught.Add( "pike", 0 );
			FishTotalCaught.Add( "salmon", 0 );
			FishTotalCaught.Add( "trout", 0 );

			FishHighestRarity.Add( "goldfish", 0 );
			FishHighestRarity.Add( "minnow", 0 );
			FishHighestRarity.Add( "herring", 0 );
			FishHighestRarity.Add( "perch", 0 );
			FishHighestRarity.Add( "pike", 0 );
			FishHighestRarity.Add( "salmon", 0 );
			FishHighestRarity.Add( "trout", 0 );

			FishBiggest.Add( "goldfish", 0 );
			FishBiggest.Add( "minnow", 0 );
			FishBiggest.Add( "herring", 0 );
			FishBiggest.Add( "perch", 0 );
			FishBiggest.Add( "pike", 0 );
			FishBiggest.Add( "salmon", 0 );
			FishBiggest.Add( "trout", 0 );

		}

		public void HandleFishing()
		{

			if ( !Drilling )
			{

				SetClothing( "tool", (Fishing || removeTools >= 0) ? ( UpgradedRod ? "models/tools/better_fishingrod.vmdl" : "models/tools/basic_fishingrod.vmdl" ) : "none" );

			}

			SetAnimParameter( "fishing", Fishing );

			if ( Input.Released( InputButton.Attack2 ) )
			{

				if ( Fishing )
				{

					if ( FishBaited )
					{

						List<Fish> copyList = new(CaughtFish);

						foreach ( Fish fish in copyList )
						{

							if ( fish.IsValid )
							{
								FishBaited = false;

								FishTotalCaught[fish.Species]++;

								if ( fish.Size > FishBiggest[fish.Species] ) { FishBiggest[fish.Species] = fish.Size; }
								if ( fish.TotalRarity > FishHighestRarity[fish.Species] ) { FishHighestRarity[fish.Species] = fish.TotalRarity; }

								if( FishTotalCaught[fish.Species] >= 5 )
								{

									Game.FishUnlock[fish.Species] = true;

								}

								FishUI( To.Single(this), fish.Species, fish.Variant );

								GameServices.SubmitScore( Client.PlayerId, fish.TotalRarity );

								fish.Catch();

							}

						}

						copyList.Clear();

					}

					Fishing = false;
					BlockMovement = false;

					Play3D( "rod_woosh", this );

					removeTools = 0.66f;

					Hole hole = CurrentHole as Hole;
					hole.Bobber = false;

					CurrentHole = Sandbox.Internal.GlobalGameNamespace.Map.Entity;

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

				Sound.FromWorld( "", fxPosition );
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

		[ClientRpc]
		public static void FishUI( string species, bool variant)
		{

			Event.Run( "frostrial.fish_caught", species, variant );

		}

	}

}
