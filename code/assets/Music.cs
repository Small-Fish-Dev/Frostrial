using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	[Library("music"), AutoGenerate]
	public partial class Music : Asset
	{
		public static IReadOnlyDictionary<string, Music> All => _all;
		internal static Dictionary<string, Music> _all = new();

		public string Artist { get; set; }
		public string Album { get; set; }
		public string Song { get; set; }
		public float Length { get; set; }
		public string URL { get; set; }
		public string AlbumCover { get; set; }

		protected override void PostLoad()
		{
			base.PostLoad();

			if ( !_all.ContainsKey( Name ) )
				_all.Add( Name, this );
		}

		public Sound Play(float volume = 1)
		{
			var s = Sound.FromScreen( Name );
			s.SetVolume( volume );

			return s;
		}
	}
}
