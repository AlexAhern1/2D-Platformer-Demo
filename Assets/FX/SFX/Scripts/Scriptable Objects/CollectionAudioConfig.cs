using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Audio/Collection")]
    public class CollectionAudioConfig : AudioConfig
    {
        [SerializeField] private AudioClip[] dataCollection;
        [Min(0)] public float Volume;
        [Min(0)] public float Pitch;

        public override AudioData SampleAudioClip()
        {
            data.Volume = Volume;
            data.Pitch = Pitch;

            data.Clip = dataCollection[Random.Range(0, dataCollection.Length)]; ;
            data.MixerGroup = mixerGroup;

            return data;
        }
    }
}