using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_hut", Description = "main base" )]
	[Hammer.EditorModel( "models/randommodels/cabin_walls.vmdl" )]
	public partial class Hut : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/cabin_walls.vmdl" );
			SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -100, -170, 0 ), new Vector3( 100, 150, 12 ) );

			Rotation = Rotation.FromYaw( -90 );

			Game current = Game.Current as Game;
			current.Hut = this;

			new ModelEntity( "models/randommodels/campfire.vmdl" ).Position = Position + Vector3.Up * 12;
			ModelEntity floor = new ModelEntity( "models/randommodels/cabin_floor.vmdl" );
			floor.Position = Position;
			floor.Rotation = Rotation.FromYaw( -90 );

		}

		public override void ClientSpawn()
		{

			base.ClientSpawn();

			Particles.Create( "particles/fire_embers.vpcf", Position + Vector3.Up * 12 );

		}

		[Event.Tick.Client]
		public void OnTick()
		{

			var startFadeDistance = 300f;
			var endFadeDistance = 150f;
			var player = Local.Pawn as Player;
			var distance = player.Position.Distance( this.Position );

			RenderColor = RenderColor.WithAlpha( 1 - (startFadeDistance - distance ) / endFadeDistance );

		}

	}

}
