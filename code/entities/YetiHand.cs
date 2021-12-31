using Sandbox;
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
			canUse = false; // probably dodging some kind of race state or something

			var p = user as Player;
			p.AddMoney( 800f );
			p.Hint( "This Yeti Hand is old, lucky", 2f, true );
			Delete();

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/treasures/yeti_hand.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			GlowState = GlowStates.On;
			GlowColor = new Color( 0.3f, 0.07f, 0.07f );


		}


	}

}
