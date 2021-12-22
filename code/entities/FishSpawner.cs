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

		[Event.Tick.Server]
		public void OnTick()
		{


		}

		[Event.Tick.Client]
		public void CLientTick()
		{

			DebugOverlay.Circle( Position, Rotation.FromPitch( 90 ), Range, Color.Red ); ;

		}

	}

}
