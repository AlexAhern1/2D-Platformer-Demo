using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FlashConfig
    {
        public string Description;

        public Color FlashColor;
        public AnimationCurve AlphaCurve;
        public float Duration;
    }
}