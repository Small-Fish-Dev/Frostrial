using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Drilling { get; set; } = false;
		[Net] float drillingCompletion { get; set; } = 0f;
		[Net] float lastAttempt { get; set; } = 0f;
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

					if ( trace.Surface.Name != "glass" )
					{

						return false;

					}

				}

				return true;

			}

		}

		public void HandleDrilling()
		{

			if ( IsClient ) return;

			Vector3 holePosition = Position + Input.Rotation.Forward.WithZ( 0f ).Normal * 40f;

			if ( Input.Pressed( InputButton.Attack1 ) )
			{

				if ( lastAttempt <= Time.Now )
				{

					if ( IsOnIce )
					{

						Drilling = true;
						HandleDrillingEffects( true, holePosition );
						drillingCompletion = Time.Now + 2f; // TODO: Better drilling speed depends on drill

					}
					else
					{ 
						
						// Show error messag here
					
					}

					lastAttempt = Time.Now + attemptCooldown;

				}

			}

			if ( Input.Released( InputButton.Attack1 ) )
			{

				if ( Drilling )
				{

					Drilling = false;
					HandleDrillingEffects( false, holePosition );
					drillingCompletion = 0f;

				}

			}

			if ( Input.Down( InputButton.Attack1 ) )
			{

				if ( Drilling )
				{

					if ( drillingCompletion <= Time.Now )
					{

						Drilling = false;
						HandleDrillingEffects( false, holePosition );

						var hole = new Hole();
						hole.Position = holePosition;
						hole.CreationTime = Time.Now;

					}

				}

			}

		}
		Particles drillingParticle { get; set; }
		Sound drillingSound { get; set; }

		[ClientRpc]
		public void HandleDrillingEffects( bool fxState, Vector3 fxPosition )
		{

			if( fxState )
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
