using UnityEngine;

namespace Game
{
    public abstract class BinaryOperator : ScriptableObject
    {
        public abstract float Get(float a, float b);
    }
}