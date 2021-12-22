using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class FishSpawner : AnimEntity
	{

		public Dictionary<string, float[]> FishVariety = new();
		[Net] public int RarityLevel { get; set; } = 0; // 0 = $, 1 = $$, 2 = $$$
		[Net] public float Range { get; set; } = 500f;
		[Net] public List<Fish> Fishes { get; set;} = new List<Fish>();
		int fishNumber => (int)Range / 100;

		public override void Spawn()
		{

			FishVariety.Add( "goldfish",	new float[3] { 0, 0, 0 } );
			FishVariety.Add( "minnow",		new float[3] { 60, 5, 1 } );
			FishVariety.Add( "herring",		new float[3] { 30, 10, 4 } );
			FishVariety.Add( "perch",		new float[3] { 10, 50, 10 } );
			FishVariety.Add( "pike",		new float[3] { 0, 25, 15 } );
			FishVariety.Add( "salmon",		new float[3] { 0, 10, 50 } );
			FishVariety.Add( "trout",		new float[3] { 0, 0, 20 } );

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

			if ( Fishes.Count < fishNumber )
			{

				var fish = new Fish();
				fish.Position = Position + new Vector3( Rand.Float( -1, 1 ), Rand.Float( -1, 1 ), 0 ).Normal * Rand.Float( Range );
				fish.Rotation = Rotation.FromYaw( Rand.Float( 360 ) );
				fish.Species = GetRandomFish( RarityLevel );
				fish.Size = Rand.Float( 1f, 3f );
				fish.Scale = fish.Size;

				Fishes.Add( fish );

			}


		}

		[Event.Tick.Client]
		public void ClientTick()
		{

			DebugOverlay.Circle( Position, Rotation.FromPitch( 90 ), Range, new Color( 1, 0, 0, 0.7f ) );

		}

	}

}
