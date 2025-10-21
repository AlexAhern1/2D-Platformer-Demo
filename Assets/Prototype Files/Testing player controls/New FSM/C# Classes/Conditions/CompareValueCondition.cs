using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class CompareValueCondition : ICondition
    {
        [Header("Values")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter Value1;

        [SerializeReference, SubclassSelector]
        public IFloatGetter Value2;

        [Header("Comparison Opeartor")]
        public ComparisonOperator Comparator;

        public bool Evaluate() => Comparator.Evaluate(Value1.GetFloat(), Value2.GetFloat());
    }
}