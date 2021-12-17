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

			ZNear = -102400; // just a big number to make the world not cut off
			Position = player.Position + new Vector3( 0, 0, 72 / 2 ); // + half a player's height
			CurrentAngles = CurrentAngles.WithYaw( MathX.LerpTo( CurrentAngles.yaw, TargetAngles.yaw, 5f * Time.Delta ) );
			Rotation = CurrentAngles.ToRotation();
			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( LastAngleChange >= AngleChangeDelay )
			{
				float rotDir = (input.Down( InputButton.Menu ) ? 1 : 0) + (input.Down( InputButton.Use ) ? -1 : 0);

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
				Log.Info( $"Sending new angle {TargetAngles}..." );
				Player.ChangeMovementDirection( new Vector3(TargetAngles.pitch, TargetAngles.yaw, TargetAngles.roll) ); // change the angle
				hasNewAngle = false;
			}
		}
	}
}
