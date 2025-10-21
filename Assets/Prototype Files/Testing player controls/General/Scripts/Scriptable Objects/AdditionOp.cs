using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Add")]
    public class AdditionOp : BinaryOperator
    {
        public override float Get(float a, float b) => a + b;
    }
}