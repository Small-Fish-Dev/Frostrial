using Sandbox;
using System;

namespace Frostrial
{
	public partial class Hole : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/icehole_low.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

		}

	}

}
