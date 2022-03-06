using Sandbox;
using System;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public Entity CurrentHole { get; set; } = Sandbox.Internal.GlobalGameNamespace.Map.Entity;

	}
	public partial class Hole : AnimEntity, IUse, IDescription
	{

		[Net] public bool Bobber { get; set; } = false;

		public string Description => "Interact with this hole to fish.";

		public bool IsUsable( Entity user ) => true; // TODO: block the usability if a hole is already occupied by another player

		public bool OnUse( Entity user )
		{
			if ( user is not Player p )
				return false;

			p.Fishing = true;
			p.BlockMovement = true;
			Velocity = Vector3.Zero;

			p.CurrentHole = this;

			Bobber = true;

			p.Say( VoiceLine.Fishing );
			Player.Play3D( "rod_woosh", p );
			Player.Play3D( "rod_throw", this );

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/randommodels/icehole_low.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Rotation = Rotation.FromYaw( Rand.Float( 360f ) );

			Tags.Add( "use" );
		}

		[Event.Tick]
		public void Tick()
		{

			SetBodyGroup( "bobber", Bobber ? 1 : 0 );

		}

	}

}
