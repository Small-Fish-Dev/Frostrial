using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_yeti_scalp", Description = "Incredibly rare!!!" )]
	[Hammer.EditorModel( "models/treasures/scalp.vmdl" )]
	public partial class YetiScalp : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/treasures/scalp.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			GlowState = GlowStates.On;
			GlowColor = new Color( 0.3f, 0.07f, 0.07f );

		}

	}

}
