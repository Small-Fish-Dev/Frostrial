using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class DeadFish : ModelEntity
	{

		[Net] public string Species { get; set; } = "goldfish";
		[Net] public float Size { get; set; } = 0.1f; // Meters
		[Net] public bool Variant { get; set; } = false;
		[Net] public float Rarity { get; set; } = 0;
		public float TotalRarity
		{
			get
			{

				return (float)Math.Pow( 12, ( Size / Game.FishSizes[Species] + 0.3f ) * (Rarity + 0.5f) ) * (Variant ? 5 : 1);

			}
		}

		public float Value
		{
			get
			{

				return (float)Math.Round( Math.Pow( 12, ( Size / Game.FishSizes[Species] + 0.3f ) * (Rarity + 0.5f) ) * (Variant ? 3 : 1), 2 );

			}
		}

		public DeadFish()
		{ }

		public DeadFish( string species = "goldfish", float size = 0.1f, bool variant = false, float rarity = 0 )
		{

			Species = species;
			Size = size;
			Variant = variant;
			Rarity = rarity;

			MoveType = MoveType.Physics;
			UsePhysicsCollision = true;

			SetInteractsAs( CollisionLayer.Debris );
			SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			SetInteractsExclude( CollisionLayer.Player );

			SetModel( Game.FishNames[Species] );
			Scale = 2 * ( Size / Game.FishSizes[Species] );


		}

	}

}
