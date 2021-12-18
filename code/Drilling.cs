using Sandbox;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Drilling { get; set; } = false;
		[Net] RealTimeUntil drillingCompletion { get; set; } = 0f;
		[Net] RealTimeSince lastAttempt { get; set; } = 0f;
		[Net] float attemptCooldown { get; set; } = 0.5f;

		[Net]
		public bool IsOnIce
		{

			get
			{

				for ( var i = 0; i < 9; i++ )
				{

					Log.Info( i );

					float checkDistance = 80f;
					Vector3 checkPos = Rotation.FromYaw( i * 45f ).Forward * checkDistance;

					var trace = Trace.Ray( Position + checkPos + Vector3.Up * 16f, Position + checkPos + Vector3.Down * 16f )
					.WorldOnly()
					.Run();

					if ( trace.Surface.Name != "ice" )
					{

						return false;

					}

				}

				return true;

			}

		}

		Vector3 holePosition = new();

		public void HandleDrilling()
		{

			if ( IsClient ) return;

			if ( Input.Pressed( InputButton.Attack1 ) )
			{

				if ( lastAttempt >= attemptCooldown )
				{

					if ( Controller.Velocity.LengthSquared < 1 && IsOnIce ) // Don't allow the player to make holes while sliding
					{
						holePosition = Position + Input.Rotation.Forward.WithZ( 0f ).Normal * 60f;
						Drilling = true;
						HandleDrillingEffects( true, holePosition );
						drillingCompletion = 2f; // TODO: Better drilling speed depends on drill
						BlockMovement = true;
					}
					else
					{

						// Show error messag here

					}

					lastAttempt = 0;

				}

			}

			if ( Input.Released( InputButton.Attack1 ) )
			{

				if ( Drilling )
				{

					Drilling = false;
					HandleDrillingEffects( false, holePosition );
					drillingCompletion = 0f;
					BlockMovement = false;

				}

			}

			if ( Input.Down( InputButton.Attack1 ) )
			{

				if ( Drilling )
				{

					if ( drillingCompletion <= 0 )
					{

						Drilling = false;
						HandleDrillingEffects( false, holePosition );

						var hole = new Hole();
						hole.Position = holePosition;
						hole.CreationTime = Time.Now;
						BlockMovement = false;

					}

				}

			}

		}
		Particles drillingParticle { get; set; }
		Sound drillingSound { get; set; }

		[ClientRpc]
		public void HandleDrillingEffects( bool fxState, Vector3 fxPosition )
		{

			if ( fxState )
			{

				Sound.FromWorld( "", fxPosition ); // TODO Play the drilling
				drillingParticle = Particles.Create( "particles/drilling_particle.vpcf", fxPosition );

			}
			else
			{

				if ( drillingParticle != null )
				{

					drillingParticle.Destroy();

				}

			}

		}

	}

}
