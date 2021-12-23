using Sandbox;
using System;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public Entity CurrentHole { get; set; } = PhysicsWorld.WorldBody.Entity;

	}
	public partial class Hole : AnimEntity
	{

		[Net] public bool Bobber { get; set; } = false;

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/icehole_low.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

		}

		[Event.Tick]
		public void Tick()
		{

			SetBodyGroup( "bobber", Bobber ? 1 : 0 );

		}

	}

}
