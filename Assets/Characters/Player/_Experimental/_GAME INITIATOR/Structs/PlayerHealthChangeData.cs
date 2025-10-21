namespace Game.Player
{
    public struct PlayerHealthChangeData
    {
        public DamageTaken DamageTaken;
        public WeaponType Weapon;

        // posture
        public float StaggerStrength;
        public float StaggerResistance;
    }
}