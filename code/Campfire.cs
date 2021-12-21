using Sandbox;
using System;

namespace Frostrial
{
	public partial class Campfire : AnimEntity
	{

		PointLightEntity light { get; set; }

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/campfire.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

		}
		public override void ClientSpawn()
		{

			base.ClientSpawn();

			Particles.Create( "particles/fire_embers.vpcf", Position + Vector3.Up * 10 );

			EnableShadowCasting = false;

			light = new PointLightEntity();
			light.Position = Position + Vector3.Up * 16;
			light.Color = Color.Orange;
			light.DynamicShadows = true;

		}

		[Event.Tick.Client]
		public void OnTick()
		{

			var player = Local.Pawn as Player;
			var distance = player.Position.Distance( this.Position );

			light.SetLightBrightness( 2f + (float)Math.Cos( (float)Time.Now * 25 ) * 0.2f * (1 + Time.Now % 1) );

		}

	}

}
