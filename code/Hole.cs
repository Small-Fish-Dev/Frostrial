using Sandbox;
using System;

namespace Frostrial
{
	public partial class Hole : AnimEntity
	{

		[Net] public float CreationTime { get; set; }

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/icehole_low.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

		}

	}

}
