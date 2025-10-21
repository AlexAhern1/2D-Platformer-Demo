using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ScreenShakeData
    {
        [SerializeReference, SubclassSelector]
        public ICameraShaker Shaker;
        public float Duration;
    }
}