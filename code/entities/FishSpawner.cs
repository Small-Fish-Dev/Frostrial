using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class FishSpawner : AnimEntity
	{

		public Dictionary<string, float[]> FishVariety = new();
		public Dictionary<string, float> FishSizes = new();
		public Dictionary<string, float> FishRarity = new();
		[Net] public int RarityLevel { get; set; } = 0; // 0 = $, 1 = $$, 2 = $$$
		[Net] public float Range { get; set; } = 500f;
		[Net] public IList<Fish> Fishes { get; set;} = new List<Fish>();
		int fishNumber => (int)Range / 50;

		public override void Spawn()
		{

			FishVariety.Add( "goldfish",	new float[3] { 0, 0, 0 } );
			FishVariety.Add( "minnow",		new float[3] { 60, 5, 1 } );
			FishVariety.Add( "herring",		new float[3] { 30, 10, 4 } );
			FishVariety.Add( "perch",		new float[3] { 10, 50, 10 } );
			FishVariety.Add( "pike",		new float[3] { 0, 25, 15 } );
			FishVariety.Add( "salmon",		new float[3] { 0, 10, 50 } );
			FishVariety.Add( "trout",		new float[3] { 0, 0, 20 } );

			FishSizes.Add( "goldfish",		0.01f );
			FishSizes.Add( "minnow",		0.12f );
			FishSizes.Add( "herring",		0.24f );
			FishSizes.Add( "perch",			0.40f );
			FishSizes.Add( "pike",			0.60f );
			FishSizes.Add( "salmon",		0.80f );
			FishSizes.Add( "trout",			1.00f );

			FishRarity.Add( "goldfish",		0.1f );
			FishRarity.Add( "minnow",		0.2f );
			FishRarity.Add( "herring",		0.3f );
			FishRarity.Add( "perch",		0.5f );
			FishRarity.Add( "pike",			0.6f );
			FishRarity.Add( "salmon",		0.9f );
			FishRarity.Add( "trout",		1.0f );

			base.Spawn();


		}
		// Do not look
		public string GetRandomFish( int rarity )
		{

			string[] selectArray = new string[100];
			int randomNumber = Rand.Int( 99 );
			int currentNumber = 0;

			foreach ( var fish in FishVariety )
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
				float fishSize = FishSizes[randomFish];
				float randomSize = fishSize * (0.5f + RandomBell() * 1.5f);

				var fish = new Fish();
				fish.Position = Position + new Vector3( Rand.Float( -1, 1 ), Rand.Float( -1, 1 ), 0 ).Normal * Rand.Float( Range );
				fish.Rotation = Rotation.FromYaw( Rand.Float( 360 ) );
				fish.Species = randomFish;
				fish.Size = randomSize;
				fish.Spawner = this;
				fish.FishList = Fishes;
				fish.Rarity = FishRarity[randomFish];

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
