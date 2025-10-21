using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Multiply")]
    public class MultiplicationOp : BinaryOperator
    {
        public override float Get(float a, float b) => a * b;
    }
}
