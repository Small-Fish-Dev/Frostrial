using Sandbox;

namespace Frostrial
{

	public partial class DeadFish : ModelEntity, IUse, IDescription
	{
		bool canUse = true;
		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			canUse = false; // probably dodging some kind of race state or something

			var p = user as Player;
			switch ( Rarity )
			{

				case <= 0.3f:
					p.Hint( "This isn't going to cut it", 1.7f );
					break;

				case <= 0.6f:
					p.Hint( "There we go!", 1f );
					break;

				case > 0.6f:
					p.Hint( "That's a big one!", 1.2f );
					break;

			}

			Player.Play3D( "fish_flop", this );

			p.AddMoney( Value );

			StopBuzzing();
			Delete();

			return true;
		}

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

		protected Sound BuzzingSound { get; set; }

		public string Description => "Interact to pick up the Dead Fish.";

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
			SetMaterialGroup( Variant ? Game.FishAlt[Species] : "default" );
			Scale = 2 * (Size / Game.FishSizes[Species]);

			Tags.Add( "use" );
		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			BuzzingSound = Sound.FromEntity( "buzzing", this );

			Particles.Create( "particles/basic_fish_flies_particles.vpcf", this );

			if ( Variant )
			{

				Particles.Create( "particles/rare_fish_particles.vpcf", this );

			}

		}

		[ClientRpc]
		public void StopBuzzing()
		{
			BuzzingSound.Stop();
		}

	}

}
