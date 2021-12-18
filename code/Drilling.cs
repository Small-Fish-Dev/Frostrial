using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float DrillingCompletion { get; set; }
		private bool _drilling;
		/*[Net]
		public bool Drilling
		{
			get { return _drilling; }

			set
			{

				bool canDrill = HandleDrilling( value ); // TODO: Fix this mess before pushing please

				if ( _drilling != value )
				{

					DrillingCompletion = value ? Time.Now + 3f : 0f; // REPLACE WITH DRILLING SPEED

				}

				_drilling = HandleDrilling( value );

			}

		}*/
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


		/*public bool HandleDrilling( bool shouldDrill )
		{

			if()


			return false;

		}*/

	}

}
