using Sandbox;
using System;


namespace Sandbox
{

	public class FreezePostProcessEffect : BasePostProcess
	{
		private Material PostProcessMaterial;

		public float FreezeStrength
		{
			set => Set("freeze.strength", value);
		}

		public FreezePostProcessEffect()
		{
			PostProcessMaterial = Material.Load( "materials/postprocessing/postprocess_freeze.vmat" );
		}

		public override void Render()
		{
			if ( PostProcessMaterial == null ) return;

			Sandbox.Render.Material = PostProcessMaterial;
			RenderScreenQuad();
		}
	}
}
