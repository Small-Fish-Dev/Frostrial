using Sandbox;
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
		public static Game Instance { get; internal set; }

		[Net] public Hut HutEntity { get; set; }
		public static Dictionary<string, string> InteractionsText = new();

		public static Dictionary<string, string> FishNames = new();
		public static Dictionary<string, float[]> FishVariety = new();
		public static Dictionary<string, float> FishSizes = new();
		public static Dictionary<string, float> FishRarity = new();
		public static Dictionary<string, bool> FishUnlock = new();

		public static Dictionary<string, float> Prices = new();

		public Game()
		{
			Instance = this;

			if ( IsServer )
			{

				new FrostrialHUD();

				//I could do this with an fgd, but time's running out!
				var zone1 = new FishSpawner();
				zone1.Position = new Vector3( -400, 150, -9 );
				zone1.Range = 900f;
				zone1.RarityLevel = 0;

				var zone12 = new FishSpawner();
				zone12.Position = new Vector3( 920, -1260, -9 );
				zone12.Range = 570f;
				zone12.RarityLevel = 0;

				var zone2 = new FishSpawner();
				zone2.Position = new Vector3( 2500, -1600, -9 );
				zone2.Range = 700f;
				zone2.RarityLevel = 1;

				var zone3 = new FishSpawner();
				zone3.Position = new Vector3( 1200, 280, -9 );
				zone3.Range = 700f;
				zone3.RarityLevel = 1;

				var zone4 = new FishSpawner();
				zone4.Position = new Vector3( 2500, 1000, -9 );
				zone4.Range = 700f;
				zone4.RarityLevel = 2;

				var zone5 = new FishSpawner();
				zone5.Position = new Vector3( 500, 2250, -9 );
				zone5.Range = 700f;
				zone5.RarityLevel = 2;


			}
			else
			{

				InteractionsText.Add( "Hut", "Interact with the hut to buy items and upgrades." );
				InteractionsText.Add( "Hole", "Interact with this hole to fish." );
				InteractionsText.Add( "Player", "Interact with yourself to use items." );
				InteractionsText.Add( "YetiHand", "Interact to pick up the Yeti Hand." );
				InteractionsText.Add( "YetiScalp", "Interact to pick up the Yeti Scalp." );
				InteractionsText.Add( "DeadFish", "Interact to pick up the Dead Fish." );

			}

			FishNames.Add( "goldfish", "models/fishes/fishshadow.vmdl" );
			FishNames.Add( "minnow", "models/fishes/minnow/minnow.vmdl" );
			FishNames.Add( "herring", "models/fishes/herring/herring.vmdl" );
			FishNames.Add( "perch", "models/fishes/perch/perch.vmdl" );
			FishNames.Add( "pike", "models/fishes/pike/pike.vmdl" );
			FishNames.Add( "salmon", "models/fishes/salmon/salmon.vmdl" );
			FishNames.Add( "trout", "models/fishes/trout/trout.vmdl" );

			FishVariety.Add( "goldfish", new float[3]	{ 0, 0, 0 } );
			FishVariety.Add( "minnow", new float[3]		{ 55, 5, 1 } );
			FishVariety.Add( "herring", new float[3]	{ 30, 20, 4 } );
			FishVariety.Add( "perch", new float[3]		{ 10, 40, 15 } );
			FishVariety.Add( "pike", new float[3]		{ 4, 30, 25 } );
			FishVariety.Add( "salmon", new float[3]		{ 1, 4, 35 } );
			FishVariety.Add( "trout", new float[3]		{ 0, 1, 20 } );

			FishSizes.Add( "goldfish",	0.01f );
			FishSizes.Add( "minnow",	0.12f );
			FishSizes.Add( "herring",	0.24f );
			FishSizes.Add( "perch",		0.40f );
			FishSizes.Add( "pike",		0.60f );
			FishSizes.Add( "salmon",	0.80f );
			FishSizes.Add( "trout",		1.00f );

			FishRarity.Add( "goldfish", 0.01f );
			FishRarity.Add( "minnow",	0.1f );
			FishRarity.Add( "herring",	0.2f );
			FishRarity.Add( "perch",	0.35f );
			FishRarity.Add( "pike",		0.5f );
			FishRarity.Add( "salmon",	0.75f );
			FishRarity.Add( "trout",	1.0f );

			FishUnlock.Add( "goldfish", false );
			FishUnlock.Add( "minnow", false );
			FishUnlock.Add( "herring", false );
			FishUnlock.Add( "perch", false );
			FishUnlock.Add( "pike", false );
			FishUnlock.Add( "salmon", false );
			FishUnlock.Add( "trout", false );

			Prices.Add( "bait", 1.49f );
			Prices.Add( "campfire", 3.99f );
			Prices.Add( "coat", 49.99f );
			Prices.Add( "drill", 299.99f );
			Prices.Add( "rod", 649.99f );
			Prices.Add( "plane", 3999.99f );

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

			var trace = Trace.Ray( position + Vector3.Up * 10f, position + Vector3.Down * 2f )
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
				if ( ent is not Hole && ent is not Campfire && ent is not Hut ) continue;

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
				if ( ent is Fish || ent is FishSpawner ) continue;

				float entDistance = ent.Position.Distance( position );

				if ( entDistance < currentDistance )
				{

					currentEntity = ent;
					currentDistance = entDistance;

				}

			}

			return currentEntity;

		}

		public static Entity NearestPlayer( Vector3 position, float range = 30f )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );
			Entity currentEntity = PhysicsWorld.WorldBody.Entity;
			float currentDistance = range;

			foreach ( Entity ent in entityList )
			{

				if ( ent is Player )
				{

					float entDistance = ent.Position.Distance( position );

					if ( entDistance < currentDistance )
					{

						currentEntity = ent;
						currentDistance = entDistance;

					}

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
