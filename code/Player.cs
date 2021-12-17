using Sandbox;

namespace Frostrial
{
	partial class Player : Sandbox.Player
	{
		[Net, Local] public Rotation MovementDirection { get; set; } = new Angles( 0, 45, 0 ).ToRotation();

		[ServerCmd]
		public static void ChangeMovementDirection(Vector3 a) // FIXME: replace it with Angles as soon as passing Angles to the ServerCmd is fixed
		{
			var pawn = ConsoleSystem.Caller.Pawn as Player;
			var angle = new Angles( a.x, a.y, a.z );
			Log.Info( $"New movement direction for the player {pawn.Name}: {angle.yaw}" );
			pawn.MovementDirection = angle.ToRotation();
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new TopdownPlayerController();

			Animator = new TopdownPlayerAnimator();

			Camera = new IsometricCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// Called to simulate fishing rod/drill/etc
			//
			SimulateActiveChild( cl, ActiveChild );
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
