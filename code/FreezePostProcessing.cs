using Sandbox;
using System;


namespace Sandbox
{

	public class FreezePostProcessEffect : BasePostProcess
	{
		public static Material PostProcessMaterial;

		public class FreezeSettings
		{
			internal BasePostProcess parent;

			internal bool _enabled;


			public float FreezeStrength
			{
				set
				{
					parent.Set( "freeze.strength", in value );
				}
			}

		}
		public FreezeSettings Freeze = new FreezeSettings();

		public FreezePostProcessEffect()
		{
			PostProcessMaterial = Material.Load( "materials/postprocessing/postprocess_freeze.vmat" );
			Freeze.parent = this;
		}

		public override void Render()
		{
			if ( PostProcessMaterial == null ) return;

			Sandbox.Render.Material = PostProcessMaterial;
			RenderScreenQuad();
		}
	}
}
