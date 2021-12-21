using Sandbox;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float DrillingSpeed { get; set; } = 5f; // Seconds before completing, better drill = faster
		[Net] public bool Drilling { get; set; } = false;
		[Net] RealTimeUntil drillingCompletion { get; set; } = 0f;
		[Net] RealTimeSince lastAttempt { get; set; } = 0f;
		[Net] float attemptCooldown { get; set; } = 0.5f;

		Vector3 holePosition = new();

		public void HandleDrilling()
		{

			if ( IsClient ) return;

			if ( Input.Pressed( InputButton.Attack1 ) )
			{

				if ( lastAttempt >= attemptCooldown )
				{

					holePosition = Position + Input.Rotation.Forward.WithZ( 0f ).Normal * 60f;

					if ( Controller.Velocity.LengthSquared < 1 ) // Don't allow the player to make holes while sliding
					{

						if( Game.IsOnIce( holePosition ) )
						{

							if( !Game.IsNearEntity( holePosition, 5f ) )
							{

								Drilling = true;
								HandleDrillingEffects( true, holePosition );
								drillingCompletion = DrillingSpeed; 
								BlockMovement = true;

							}
							else
							{

								Hint( "I'm not drilling there.", 2f );

							}

						}
						else
						{

							Hint( "I can't drill on here!", 2f );

						}

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
