using Sandbox;
using Sandbox.Audio;

namespace Frostrial;

public sealed class AmbientWindManager : Component
{
    public class WindChannel
    {
        public readonly SoundHandle Sound;

        /// <summary>
        /// How fast will the volume go up
        /// </summary>
        public readonly float DeltaUp;

        /// <summary>
        /// How fast will the volume go down
        /// </summary>
        public readonly float DeltaDown;

        public float TargetVolume = 0;

        public WindChannel(SoundEvent file, float deltaUp = 1, float deltaDown = 1)
        {
            Sound = global::Sandbox.Sound.Play(file);
            // Start silent
            Sound.Volume = 0;
            Sound.TargetMixer = Mixer.FindMixerByName("Wind");

            DeltaUp = deltaUp;
            DeltaDown = deltaDown;
        }
    }

    [Property, Category("Sounds")] public SoundEvent WindNormal { get; set; }
    [Property, Category("Sounds")] public SoundEvent WindStrong { get; set; }
    // rndtrash: We could've used the brand new Mixer Low Pass Filter, but I feel it's too expensive for
    // a channel that has the exact same sounds all the time. It is more useful for stuff like voice chat.
    [Property, Category("Sounds")] public SoundEvent WindNormalMuffled { get; set; }
    [Property, Category("Sounds")] public SoundEvent WindStrongMuffled { get; set; }

    /// <summary>
    /// Minimum volume of the wind sounds
    /// </summary>
    [Property, Range(0, 1)] public float WindVolumeLowerBound = 0.3f;

    private WindChannel _windNormal;
    private WindChannel _windNormalMuffled;
    private WindChannel _windStrong;
    private WindChannel _windStrongMuffled;
    private bool _wasInShelter = true;

    protected override void OnEnabled()
    {
        _windNormal = new WindChannel(WindNormal, 1, 0.1f);
        _windStrong = new WindChannel(WindStrong, 0.1f, 0.1f);
        _windNormalMuffled = new WindChannel(WindNormalMuffled, 1, 0.1f);
        _windStrongMuffled = new WindChannel(WindStrongMuffled, 1, 0.1f);
    }

    protected override void OnUpdate()
    {
        // TODO:
        float windStrength = 0.5f;
        bool isOnIce = false;
        bool isInShelter = false;

        if (_wasInShelter != isInShelter)
        {
            // Quickly swap the loudness of muffled and normal variants
            (_windNormalMuffled.Sound.Volume, _windNormal.Sound.Volume) = (_windNormal.Sound.Volume,
                _windNormalMuffled.Sound.Volume);
            (_windStrongMuffled.Sound.Volume, _windStrong.Sound.Volume) = (_windStrong.Sound.Volume,
                _windStrongMuffled.Sound.Volume);

            // Mute the loud channels when in the shelter
            if (isInShelter)
            {
                _windNormal.TargetVolume = _windStrong.TargetVolume = 0;
            }
            else
            {
                _windNormalMuffled.TargetVolume = _windStrongMuffled.TargetVolume = 0;
            }

            _wasInShelter = isInShelter;
        }

        // The normal wind should be always heard at least a little bit
        var windVolumeNormal = windStrength.Remap(0, 1, WindVolumeLowerBound, 1);
        var windVolumeStrong = isOnIce || windStrength > 0.5 ? windStrength.Remap(0, 1, WindVolumeLowerBound, 1) : 0;

        if (isInShelter)
            _windNormalMuffled.TargetVolume = windVolumeNormal;
        else
            _windNormal.TargetVolume = windVolumeNormal;

        if (isInShelter)
            _windStrongMuffled.TargetVolume = windVolumeStrong;
        else
            _windStrong.TargetVolume = windVolumeStrong;

        UpdateSound(_windNormal);
        UpdateSound(_windNormalMuffled);
        UpdateSound(_windStrong);
        UpdateSound(_windStrongMuffled);
    }

    private static void UpdateSound(WindChannel sound)
    {
        sound.Sound.Volume = sound.Sound.Volume.LerpTo(sound.TargetVolume,
            (sound.TargetVolume > sound.Sound.Volume ? sound.DeltaUp : sound.DeltaDown) * Time.Delta);
    }
}