using Sandbox;
using System;

namespace Frostrial
{

	public partial class Campfire : ModelEntity
	{

		public Particles ParticleEffect { get; set; }
		public PointLightEntity LightEffect { get; set; }
		float durability { get; set; } = 30f; // How long before it breaks
		[Net] RealTimeUntil timeOfDeath { get; set; }

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/campfire.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );
			PhysicsEnabled = true;

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

			timeOfDeath = durability;

			EnableShadowCasting = false;

			Sound.FromEntity( "campfire", this );

		}

		public override void ClientSpawn()
		{

			ParticleEffect = Particles.Create( "particles/fire_embers.vpcf", Position + Vector3.Up * 10 );

			base.ClientSpawn();
			LightEffect = new PointLightEntity();
			LightEffect.Position = Position + Vector3.Up * 16;
			LightEffect.Color = Color.Orange;
			LightEffect.DynamicShadows = true;

		}

		[Event.Tick]
		public void OnTick()
		{

			if( IsClient )
			{
				
				if( timeOfDeath > 0.1f ) // I am sick of getting random "Error calling event 'tick'" messages. Hopefully this fixes it
				{

					LightEffect.SetLightBrightness( 2f + (float)Math.Cos( (float)Time.Now * 25 ) * 0.2f * (1 + Time.Now % 1) );

				}

			}
			else
			{

				if ( timeOfDeath <= 0 )
				{

					DeleteEffects( this );
					Delete();

				}

			}

		}

		[ClientRpc]
		public static void DeleteEffects( Campfire campfire )
		{

			campfire.ParticleEffect.Destroy();
			campfire.LightEffect.Delete();

		}

	}

}
