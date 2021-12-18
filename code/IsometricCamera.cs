using Sandbox;

namespace Frostrial
{
	public class IsometricCamera : Camera
	{
		public float AngleChangeDelay => 0.2f;

		TimeSince LastAngleChange = 0;
		Angles TargetAngles = new Angles( 30, 45, 0 );
		Rotation TargetRotation = new();
		bool hasNewAngle = false;

		public IsometricCamera()
		{
			Ortho = true;
			OrthoSize = 0.7f;

			TargetRotation = TargetAngles.ToRotation();
			Rotation = TargetRotation;

			LastAngleChange = AngleChangeDelay;
		}

		[ClientCmd("camera_pitch")]
		public static void ChangeCameraPitch(float newPitch)
		{
			var cam = (Local.Pawn as Player).Camera as IsometricCamera;
			if ( cam == null )
				return;

			cam.TargetAngles = cam.TargetAngles.WithPitch( newPitch.Clamp( 15.0f, 45.0f ) );
			cam.TargetRotation = cam.TargetAngles.ToRotation();
		}

		public override void Update()
		{
			var player = Local.Pawn as Player;
			if ( player == null )
				return;

			Rotation = Rotation.Slerp( Rotation, TargetRotation, 5f * Time.Delta );
			Position = player.Position + Rotation.Backward * 1500 + Rotation.FromYaw( Rotation.Yaw() ).Forward * 10; // move it back a little bit
			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( LastAngleChange >= AngleChangeDelay )
			{
				float rotDir = (input.Down( InputButton.Menu ) ? -1 : 0) + (input.Down( InputButton.Use ) ? 1 : 0);

				if ( rotDir != 0 )
				{
					TargetAngles = TargetAngles.WithYaw( MathX.NormalizeDegrees( TargetAngles.yaw + rotDir * 90 ) );
					TargetRotation = TargetAngles.ToRotation();

					LastAngleChange = 0;
					hasNewAngle = true;

					// TODO: play a sound when the camera was rotated
				}
			}

			// add the view move, clamp pitch
			input.ViewAngles += input.AnalogLook;
			input.ViewAngles.roll = 0;

			// Just copy input as is
			input.InputDirection = input.AnalogMove;

			if ( hasNewAngle && input.InputDirection.IsNearZeroLength ) // if the player have stopped moving
			{
				Player.ChangeMovementDirection( TargetAngles.yaw ); // change the angle
				hasNewAngle = false;
			}
		}
	}
}
