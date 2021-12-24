using Sandbox;

namespace Frostrial
{
	partial class Player : Sandbox.Player
	{
		[Net, Local] public Rotation MovementDirection { get; set; } = new Angles( 0, 90, 0 ).ToRotation();
		[Net, Local] public bool BlockMovement { get; set; } = false;
		[Net]
		public Vector3 MouseWorldPosition
		{
			get
			{

				var tr = Trace.Ray( Input.Cursor, 5000.0f )
				.WorldOnly()
				.Run();

				return tr.EndPos;

			}
		}

		[ServerCmd]
		public static void ChangeMovementDirection( float yaw )
		{
			var pawn = ConsoleSystem.Caller.Pawn as Player;
			pawn.MovementDirection = Rotation.FromYaw( yaw );
		}

		private AmbientWindVMix vMix = new();
		private MusicPlayer musicPlayer = new();

		public override void Respawn()
		{
			SetModel( "models/jorma/jorma.vmdl" );

			Controller = new TopdownPlayerController();

			Animator = new TopdownPlayerAnimator();

			Camera = new IsometricCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			SpawnedSince = 0f;
			BlockMovement = false;

			CaughtFish = new();

			Money = Rand.Float( 0f, 3f );

			Hint( "", 4, true );

			BasicClothes();

			ClientSide();

			base.Respawn();
		}

		[ClientRpc]
		protected void ClientSide()
		{
			// TODO: Dear Ubre, please call this function right after the intro cutscene ends. Thanks! - Ivan
			musicPlayer.Initialize();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			SimulateActiveChild( cl, ActiveChild );

			HandleDrilling();
			HandleWarmth();
			HandleHUD();
			HandleInteractions();
			HandleItems();
			HandleFishing();
			HandleShopping();

			if ( IsClient )
			{
				vMix.Update( 1 - Warmth, Game.IsOnIce( Position ), Position.Distance( Game.HutEntity?.Position ?? Vector3.Zero ) < 200f );
				vMix.Tick();

				musicPlayer.Tick();
			}
		}

		public override void OnKilled()
		{

			// Do nothing because we won't die but override it because of kill command

		}

	}

}
