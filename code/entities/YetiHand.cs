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

		}

		[Event.Tick.Client]
		public void ClientTick()
		{

			DebugOverlay.Circle( Position - Vector3.Up * 3.7f, Rotation.FromPitch( 90 ), 13f, new Color( 1, 0.6f, 0.6f, 0.3f ), true, Time.Delta);

		}

	}

}
