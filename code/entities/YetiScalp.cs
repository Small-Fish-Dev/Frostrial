using Sandbox;
using Sandbox.Component;
using System;
using SandboxEditor;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frostrial
{

	[Library( "frostrial_yeti_scalp", Description = "Incredibly rare!!!" )]
	[HammerEntity]
	[Model( Model = "models/treasures/scalp.vmdl" )]
	public partial class YetiScalp : ModelEntity, IUse, IDescription
	{
		bool canUse = true;

		public string Description => "Interact to pick up the Yeti Scalp.";

		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			if ( user is not Player p )
				return false;

			canUse = false; // probably dodging some kind of race state or something

			p.AddMoney( 2500f );

			var yeti = new Yeti()
			{
				Position = new Vector3( 3275f, 3511.5f, 8f ),
				Victim = p

			};

			Player.Play3D( "yeti_roar", yeti );

			p.Say( VoiceLine.FinnishYeti );

			Delete();

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/treasures/scalp.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );
			PhysicsEnabled = true;

			var glow = Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.Color = new Color( 0.3f, 0.07f, 0.07f );

			Tags.Add( "use" );
		}

	}

}
