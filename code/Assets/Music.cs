using Sandbox;
using System.Collections.Generic;
using Sandbox.Audio;

namespace Frostrial;

[GameResource("Music", "music", "Frostrial Music", Icon = "music_note")]
public class Music : GameResource
{
    public static IReadOnlyDictionary<string, Music> All => _all;
    private static Dictionary<string, Music> _all = new();

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

    public MusicPlayer Play()
    {
        // TODO: HACK: MusicPlayer.Play doesn't like the raw paths
        var s = MusicPlayer.Play(FileSystem.Mounted, File.ResourcePath + "_c");
        s.Repeat = false;
        s.TargetMixer = Mixer.FindMixerByName("Music");

        return s;
    }
}