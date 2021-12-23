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

		public override void Spawn()
		{

			base.Spawn();

		}
		// Do not look
		public string GetRandomFish( int rarity )
		{

			string[] selectArray = new string[100];
			int randomNumber = Rand.Int( 99 );
			int currentNumber = 0;

			foreach ( var fish in Game.FishVariety )
			{

				for ( int i = 0; i < fish.Value[rarity]; i++ )
				{
					selectArray[currentNumber] = fish.Key;
					currentNumber++;

				}

			}

			return selectArray[randomNumber];

		}

		[Event.Tick.Server]
		public void OnTick()
		{

			if ( Fishes.Count < fishNumber / ( 1 + RarityLevel / 2 ) )
			{

				string randomFish = GetRandomFish( RarityLevel );
				float fishSize = Game.FishSizes[randomFish];
				float randomSize = fishSize * (0.5f + RandomBell() * 1.5f);

				var fish = new Fish()
				{

					Position = Position + new Vector3( Rand.Float( -1, 1 ), Rand.Float( -1, 1 ), 0 ).Normal * Rand.Float( Range ),
					Rotation = Rotation.FromYaw( Rand.Float( 360 ) ),
					Species = randomFish,
					Variant = Game.FishUnlock[randomFish] ? Rand.Int( 0, 4 ) == 0 : false,
					Size = randomSize,
					Scale = 0,
					Spawner = this,
					FishList = Fishes,
					Rarity = Game.FishRarity[randomFish]

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
