using Sandbox;
using System;

namespace Frostrial
{
	public partial class Hole : AnimEntity
	{

		[Net] public float CreationTime { get; set; }

		[Event.Tick.Server]
		public void OnTick()
		{

			float speed = 0.5f;
			float progress = Math.Clamp( Time.Now - CreationTime, 0, speed ) / speed;

			DebugOverlay.Circle( Position, Rotation.FromPitch( 90 ), 10f * progress, new Color( 0.02f, 0.04f, 0.15f, progress ) ); ;

		}

	}

}
