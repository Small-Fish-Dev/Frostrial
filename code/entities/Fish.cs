using Sandbox;
using System;

namespace Frostrial
{

	public partial class Fish : AnimEntity
	{

		[Net] public string Species { get; set; } = "goldfish";
		[Net] public float Size { get; set; } = 0.1f; // Meters
		[Net] public bool Variant { get; set; } = false;
		[Net] public FishSpawner Spawner { get; set; }

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/fishes/fishshadow.vmdl" );

			EnableShadowCasting = false;

		}

		[Event.Tick.Server]
		public void OnTick()
		{

		}

	}

}
