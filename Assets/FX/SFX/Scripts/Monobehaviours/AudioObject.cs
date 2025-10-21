using UnityEngine;

namespace Game
{
    public class AudioObject : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        /// <summary>
        /// This must always be called AFTER instantiating an audio object instance.
        /// </summary>
        /// <param name="data"></param>
        public void Play(AudioData data)
        {
            _source.volume = data.Volume;
            _source.pitch = data.Pitch;

            _source.clip = data.Clip;
            _source.outputAudioMixerGroup = data.MixerGroup;

            _source.Play();

            Destroy(gameObject, data.Clip.length);
        }
    }
}