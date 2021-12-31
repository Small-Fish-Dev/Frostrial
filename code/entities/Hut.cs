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

		ModelEntity crate;

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/cabin_walls.vmdl" );
			SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -100, -170, 0 ), new Vector3( 100, 150, 12 ) );

			Rotation = Rotation.FromYaw( -90 );

			Game.HutEntity = this;

			var fire = new ModelEntity( "models/randommodels/cabin_chimney_base.vmdl" );
			fire.Position = Position + Vector3.Up * 12;
			fire.EnableShadowCasting = false;

			Sound.FromEntity( "campfire", fire );

			crate = new ModelEntity( "models/randommodels/crate.vmdl" );
			crate.SetMaterialGroup( 1 );
			crate.Position = Position + Vector3.Up * 12;
			crate.GlowState = GlowStates.On;
			crate.GlowColor = new Color( 0.8f, 0.2f, 0.2f );

		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			Particles.Create( "particles/fire_embers.vpcf", Position + (Vector3.Up * 15) + (Vector3.Left * 95) );

			light = new PointLightEntity();
			light.Position = Position + (Vector3.Up * 25) + (Vector3.Left * 95);
			light.Color = Color.Orange;
			light.DynamicShadows = true;

			Game.HutEntity = this;

		}

		[Event.Tick.Client]
		public void OnTick()
		{

			var startFadeDistance = 300f;
			var endFadeDistance = 150f;
			var player = Local.Pawn as Player;
			var distance = player.Position.Distance( this.Position );

			RenderColor = RenderColor.WithAlpha( 1 - (startFadeDistance - distance ) / endFadeDistance );

			light.SetLightBrightness( 20 + (float)Math.Cos( (float)Time.Now * 25 ) * 2 * ( 1 + Time.Now % 1 ) ); // Acceptable flickering

		}

		[Event( "frostrial.crate_used" )]
		public void CrateUsed()
		{

			crate.GlowState = GlowStates.Off;

		}

		public bool OnUse( Entity user )
		{
			var p = user as Player;

			p.ShopOpen = true;
			p.BlockMovement = true;
			p.Hint( "I'm almost there", 2f );

			Event.Run( "frostrial.crate_used" );

			return true;
		}

		public bool IsUsable( Entity user ) => true;
	}

}
