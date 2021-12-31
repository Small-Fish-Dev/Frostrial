using Sandbox;

namespace Frostrial
{

	public partial class Game : Sandbox.Game
	{
#if false
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

		[ServerCmd( "setmoney" )]
		public static void SetMoney( float amount )
		{

			var player = ConsoleSystem.Caller.Pawn as Player;
			player.AddMoney( amount - player.Money );

		}

		[ServerCmd( "spawnyeti" )]
		public static void SpawnYeti()
		{

			var player = ConsoleSystem.Caller.Pawn as Player;

			var spawnPos = player.Position + player.Rotation.Forward.WithZ( 0f ).Normal * 500f;

			new Yeti()
			{

				Position = spawnPos,
				Victim = player as Entity

			};

		}
#endif

	}

}

