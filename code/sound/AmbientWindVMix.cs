using Sandbox;

namespace Frostrial
{
	public partial class AmbientWindVMix : BootlegVMix
	{
		[ConVar.Replicated( "debug_bootlegvmix" )]
		public static bool DebugBootlegVMix { get; set; } = false;

		private readonly int S_NORMAL = 0;
		private readonly int S_STRONG = 1;
		private readonly int S_NORMAL_MUFFLED = 2;
		private readonly int S_STRONG_MUFFLED = 3;

		private bool wasInShelter = true;

		public AmbientWindVMix() : base()
		{
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.normal", 1, 0.1f ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.strong", 0.1f, 0.1f ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.normal.muffled", 1, 0.1f ) );
			Sounds.Add( BootlegVMixItem.FromFile( "ambient.wind.strong.muffled", 1, 0.1f ) );
		}

		public void Update( float windStrength, bool isOnIce, bool isInShelter )
		{
			if ( wasInShelter != isInShelter )
			{
				(Sounds[S_NORMAL_MUFFLED].CurrentVolume, Sounds[S_NORMAL].CurrentVolume) = (Sounds[S_NORMAL].CurrentVolume, Sounds[S_NORMAL_MUFFLED].CurrentVolume);
				(Sounds[S_STRONG_MUFFLED].CurrentVolume, Sounds[S_STRONG].CurrentVolume) = (Sounds[S_STRONG].CurrentVolume, Sounds[S_STRONG_MUFFLED].CurrentVolume);

				Sounds[!isInShelter ? S_NORMAL_MUFFLED : S_NORMAL].TargetVolume = Sounds[!isInShelter ? S_STRONG_MUFFLED : S_STRONG].TargetVolume = 0;

				wasInShelter = isInShelter;
			}

			Sounds[isInShelter ? S_NORMAL_MUFFLED : S_NORMAL].TargetVolume = MathX.Remap( windStrength, 0, 1, 0.3f, 1 );
			Sounds[isInShelter ? S_STRONG_MUFFLED : S_STRONG].TargetVolume = isOnIce || windStrength > 0.5 ? MathX.Remap( windStrength, 0, 1, 0.3f, 1 ) : 0;

			if ( DebugBootlegVMix )
				DebugOverlay.ScreenText( $"{windStrength} {isOnIce} {isInShelter} :\n{Sounds[S_NORMAL].TargetVolume} {Sounds[S_STRONG].TargetVolume} {Sounds[S_NORMAL_MUFFLED].TargetVolume} {Sounds[S_STRONG_MUFFLED].TargetVolume}\n{Sounds[S_NORMAL].CurrentVolume} {Sounds[S_STRONG].CurrentVolume} {Sounds[S_NORMAL_MUFFLED].CurrentVolume} {Sounds[S_STRONG_MUFFLED].CurrentVolume}" );
		}
	}
}
