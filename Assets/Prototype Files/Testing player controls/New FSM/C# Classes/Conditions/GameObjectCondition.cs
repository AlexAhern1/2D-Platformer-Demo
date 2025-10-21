using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class BehaviourConditionReference : ICondition
    {
        public InterfaceReference<MonoBehaviour, ICondition> Condition;

        public bool Evaluate() => Condition.Interface.Evaluate();
    }

    [System.Serializable]
    public class ScriptableObjectConditionReference : ICondition
    {
        public InterfaceReference<ScriptableObject, ICondition> Condition;

        public bool Evaluate() => Condition.Interface.Evaluate();
    }
}