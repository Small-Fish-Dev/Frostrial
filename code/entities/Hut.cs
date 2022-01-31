using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_hut", Description = "main base" )]
	[Hammer.EditorModel( "models/randommodels/cabin_walls.vmdl" )]
	public partial class Hut : AnimEntity, IUse, IDescription
	{

		PointLightEntity light { get; set; }
		public string Description => "Interact with the hut to buy items and upgrades.";
		[Net] ModelEntity leftWall { set; get; }
		[Net] ModelEntity rightWall { set; get; }
		[Net] ModelEntity backWall { set; get; }
		[Net] ModelEntity frontWall { set; get; }
		[Net] ModelEntity crate { set; get; }

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/cabin_roof.vmdl" );
			SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -15, -15, 0 ), new Vector3( 20, 15, 80 ) );

			Game.Instance.HutEntity = this;

			var fire = new ModelEntity( "models/randommodels/cabin_chimney_base.vmdl" );
			fire.Position = Position + Vector3.Up * 12;
			fire.EnableShadowCasting = false;

			Sound.FromEntity( "campfire", fire );

			crate = new ModelEntity( "models/randommodels/crate.vmdl" );
			crate.SetMaterialGroup( 1 );
			crate.Position = Position + Vector3.Up * 12;
			crate.GlowState = GlowStates.On;
			crate.GlowColor = new Color( 0.8f, 0.2f, 0.2f );

			Tags.Add( "use" );

			leftWall = new ModelEntity( "models/randommodels/cabin_wall_left.vmdl" );
			leftWall.Position = Position;
			leftWall.Rotation = Rotation;

			rightWall = new ModelEntity( "models/randommodels/cabin_wall_right.vmdl" );
			rightWall.Position = Position;
			rightWall.Rotation = Rotation;

			backWall = new ModelEntity( "models/randommodels/cabin_wall_back.vmdl" );
			backWall.Position = Position;
			backWall.Rotation = Rotation;

			frontWall = new ModelEntity( "models/randommodels/cabin_wall_front.vmdl" );
			frontWall.Position = Position;
			frontWall.Rotation = Rotation;

		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			Particles.Create( "particles/fire_embers.vpcf", Position + (Vector3.Up * 15) + (Vector3.Left * 95) );

			light = new PointLightEntity();
			light.Position = Position + (Vector3.Up * 25) + (Vector3.Left * 95);
			light.Color = Color.Orange;
			light.DynamicShadows = true;

			Game.Instance.HutEntity = this;

		}

		[Event.Tick.Client]
		public void OnTick()
		{

			var startFadeDistance = 500f;
			var endFadeDistance = 150f;
			var player = Local.Pawn as Player;
			var playerPos = player.Position.WithZ( 0 );
			var hutPos = this.Position.WithZ( 0 );
			var distance = playerPos.Distance( hutPos );

			RenderColor = RenderColor.WithAlpha( 1 - (startFadeDistance - distance ) / endFadeDistance );

			light.SetLightBrightness( 20 + (float)Math.Cos( (float)Time.Now * 25 ) * 2 * ( 1 + Time.Now % 1 ) ); // Acceptable flickering

			var cam = player.Camera as IsometricCamera;
			var dir = cam.Rotation.Forward.WithZ( 0 ).Normal;

			var distanceX = Math.Clamp( ( playerPos.x - hutPos.x ) / 100, -1, 1 );
			var distanceY = Math.Clamp( ( playerPos.y - hutPos.y ) / 150, -1, 1 );

			frontWall.RenderColor = frontWall.RenderColor.WithAlpha( (1 - dir.y + RenderColor.a ) * ( 1 + distanceY ) ) ;
			backWall.RenderColor = backWall.RenderColor.WithAlpha( (1 + dir.y + RenderColor.a ) * ( 1 - distanceY ) );
			leftWall.RenderColor = leftWall.RenderColor.WithAlpha( (1 - dir.x + RenderColor.a ) * ( 1 + distanceX ) );
			rightWall.RenderColor = rightWall.RenderColor.WithAlpha( (1 + dir.x + RenderColor.a ) * ( 1 - distanceX ) );

		}

		[Event( "frostrial.crate_used" )]
		public void CrateUsed()
		{

			crate.GlowState = GlowStates.Off;

		}

		public bool OnUse( Entity user )
		{
			if ( user is not Player p )
				return false;

			p.ShopOpen = true;
			p.BlockMovement = true;
			p.Say( VoiceLine.ImAlmostThere );

			Event.Run( "frostrial.crate_used" );

			return true;
		}

		public bool IsUsable( Entity user ) => true;
	}

}
