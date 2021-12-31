using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_yeti_scalp", Description = "Incredibly rare!!!" )]
	[Hammer.EditorModel( "models/treasures/scalp.vmdl" )]
	public partial class YetiScalp : AnimEntity, IUse, IDescription
	{
		bool canUse = true;

		public string Description => "Interact to pick up the Yeti Scalp.";

		public bool IsUsable( Entity user ) => canUse;

		public bool OnUse( Entity user )
		{
			canUse = false; // probably dodging some kind of race state or something

			var p = user as Player;

			p.AddMoney( 2500f );

			var yeti = new Yeti()
			{
				Position = new Vector3( 3275f, 3511.5f, 8f ),
				Victim = p

			};

			Player.Play3D( "yeti_roar", yeti );

			p.Hint( "It's the Finnish Yeti! I must head back to the cabin!", 4f, true );

			Delete();

			return true;
		}

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/treasures/scalp.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			GlowState = GlowStates.On;
			GlowColor = new Color( 0.3f, 0.07f, 0.07f );

		}

	}

}
