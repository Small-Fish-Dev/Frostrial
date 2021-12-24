using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_fish_au_poopoo_caca", Description = "What the hell is this" )]
	[Hammer.EditorModel( "models/fishes/perch/perch.vmdl" )]
	public partial class FishAuPoopooCaca : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "models/fishes/perch/perch.vmdl" );
			SetMaterialGroup( "poo" );
			Scale = 2;
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			GlowState = GlowStates.On;
			GlowColor = new Color( 0.3f, 0.07f, 0.07f );


		}


	}

}
