using Sandbox;

namespace Frostrial
{
	public partial class AmbientWindVMix : BootlegVMix
	{
		private readonly int S_NORMAL = 0;
		private readonly int S_STRONG = 1;
		private readonly int S_NORMAL_MUFFLED = 2;
		private readonly int S_STRONG_MUFFLED = 3;

		public AmbientWindVMix() : base()
		{
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.normal", 1, 0.1f ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.strong", 0.2f, 1 ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.normal.muffled", 1, 0.1f ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.strong.muffled", 1, 0.1f ) );
		}

		public void Update( float windStrength, bool isOnIce, bool isInShelter )
		{
			Sounds[S_NORMAL].TargetVolume = MathX.Remap( windStrength, 0, 1, 0.3f, 1 );
			Sounds[S_STRONG].TargetVolume = isOnIce ? MathX.Remap( windStrength, 0, 1, 0.5f, 1 ) : 0;

			if ( isInShelter )
			{
				Sounds[S_NORMAL_MUFFLED].TargetVolume = Sounds[S_NORMAL].TargetVolume;
				Sounds[S_STRONG_MUFFLED].TargetVolume = Sounds[S_STRONG].TargetVolume;

				Sounds[S_NORMAL].TargetVolume = Sounds[S_STRONG].TargetVolume = 0;
			}
			else
			{
				Sounds[S_NORMAL_MUFFLED].TargetVolume = Sounds[S_STRONG_MUFFLED].TargetVolume = 0;
			}
#if false
			DebugOverlay.ScreenText( $"{windStrength} {isOnIce} {isInShelter} :\n{Sounds[S_NORMAL].TargetVolume} {Sounds[S_STRONG].TargetVolume} {Sounds[S_NORMAL_MUFFLED].TargetVolume} {Sounds[S_STRONG_MUFFLED].TargetVolume}\n{Sounds[S_NORMAL].CurrentVolume} {Sounds[S_STRONG].CurrentVolume} {Sounds[S_NORMAL_MUFFLED].CurrentVolume} {Sounds[S_STRONG_MUFFLED].CurrentVolume}" );
#endif
		}
	}
}
