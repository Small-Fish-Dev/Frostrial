using Sandbox;

namespace Frostrial
{
	public class IsometricCamera : Camera
	{
		public float AngleChangeDelay => 0.2f;

		TimeSince LastAngleChange = 0;
		Angles TargetAngles = new Angles( 45, 45, 0 );
		Angles CurrentAngles = Angles.Zero;
		bool hasNewAngle = false;

		public IsometricCamera()
		{
			Ortho = true;
			OrthoSize = 1f;

			CurrentAngles = TargetAngles;
		}

		public override void Update()
		{
			var player = Local.Pawn as Player;
			if ( player == null )
				return;

			CurrentAngles = CurrentAngles.WithYaw( MathX.LerpTo( CurrentAngles.yaw, TargetAngles.yaw, 5f * Time.Delta ) );
			Position = player.Position + CurrentAngles.ToRotation().Backward * 1024; // move it back a little bit
			Rotation = CurrentAngles.ToRotation();
			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( LastAngleChange >= AngleChangeDelay )
			{
				float rotDir = (input.Down( InputButton.Menu ) ? -1 : 0) + (input.Down( InputButton.Use ) ? 1 : 0);

				if ( rotDir != 0 )
				{
					TargetAngles = TargetAngles.WithYaw( TargetAngles.yaw + rotDir * 90 );

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
				Player.ChangeMovementDirection( new Vector3( TargetAngles.pitch, TargetAngles.yaw, TargetAngles.roll ) ); // change the angle
				hasNewAngle = false;
			}
		}
	}
}
