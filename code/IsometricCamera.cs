using Sandbox;

namespace Frostrial
{
	public class IsometricCamera : Camera
	{
		public float AngleChangeDelay => 0.5f;

		TimeSince LastAngleChange = 0;
		Angles TargetAngles = new Angles( 45, 45, 0 );
		Angles CurrentAngles = Angles.Zero;

		public IsometricCamera()
		{
			Ortho = true;
			OrthoSize = 1f;

			CurrentAngles = TargetAngles;
		}

		public override void Update()
		{
			var player = Local.Client?.Pawn;
			if ( player == null )
				return;

			ZNear = -102400;
			Position = player.Position + new Vector3(0, 0, 72 / 2);
			CurrentAngles = CurrentAngles.WithYaw(MathX.LerpTo(CurrentAngles.yaw, TargetAngles.yaw, 5f * Time.Delta));
			Rotation = CurrentAngles.ToRotation();
			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( LastAngleChange >= AngleChangeDelay )
			{
				float rotDir = (input.Down( InputButton.Menu ) ? -1 : 0) + (input.Down( InputButton.Use ) ? 1 : 0);

				TargetAngles = TargetAngles.WithYaw( TargetAngles.yaw + rotDir * 90 );

				LastAngleChange = 0;

				Event.Run( "frostrial.camrot" ); // TODO: play a sound when the camera was rotated
			}

			// add the view move, clamp pitch
			input.ViewAngles += input.AnalogLook;
			input.ViewAngles.roll = 0;

			// Just copy input as is
			input.InputDirection = input.AnalogMove;
		}
	}
}
