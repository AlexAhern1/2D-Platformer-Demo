using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public abstract class AudioConfig : ScriptableObject
    {
        [SerializeField] protected AudioMixerGroup mixerGroup;
        protected AudioData data = new();

        public abstract AudioData SampleAudioClip();
    }
}