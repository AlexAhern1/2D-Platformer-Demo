using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HurtTransitionConfig
    {
        [Header("Light Knockback")]
        public bool CanLightKnockback = false;

        [Header("Heavy Knockback")]
        public bool CanHeavyKnockback = true;

        [Header("Stagger")]
        public bool CanStagger = true;
    }
}