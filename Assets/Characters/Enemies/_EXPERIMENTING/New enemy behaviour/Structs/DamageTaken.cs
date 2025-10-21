using System;

namespace Game
{
    [Serializable]
    public struct DamageTaken
    {
        public float TotalDamage;
        public HealthDamage[] DamageTypes;
    }
}