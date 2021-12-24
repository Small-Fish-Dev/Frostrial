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

				return Game.CalcValue( Species, Size, Variant, 5 );

			}
		}

		public float Value
		{
			get
			{

				return Game.CalcValue( Species, Size, Variant, 3 );

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
			SetMaterialGroup( Game.FishAlt[Species] );
			Scale = 2 * (Size / Game.FishSizes[Species]);

		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			if ( Variant )
			{

				Particles.Create( "particles/rare_fish_particles.vpcf", this );

			}

		}

	}

}
