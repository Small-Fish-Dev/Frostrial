using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	public partial class BootlegVMix
	{
		public class BootlegVMixItem
		{
			public Sound Sound;
			public float CurrentVolume;
			public float TargetVolume;
			public float DeltaUp;
			public float DeltaDown;

			public static BootlegVMixItem FromFile( string file, float deltaUp = 1, float deltaDown = 1 )
			{
				var bvmixi = new BootlegVMixItem()
				{
					Sound = Sound.FromScreen( file ),
					CurrentVolume = 0,
					TargetVolume = 0,
					DeltaUp = deltaUp,
					DeltaDown = deltaDown
				};
				bvmixi.Sound.SetVolume( bvmixi.CurrentVolume );

				return bvmixi;
			}
		}

		public List<BootlegVMixItem> Sounds { get; set; } = new();
		public bool Mute { get; set; } = false;

		public BootlegVMix()
		{
		}

		public void Tick()
		{
			foreach ( var sound in Sounds )
			{
				sound.CurrentVolume = MathX.LerpTo( sound.CurrentVolume, sound.TargetVolume, (sound.TargetVolume > sound.CurrentVolume ? sound.DeltaUp : sound.DeltaDown) * Time.Delta );
				sound.Sound.SetVolume( Mute ? 0 : sound.CurrentVolume );
			}
		}
	}
}
