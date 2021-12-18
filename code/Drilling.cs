using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Drilling { get; set; } = false;
		[Net] public float DrillingCompletion { get; set; }
		[Net] public bool IsOnIce
		{
			get => Trace.Ray( Position, Position + Vector3.Down * 16f )
				.WorldOnly()
				.Run()
				.Surface
				.Name == "glass";
		}

	}

}
