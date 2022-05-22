using Sandbox;
using Sandbox.Component;
using System;
using SandboxEditor;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frostrial
{

	[Library( "frostrial_fish_au_poopoo_caca", Description = "What the hell is this" )]
	[HammerEntity]
	[Model( Model = "models/fishes/perch/perch.vmdl" )]
	public partial class FishAuPoopooCaca : ModelEntity, IUse, IDescription
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
			PhysicsEnabled = true;

			var glow = Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.Color = new Color( 0.3f, 0.07f, 0.07f );

			Tags.Add( "use" );
		}


	}

}
