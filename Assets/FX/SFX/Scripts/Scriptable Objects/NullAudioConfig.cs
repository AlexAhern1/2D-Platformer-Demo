using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Audio/Null")]
    public class NullAudioConfig : AudioConfig
    {
        public override AudioData SampleAudioClip() => null;
    }
}