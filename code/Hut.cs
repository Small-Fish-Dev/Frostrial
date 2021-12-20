using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_hut", Description = "main base" )]
	[Hammer.EditorModel( "models/randommodels/campfire.vmdl" )]
	public partial class Hut : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/campfire.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

			Game current = Game.Current as Game;
			current.Hut = this;

		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			Particles.Create( "particles/fire_embers.vpcf", Position );

		}

		[Event.Tick.Server]
		public void OnTick()
		{



		}

	}

}
