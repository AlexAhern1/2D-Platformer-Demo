using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Resource/Stat Max")]
    public class StatMaxResource : ResourceSO
    {
        public Stat MaxValueStat;

        public override float Max => MaxValueStat.Value;
    }
}