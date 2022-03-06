using Sandbox;
using Sandbox.Component;
using System;

namespace Frostrial
{

	[Library( "frostrial_yeti_hand", Description = "Incredibly rare!!!" )]
	[Hammer.EditorModel( "models/treasures/yeti_hand.vmdl" )]
	public partial class YetiHand : AnimEntity, IUse, IDescription
	{
		bool canUse = true;

		public string Description => "Interact to pick up the Yeti Hand.";

		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			if ( user is not Player p )
				return false;

			canUse = false; // probably dodging some kind of race state or something

			p.AddMoney( 800f );
			p.Say( VoiceLine.OldYetiHand );
			Delete();

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/treasures/yeti_hand.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			var glow = Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.Color = new Color( 0.3f, 0.07f, 0.07f );

			Tags.Add( "use" );
		}


	}

}
