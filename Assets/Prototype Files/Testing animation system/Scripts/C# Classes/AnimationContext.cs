using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Data container for animations.
    /// </summary>
    [Serializable]
    public class AnimationContext
    {
        public int ID;
        public AnimationClip Clip;
    }
}