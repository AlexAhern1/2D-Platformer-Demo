using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Resource/Float Max")]
    public class FloatMaxResource : ResourceSO
    {
        [SerializeField] private FloatReference _maxValue;

        public override float Max => _maxValue.Value;
    }
}