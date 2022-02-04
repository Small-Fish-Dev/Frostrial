using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_fish_au_poopoo_caca", Description = "What the hell is this" )]
	[Hammer.EditorModel( "models/fishes/perch/perch.vmdl" )]
	public partial class FishAuPoopooCaca : AnimEntity, IUse, IDescription
	{
		bool canUse = true;

		public string Description => "Interact to pick up the Fish Au Poopoo Caca.";

		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			if ( user is not Player p )
				return false;

			canUse = false; // probably dodging some kind of race state or something

			p.AddMoney( 19.84f );

			Player.FishUI( To.Single( user ), "fishaupoopoocaca", true );
			p.Say( VoiceLine.LeFishe );

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

			Tags.Add( "use" );
		}


	}

}
