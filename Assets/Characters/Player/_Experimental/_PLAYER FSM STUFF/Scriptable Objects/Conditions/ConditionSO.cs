using UnityEngine;

namespace Game
{
    public abstract class ConditionSO : ScriptableObject, ICondition
    {
        public abstract bool Evaluate();

        public virtual void ResetCondition() { }

        public virtual void Initialize() { }
    }
}