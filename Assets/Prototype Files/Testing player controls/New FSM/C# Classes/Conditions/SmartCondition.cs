using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class SmartCondition : ICondition
    {
        public bool DesiredValue;

        [SerializeReference, SubclassSelector]
        public ICondition Condition;

        public bool Evaluate() => Condition.Evaluate() == DesiredValue;
    }
}