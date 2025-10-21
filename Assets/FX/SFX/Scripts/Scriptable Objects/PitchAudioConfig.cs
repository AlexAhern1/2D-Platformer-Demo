using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Audio/Pitch Only")]
    public class PitchAudioConfig : AudioConfig
    {
        public AudioClip Clip;
        [Min(0)] public float Volume;
        [Min(0)] public float Pitch;

        [Min(0)] public float PitchVariation;

        public override AudioData SampleAudioClip()
        {
            data.Volume = Volume;
            data.Pitch = Random.Range(Pitch - PitchVariation, Pitch +  PitchVariation);

            data.Clip = Clip;
            data.MixerGroup = mixerGroup;

            return data;
        }
    }
}