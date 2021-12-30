using Sandbox;
using System;
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

		[Net] public static Hut HutEntity { get; set; }
		[Net] public static string CurrentTitle { get; set; } = "Wake up";
		[Net] public static string CurrentSubtitle { get; set; } = "";
		public static Dictionary<string, string> InteractionsText = new();

		public static Dictionary<string, string> FishNames = new();
		public static Dictionary<string, string> FishAlt = new();
		public static Dictionary<string, float[]> FishVariety = new();
		public static Dictionary<string, float> FishSizes = new();
		public static Dictionary<string, float> FishRarity = new();
		public static Dictionary<string, bool> FishUnlock = new();
		public static Dictionary<string, string[]> FishPictures = new()
		{
			{ "goldfish", new string[2] { "", "" } },
			{ "minnow", new string[2] { "ui/fishes/minnow.png", "ui/fishes/minnow_gold.png" } },
			{ "herring", new string[2] { "ui/fishes/herring.png", "ui/fishes/herring_european.png" } },
			{ "perch", new string[2] { "ui/fishes/perch.png", "ui/fishes/perch_rainbow.png" } },
			{ "pike", new string[2] { "ui/fishes/pike.png", "ui/fishes/pike_chain.png" } },
			{ "salmon", new string[2] { "ui/fishes/salmon.png", "ui/fishes/salmon_coho.png" } },
			{ "trout", new string[2] { "ui/fishes/trout.png", "ui/fishes/trout_gold.png" } },
			{ "fishaupoopoocaca", new string[2] { "ui/fishes/perch_poo.png", "" } }
		};

		public static Dictionary<string, float> Prices = new();

		private AmbientWindVMix vMix = new();
		private MusicPlayer musicPlayer = new();

		public Game()
		{

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
				InteractionsText.Add( "FishAuPoopooCaca", "Interact to pick up the Fish Au Poopoo Caca." );
			}

			FishNames.Add( "goldfish", "models/fishes/fishshadow.vmdl" );
			FishNames.Add( "minnow", "models/fishes/minnow/minnow.vmdl" );
			FishNames.Add( "herring", "models/fishes/herring/herring.vmdl" );
			FishNames.Add( "perch", "models/fishes/perch/perch.vmdl" );
			FishNames.Add( "pike", "models/fishes/pike/pike.vmdl" );
			FishNames.Add( "salmon", "models/fishes/salmon/salmon.vmdl" );
			FishNames.Add( "trout", "models/fishes/trout/trout.vmdl" );

			FishVariety.Add( "goldfish", new float[3] { 0, 0, 0 } );
			FishVariety.Add( "minnow", new float[3] { 55, 5, 1 } );
			FishVariety.Add( "herring", new float[3] { 30, 20, 4 } );
			FishVariety.Add( "perch", new float[3] { 10, 40, 15 } );
			FishVariety.Add( "pike", new float[3] { 4, 30, 25 } );
			FishVariety.Add( "salmon", new float[3] { 1, 4, 35 } );
			FishVariety.Add( "trout", new float[3] { 0, 1, 20 } );

			FishSizes.Add( "goldfish", 0.01f );
			FishSizes.Add( "minnow", 0.12f );
			FishSizes.Add( "herring", 0.24f );
			FishSizes.Add( "perch", 0.40f );
			FishSizes.Add( "pike", 0.60f );
			FishSizes.Add( "salmon", 0.80f );
			FishSizes.Add( "trout", 1.00f );

			FishRarity.Add( "goldfish", 0.01f );
			FishRarity.Add( "minnow", 0.1f );
			FishRarity.Add( "herring", 0.2f );
			FishRarity.Add( "perch", 0.35f );
			FishRarity.Add( "pike", 0.5f );
			FishRarity.Add( "salmon", 0.75f );
			FishRarity.Add( "trout", 1.0f );

			FishUnlock.Add( "goldfish", false );
			FishUnlock.Add( "minnow", false );
			FishUnlock.Add( "herring", false );
			FishUnlock.Add( "perch", false );
			FishUnlock.Add( "pike", false );
			FishUnlock.Add( "salmon", false );
			FishUnlock.Add( "trout", false );

			FishAlt.Add( "goldfish", "" );
			FishAlt.Add( "minnow", "Gold" );
			FishAlt.Add( "herring", "european" );
			FishAlt.Add( "perch", "rainbow" );
			FishAlt.Add( "pike", "chain" );
			FishAlt.Add( "salmon", "coho" );
			FishAlt.Add( "trout", "gold" );

			Prices.Add( "bait", 1.49f );
			Prices.Add( "campfire", 3.99f );
			Prices.Add( "coat", 49.99f );
			Prices.Add( "drill", 399.99f );
			Prices.Add( "rod", 749.99f );
			Prices.Add( "plane", 6999.99f );

		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new Player();
			client.Pawn = player;

			MusicInitRPC( To.Single( client ) );

			player.Respawn();
		}

		[ClientRpc]
		protected void MusicInitRPC()
		{
			musicPlayer.Initialize();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( IsServer )
				return;

			var player = cl.Pawn as Player;

			vMix.Update( 1 - player.Warmth, IsOnIce( player.Position ), player.Position.Distance( HutEntity?.Position ?? Vector3.Zero ) < 200f );
			vMix.Tick();

			musicPlayer.Tick();
		}

		public static bool IsOnIce( Vector3 position )
		{

			var trace = Trace.Ray( position + Vector3.Up * 10f, position + Vector3.Down * 2f )
			.WorldOnly()
			.Run();

			return trace.Surface.Name == "ice";

		}

		public static bool IsOnDirt( Vector3 position )
		{

			var trace = Trace.Ray( position + Vector3.Up * 10f, position + Vector3.Down * 2f )
			.WorldOnly()
			.Run();

			return trace.Surface.Name == "dirt";

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

		public static float CalcValue( string species, float size, bool variant, float variantWeight )
		{

			float sizeRatio = size / Game.FishSizes[species] + 0.5f;
			float variantBonus = variant ? variantWeight : 1;
			float rarity = Game.FishRarity[species] + 0.5f;

			return (float)Math.Pow( sizeRatio * rarity, 6 ) * variantBonus;

		}

		// TODO: can we use a template for these?

		public static Entity NearestDescribableEntity( Vector3 position, float range = 30f )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );
			Entity currentEntity = PhysicsWorld.WorldBody.Entity; // Technically correct to return the world
			float currentDistance = range;

			foreach ( Entity ent in entityList )
			{

				if ( !(ent is IDescription) ) continue;

				float entDistance = ent.Position.Distance( position );

				if ( entDistance < currentDistance )
				{

					currentEntity = ent;
					currentDistance = entDistance;

				}

			}

			return currentEntity;

		}

		public static Entity NearestInteractiveEntity( Vector3 position, float range = 30f )
		{

			var entityList = Physics.GetEntitiesInSphere( position, range );
			Entity currentEntity = PhysicsWorld.WorldBody.Entity; // Technically correct to return the world
			float currentDistance = range;

			foreach ( Entity ent in entityList )
			{

				if ( !(ent is IUse ue) || !ue.IsUsable( Local.Pawn ) ) continue;

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

		public static bool IsInside( Vector3 position, Vector3 min, Vector3 max )
		{

			if ( position.x < min.x || position.x > max.x ) { return false; }
			if ( position.y < min.y || position.y > max.y ) { return false; }
			if ( position.z < min.z || position.z > max.z ) { return false; }

			return true;

		}

	}

}
