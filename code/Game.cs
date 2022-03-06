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
		[Net] public Hut HutEntity { get; set; }
		[Net] public FrostrialHUD HudEntity { get; set; }
		[Net] public string CurrentTitle { get; set; } = "Wake up";
		[Net] public string CurrentSubtitle { get; set; } = "";

		public static Dictionary<string, bool> FishUnlock = new();

		public static Dictionary<string, float> Prices = new();

		private readonly AmbientWindVMix vMix = new();
		private readonly MusicPlayer musicPlayer = new();

		public static Game Instance { get; internal set; }

		public Game()
		{
			Instance = this;

			if ( IsServer )
			{
				HudEntity = new FrostrialHUD();

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

			if ( IsClient )
			{
				Player.CheckVoiceLines();
			}

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

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			Event.Run( "frostrial.player.left", cl );
			
			base.ClientDisconnect( cl, reason );
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

			if ( Local.Pawn is not Player player )
				return;

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

		public static bool IsOnSnow( Vector3 position )
		{

			var trace = Trace.Ray( position + Vector3.Up * 10f, position + Vector3.Down * 2f )
			.WorldOnly()
			.Run();

			return trace.Surface.Name == "snow";

		}

		public static bool IsNearEntity( Vector3 position, float range )
		{

			var entityList = Entity.FindInSphere( position, range );

			foreach ( var ent in entityList )
			{

				if ( ent is Player ) continue;
				if ( ent is not Hole && ent is not Campfire && ent is not Hut ) continue;

				return true;

			}

			return false;

		}

		public static float CalcValue( string species, float size, bool variant, float variantWeight )
		{

			var sizeRatio = size / FishAsset.All[species].Size + 0.5f;
			var variantBonus = variant ? variantWeight : 1;
			var rarity = FishAsset.All[species].Rarity + 0.5f;

			return (float)Math.Pow( sizeRatio * rarity, 6 ) * variantBonus;

		}

		// TODO: can we use a template for these?

		public static Entity NearestDescribableEntity( Vector3 position, float range = 30f )
		{

			var entityList = Entity.FindInSphere( position, range );
			Entity currentEntity = null;
			var currentDistance = range;

			foreach ( var ent in entityList )
			{

				if ( ent is not IDescription ) continue;

				var entDistance = ent.Position.Distance( position );

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

			var entityList = Entity.FindInSphere( position, range );
			Entity currentEntity =  Sandbox.Internal.GlobalGameNamespace.Map.Entity; // Technically correct to return the world
			var currentDistance = range;

			foreach ( var ent in entityList )
			{

				if ( ent is not IUse ue || !ue.IsUsable( Local.Pawn ) ) continue;

				var entDistance = ent.Position.Distance( position );

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

			var entityList = Entity.FindInSphere( position, range );
			Entity currentEntity = Sandbox.Internal.GlobalGameNamespace.Map.Entity;
			var currentDistance = range;

			foreach ( var ent in entityList )
			{

				if ( ent is Player )
				{

					var entDistance = ent.Position.Distance( position );

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

			var entityList = Entity.FindInSphere( position, range );
			var currentDistance = range;

			foreach ( var ent in entityList )
			{

				if ( ent is Campfire )
				{

					var entDistance = ent.Position.Distance( position );

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
