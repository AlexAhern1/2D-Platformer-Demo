using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Less or Equal")]
    public class LessEqOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a <= b;
    }
}