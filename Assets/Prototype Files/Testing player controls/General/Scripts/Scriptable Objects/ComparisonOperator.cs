using UnityEngine;

namespace Game
{
    public abstract class ComparisonOperator : ScriptableObject
    {
        public abstract bool Evaluate(float a, float b);
    }
}