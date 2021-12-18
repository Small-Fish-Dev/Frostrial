using Sandbox;

namespace Frostrial
{
	public partial class Hole : AnimEntity
	{

		[Event.Tick.Server]
		public void OnTick()
		{

			DebugOverlay.Circle( Position, Rotation.FromPitch( 90 ), 10f, Color.Black ); ;

		}

	}

}
