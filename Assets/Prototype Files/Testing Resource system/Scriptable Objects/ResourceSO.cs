using UnityEngine;

namespace Game
{
    public abstract class ResourceSO : ScriptableObject
    {
        [SerializeField] private FloatReference _currentValue;
        [SerializeField] private float _minValue;

        public float Current => _currentValue.Value;

        public float Min => _minValue;

        public abstract float Max { get; }

        public void Add(float amount)
        {
            Set(Current + amount);
        }

        public void Set(float amount)
        {
            _currentValue.Set(Mathf.Clamp(amount, Min, Max));
        }
    }
}