using Sandbox;

namespace Frostrial
{

	public partial class Game : Sandbox.Game
	{

		[ServerCmd( "setclothes" )]
		public static void SetClothes( string clothingSlot, string modelPath )
		{

			var player = ConsoleSystem.Caller.Pawn as Player;
			player.SetClothing( clothingSlot, modelPath );

		}

		[ServerCmd( "sufferscold" )]
		public static void DisableCold( bool var )
		{

			var player = ConsoleSystem.Caller.Pawn as Player;
			player.SuffersCold = var;

		}

	}

}

