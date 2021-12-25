using Sandbox;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float DrillingSpeed { get; set; } = 5f; // Seconds before completing, Upgraded drill = 1/5th
		[Net] public bool Drilling { get; set; } = false;
		[Net] RealTimeUntil drillingCompletion { get; set; } = 0f;
		[Net] RealTimeSince lastAttempt { get; set; } = 0f;
		[Net] float attemptCooldown { get; set; } = 0.5f;

		Vector3 holePosition = new();

		public void HandleDrilling()
		{

			if ( !Fishing )
			{

				if( Drilling )
				{

					SetClothing( "tool", UpgradedDrill ? "models/tools/auger_icedrill.vmdl" : "models/tools/hand_icedrill.vmdl" );

					SetAnimBool( "handdrill", !UpgradedDrill );
					SetAnimBool( "autodrill", UpgradedDrill );

				}
				else
				{

					SetAnimBool( "handdrill", false );
					SetAnimBool( "autodrill", false );

				}

			}

			if ( IsClient ) return;

			if ( Input.Pressed( InputButton.Attack1 ) && !PlacingCampfire )
			{

				if ( lastAttempt >= attemptCooldown )
				{

					holePosition = Position + Input.Rotation.Forward.WithZ( 0f ).Normal * 20f;

					if ( Controller.Velocity.LengthSquared < 10 ) // Don't allow the player to make holes while sliding
					{

						if( Game.IsOnIce( holePosition ) || Game.IsOnDirt( holePosition ) )
						{

							if( !Game.IsNearEntity( holePosition, 5f ) )
							{

								Drilling = true;
								HandleDrillingEffects( true, holePosition );
								drillingCompletion = DrillingSpeed * ( UpgradedDrill ? 0.2f : 1f ); 
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

						if ( Game.IsOnIce( holePosition ) )
						{

							var hole = new Hole();
							hole.Position = holePosition;

						}
						else if ( Game.IsOnDirt( holePosition ) )
						{

							Baits++;
							Hint( "I found a worm, good for bait I guess.", 2.5f );
							WormsParticle();

						}
						

						BlockMovement = false;

					}

				}

			}

		}
		Particles drillingParticle { get; set; }
		Sound drillingSound { get; set; }

		[ClientRpc]
		public void WormsParticle()
		{

			Particles.Create( "particles/small_worms_particle.vpcf", Position + Vector3.Up );

		}

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
