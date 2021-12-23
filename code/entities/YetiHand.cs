using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_yeti_hand", Description = "Incredibly rare!!!" )]
	[Hammer.EditorModel( "models/treasures/yeti_hand.vmdl" )]
	public partial class YetiHand : AnimEntity
	{

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
