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

		}

		[Event.Tick.Client]
		public void ClientTick()
		{

			DebugOverlay.Circle( Position - Vector3.Up * 0.1f, Rotation.FromPitch( 90 ), 13f, new Color( 1, 0.6f, 0.6f, 0.3f ), true, Time.Delta );

		}

	}

}
