using Sandbox;

namespace Frostrial
{
	partial class Player : Sandbox.Player, IUse, IDescription
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
		[Net]
		public Entity MouseEntityPoint
		{
			get
			{

				var tr = Trace.Ray( Input.Cursor, 5000.0f )
				.EntitiesOnly()
				.WithTag( "use" )
				.Run();

				return tr.Entity;

			}
		}

		public string Description => "Interact with yourself to use items.";

		[ServerCmd]
		public static void ChangeMovementDirection( float yaw )
		{
			var pawn = ConsoleSystem.Caller.Pawn as Player;
			pawn.MovementDirection = Rotation.FromYaw( yaw );
		}

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

			Hint( "", 4, true );

			BasicClothes();

			Tags.Add( "use" );

			base.Respawn();

			//

			AddMoney( Rand.Float( 4f, 12f ) );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );

			HandleDrilling();
			HandleWarmth();
			HandleHUD();
			HandleItems();
			HandleFishing();
			HandleShopping();
		}

		public override void OnKilled()
		{

			// Do nothing because we won't die but override it because of kill command

		}

		public bool OnUse( Entity user )
		{
			if ( user == this )
			{
				ItemsOpen = true;
				BlockMovement = true;

				Hint( "Let's see...", 1.2f );
			}
			else
				Hint( "Idiot.", 1f );

			return true;
		}

		public bool IsUsable( Entity user ) => true;
	}

}
