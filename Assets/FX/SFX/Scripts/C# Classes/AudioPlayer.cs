using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AudioPlayer
    {
        [SerializeField] private AudioConfig _data;
        [SerializeField] private AudioObject _objectPrefab;

        private AudioData _sampledClip;

        public void Play()
        {
            if (_data == null || _objectPrefab == null)
            {
                Logger.Log($"Cannot play sound from {this}", Logger.Audio, MoreColors.LightOrange);
                return;
            }

            // get a sample audio clip from audio data
            _sampledClip = _data.SampleAudioClip();

            // check if no clip was obtained. if so, return early.
            if (_sampledClip == null) return;

            // create a new instance of AudioObject (TODO - use pooling)
            var obj = GameObject.Instantiate(_objectPrefab);

            // play the sampled clip through the instantiated audio object
            obj.Play(_sampledClip);
        }
    }
}