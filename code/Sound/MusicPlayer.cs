using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Events;

namespace Frostrial;

public record NextSongEvent(Music Music) : IGameEvent;

public sealed class FrostrialMusicPlayer : Component
{
    public List<Music> Queue { get; private set; } = new();
    [Property] public float TracksDelay { get; set; } = 30f;

    private int _currentQueuePosition = -1;
    private RealTimeSince _tsLastTrack = 0;
    private MusicPlayer _currentMusicPlayer = null;

    protected override void OnEnabled()
    {
        Queue = Music.All.Values.ToList();
        Shuffle(true);
        PlayNext();
    }

    protected override void OnUpdate()
    {
        if (_currentMusicPlayer is null && _tsLastTrack >= TracksDelay)
        {
            PlayNext();
        }
    }

    public void PlayNext()
    {
        var nextTrack = (_currentQueuePosition + 1) % Queue.Count;
        if (nextTrack < _currentQueuePosition)
        {
            Shuffle();
        }
        _currentQueuePosition = nextTrack;

        _currentMusicPlayer = Queue[_currentQueuePosition].Play();
        _currentMusicPlayer.OnFinished = () =>
        {
            _currentMusicPlayer = null;
            _tsLastTrack = 0;
        };

        GameObject.Dispatch(new NextSongEvent(Queue[_currentQueuePosition]));
    }

    private void Shuffle(bool ignoreFirstLastRepeat = false)
    {
        Log.Info($"Shuffling {Queue.Count} tracks");
        var lastTrack = Queue[^1];
        for (var i = 0; i < Queue.Count - 1; i++)
        {
            for (var j = i + 1; j < Queue.Count; j++)
            {
                if (System.Random.Shared.Int(0, 1) != 0)
                    continue;

                if (!ignoreFirstLastRepeat
                    && i == 0 && j == Queue.Count - 1
                    && Queue[j].ResourceId ==
                    lastTrack
                        .ResourceId) // If we are swapping the first and the last queue entry, make sure we won't make the listener to hear the same song twice
                    continue;

                (Queue[i], Queue[j]) = (Queue[j], Queue[i]);
            }
        }
    }
}