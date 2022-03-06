using Sandbox;
using Sandbox.Component;
using System;

namespace Frostrial
{
	partial class Player : Sandbox.Player, IUse, IDescription
	{
		[Net, Local] public Rotation MovementDirection { get; set; } = new Angles( 0, 90, 0 ).ToRotation();
		[Net, Local] public bool BlockMovement { get; set; } = false;

		public Vector3 MouseWorldPosition
		{
			get
			{

				IsometricCamera camera = CameraMode as IsometricCamera;
				var realRay = Input.Cursor;
				realRay.Origin = Input.Cursor.Origin - camera.Rotation.Forward * 5000;

				if ( _MouseWorldPositionDirty )
				{
					var tr = Trace.Ray( realRay, 7000.0f )
					.WorldOnly()
					.Run();
					_MouseWorldPosition = tr.EndPosition;
					_MouseWorldPositionDirty = false;
				}

#if false
				DebugOverlay.Line( Position, tr.EndPos );
				DebugOverlay.Sphere( tr.EndPos, 10f, tr.EndPos.Distance( Position ) <= InteractionMaxDistance ? Color.Cyan : Color.Red, false );
#endif

				return _MouseWorldPosition;
			}
		}
		private Vector3 _MouseWorldPosition;
		private bool _MouseWorldPositionDirty = true;

		public Entity MouseEntityPoint
		{
			get
			{

				IsometricCamera camera = CameraMode as IsometricCamera;
				var realRay = Input.Cursor;
				realRay.Origin = Input.Cursor.Origin - camera.Rotation.Forward * 5000;

				if ( _MouseEntityPointDirty )
				{
					var tr = Trace.Ray( realRay, 7000.0f )
					.EntitiesOnly()
					.WithTag( "use" )
					.Run();
					_MouseEntityPoint = tr.Entity;
					_MouseEntityPointDirty = false;
				}

				return _MouseEntityPoint;

			}
		}
		private Entity _MouseEntityPoint;
		private bool _MouseEntityPointDirty = true;

		public string Description => "Interact with yourself to use items.";
		public bool IsUsingController = Input.UsingController;
		public Vector2 VirtualCursor { get; internal set; }

		protected VoiceLinePlayer vlp;

		[ServerCmd]
		public static void ChangeMovementDirection( float yaw )
		{
			if ( ConsoleSystem.Caller.Pawn is not Player pawn )
				return;

			pawn.MovementDirection = Rotation.FromYaw( yaw );
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();

			vlp = new VoiceLinePlayer( this );

			Event.Run( "frostrial.player.inputdevice", IsUsingController );
		}

		public override void Respawn()
		{
			SetModel( "models/jorma/jorma.vmdl" );

			Controller = new TopdownPlayerController();

			Animator = new TopdownPlayerAnimator();

			CameraMode = new IsometricCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			SpawnedSince = 0f;
			BlockMovement = false;

			CaughtFish = new();

			Delay( 4 );

			BasicClothes();

			Tags.Add( "use" );

			base.Respawn();

			//

			AddMoney( Rand.Float( 4f, 12f ) );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			_MouseWorldPositionDirty = true;
			_MouseEntityPointDirty = true;

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );

			HandleDrilling();
			HandleWarmth();
			HandleHUD();
			HandleItems();
			HandleFishing();
			HandleShopping();

			// Dirty CameraMode fix
			if ( IsServer )
			{

				var cam = CameraMode as IsometricCamera;

				cam.Rotation = Rotation.Slerp( cam.Rotation, cam.TargetRotation, 5f * Time.Delta );

			}

		}

		public override void OnKilled()
		{

			// Do nothing because we won't die but override it because of kill command

		}

		public override void BuildInput( InputBuilder input )
		{
			if ( input.UsingController != IsUsingController )
			{
				IsUsingController = input.UsingController;
				Event.Run( "frostrial.player.inputdevice", IsUsingController );
			}

			if ( IsUsingController && CameraMode is IsometricCamera )
			{
				VirtualCursor = input.GetAnalog( InputAnalog.Look );
				var angles = CameraMode.Rotation.Angles();
				input.Cursor = new(
					CameraMode.Position + (CameraMode.Rotation.Up * VirtualCursor.y * MathF.Abs( MathF.Sin( angles.pitch.DegreeToRadian() ) ) - CameraMode.Rotation.Left * VirtualCursor.x) * InteractionMaxDistance,
					angles.Direction
					);
			}

			base.BuildInput( input );
		}

		public bool OnUse( Entity user )
		{
			if ( user == this )
			{
				ItemsOpen = true;
				BlockMovement = true;
				Velocity = Vector3.Zero;

				Say( VoiceLine.LetsSee );
			}
			else
				Say( VoiceLine.Idiot );

			return true;
		}

		public bool IsUsable( Entity user ) => true;
	}

}
