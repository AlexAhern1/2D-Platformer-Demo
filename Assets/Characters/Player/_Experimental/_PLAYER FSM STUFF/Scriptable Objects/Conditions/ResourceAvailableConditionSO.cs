using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Conditions/Resource Available")]
    public class ResourceAvailableConditionSO : ConditionSO
    {
        [SerializeField][Min(0)] private float _maxValue;
        [SerializeField] private float _currentValue;

        [Header("CHANGE THIS VALUE")]
        [SerializeField] private float _requiredValue;

        public float Max => _maxValue;

        public float Curr => _currentValue;

        public override bool Evaluate() => _currentValue >= _requiredValue;

        public void Reduce(float reduction) => _currentValue -= reduction;
    }
}