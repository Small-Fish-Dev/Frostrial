using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_fish_au_poopoo_caca", Description = "What the hell is this" )]
	[Hammer.EditorModel( "models/fishes/perch/perch.vmdl" )]
	public partial class FishAuPoopooCaca : AnimEntity, IUse
	{
		bool canUse = true;
		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			canUse = false; // probably dodging some kind of race state or something

			var p = user as Player;

			p.AddMoney( 19.84f );

			Player.FishPoopoo( To.Single( Client ) );
			p.Hint( "I can always count on French Cuisine", 2.5f );

			Delete();

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/fishes/perch/perch.vmdl" );
			SetMaterialGroup( "poo" );
			Scale = 2;
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			GlowState = GlowStates.On;
			GlowColor = new Color( 0.3f, 0.07f, 0.07f );


		}


	}

}
