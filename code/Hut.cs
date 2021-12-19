using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_hut", Description = "main base" )]
	[Hammer.EditorModel( "models/tools/auger_icedrill.vmdl" )]
	public partial class Hut : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/tools/auger_icedrill.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

			Game current = Game.Current as Game;
			current.Hut = this;

		}

		[Event.Tick.Server]
		public void OnTick()
		{


		}

	}

}
