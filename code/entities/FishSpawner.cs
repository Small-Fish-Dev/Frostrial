using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class FishSpawner : AnimEntity
	{

		[Net] public int RarityLevel { get; set; } = 0; // 0 = $, 1 = $$, 2 = $$$
		[Net] public float Range { get; set; } = 500f;
		[Net] public IList<Fish> Fishes { get; set;} = new List<Fish>();
		int fishNumber => (int)Range / 50;

		// Do not look
		public string GetRandomFish( int rarity )
		{

			int totalValue = 0;
			Dictionary<string, int> fishWeightedValues = new();
			string selectFish = "goldfish";

			foreach ( var fish in FishAsset.All ) // With the basic fishes it should always be 100, just in case I want to add more in the future.
			{

				totalValue += fish.Value.ZoneValue( rarity );
				fishWeightedValues[fish.Key] = totalValue;

			}
			int randomNumber = Rand.Int( totalValue );


			foreach ( var weightedValue in fishWeightedValues )
			{

				if ( weightedValue.Value >= randomNumber )
				{

					selectFish = weightedValue.Key;
					break;

				}

			}

			return selectFish;

		}

		[Event.Tick.Server]
		public void OnTick()
		{

			if ( Fishes.Count < fishNumber / ( 1 + RarityLevel / 2 ) )
			{

				string randomFish = GetRandomFish( RarityLevel );
				float fishSize = FishAsset.All[randomFish].Size;
				float randomSize = fishSize * (0.5f + RandomBell() * 1.5f);

				var fish = new Fish()
				{

					Position = Position + new Vector3( Rand.Float( -1, 1 ), Rand.Float( -1, 1 ), 0 ).Normal * Rand.Float( Range ),
					Rotation = Rotation.FromYaw( Rand.Float( 360 ) ),
					Species = randomFish,
					Variant = Game.FishUnlock[randomFish] && Rand.Int( 0, 4 ) == 0,
					Size = randomSize,
					Scale = 0,
					Spawner = this,
					FishList = Fishes,
					Rarity = FishAsset.All[randomFish].Rarity

				};

				Fishes.Add( fish );

			}


		}

		public float RandomBell()
		{

			Random rnd = new Random( Rand.Int( 100 ) );
			return (float)( Math.Pow( 2 * rnd.NextDouble() - 1, 3 ) / 2 + 0.5 );


		}

		[Event.Tick.Client]
		public void ClientTick()
		{

			//DebugOverlay.Circle( Position, Rotation.FromPitch( 90 ), Range, new Color( 1, 0, 0, 0.7f ) );

		}

	}

}
