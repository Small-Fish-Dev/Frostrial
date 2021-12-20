using Sandbox;
using System.Linq;

namespace Frostrial
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class Game : Sandbox.Game
	{

		[Net] public Hut Hut { get; set; }

		public Game()
		{
			if ( IsServer )
			{
				new FrostrialHUD();
			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new Player();
			client.Pawn = player;

			player.Respawn();
		}

		public static bool IsOnIce( Vector3 position )
		{ 
			
			var trace = Trace.Ray( position + Vector3.Up * 2f, position + Vector3.Down * 2f )
			.WorldOnly()
			.Run();

			return trace.Surface.Name == "ice";

		}

		public static bool IsNearEntity( Vector3 position, float range )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );

			foreach ( Entity ent in entityList )
			{

				if ( ent is Player ) continue;
				if ( ent is not Hole && ent is not Campfire) continue;

				return true;

			}

			return false;

		}

	}

}
