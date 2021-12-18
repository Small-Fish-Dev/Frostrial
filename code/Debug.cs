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

		[ServerCmd( "checkground" )]
		public static void CheckGround()
		{

			var player = ConsoleSystem.Caller.Pawn as Player;
			Log.Info( player.IsOnIce );

		}

	}

}

