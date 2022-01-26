using Sandbox;
using System;

namespace Frostrial
{

	[Library( "frostrial_lake", Description = "This is the lake it's a separate entity so I am able to change the renderAlpha without compromising the particles" )]
	[Hammer.EditorModel( "maps/lake.vmdl" )]
	public partial class Lake : AnimEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "maps/lake.vmdl" );

			RenderColor = RenderColor.WithAlpha( 0.5f );

		}

	}

}
