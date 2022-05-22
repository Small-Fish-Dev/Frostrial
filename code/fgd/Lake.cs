using Sandbox;
using System;
using SandboxEditor;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frostrial
{

	[Library( "frostrial_lake", Description = "This is the lake it's a separate entity so I am able to change the renderAlpha without compromising the particles" )]
	[HammerEntity]
	[Model( Model = "maps/lake.vmdl" )]
	public partial class Lake : ModelEntity
	{

		public override void Spawn()
		{

			base.Spawn();

			SetModel( "maps/lake.vmdl" );

			RenderColor = RenderColor.WithAlpha( 0.5f );

		}

	}

}
