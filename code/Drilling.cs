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
		[Net] public bool IsOnIce
		{

			get
			{

				var trace = Trace.Ray( Position, Position + Vector3.Down * 16f )
				.WorldOnly()
				.Run();

				return trace.Surface.Name == "glass";

			}

		}

		public void HandleDrilling()
		{

			if ( IsClient ) return;

			if ( Input.Pressed( InputButton.Attack1 ) )
			{

				if ( lastAttempt <= Time.Now )
				{

					if ( IsOnIce )
					{

						Drilling = true;
						drillingCompletion = Time.Now + 0.2f; // TODO: Better drilling speed depends on drill

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

						var hole = new Hole();
						hole.Position = Position;

					}

				}

			}

		}

	}

}
