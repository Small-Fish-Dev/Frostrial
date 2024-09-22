using Sandbox;
using System.Collections.Generic;
using Sandbox.Audio;

namespace Frostrial;

[GameResource("Music", "music", "Frostrial Music", Icon = "approval")]
public class Music : GameResource
{
    public static IReadOnlyDictionary<string, Music> All => _all;
    internal static Dictionary<string, Music> _all = new();

    public string Artist { get; set; }
    public string Album { get; set; }
    public string Song { get; set; }
    public string URL { get; set; }
    public string AlbumCover { get; set; }
    public SoundFile File { get; set; }

    protected override void PostLoad()
    {
        base.PostLoad();

        if (!_all.ContainsKey(ResourceName))
            _all.Add(ResourceName, this);
    }

    public SoundHandle Play()
    {
        File.Preload();
        var s = Sound.PlayFile(File);
        s.TargetMixer = Mixer.FindMixerByName("Music");

        return s;
    }
}