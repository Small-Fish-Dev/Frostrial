using Sandbox;
using System;

namespace Frostrial
{
	public partial class Campfire : AnimEntity
	{

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

			Particles.Create( "particles/fire_embers.vpcf", Position );

		}

	}

}
