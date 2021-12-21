using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;

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

		[Net] public Hut HutEntity { get; set; }
		public static Dictionary<string, string> InteractionsText = new();

		public Game()
		{

			if ( IsServer )
			{

				new FrostrialHUD();

			}
			else
			{

				InteractionsText.Add( "Hut", "Interact with the hut to buy items and upgrades." );
				InteractionsText.Add( "Hole", "Interact with this hole to fish." );
				InteractionsText.Add( "Player", "Interact with yourself to use items." );

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
				if ( ent is not Hole && ent is not Campfire && ent is not Hut) continue;

				return true;

			}

			return false;

		}

		public static Entity NearestEntity( Vector3 position, float range = 30f )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );
			Entity currentEntity = PhysicsWorld.WorldBody.Entity; // Technically correct to return the world
			float currentDistance = range;

			foreach ( Entity ent in entityList )
			{

				if ( !ent.GetType().IsSubclassOf( typeof( ModelEntity ) ) ) continue; // I don't want ModelEntity, but since it's a parent class I have to do this to exclude it

				float entDistance = ent.Position.Distance( position );

				if ( entDistance < currentDistance )
				{

					currentEntity = ent;
					currentDistance = entDistance;

				}

			}

			return currentEntity;

		}

		public static float CampfireDistance( Vector3 position, float range = 100f )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );
			float currentDistance = range;

			foreach ( Entity ent in entityList )
			{

				if ( ent is Campfire )
				{

					float entDistance = ent.Position.Distance( position );

					if ( entDistance < currentDistance )
					{

						currentDistance = entDistance;

					}

				}

			}

			return currentDistance;

		}

	}

}
