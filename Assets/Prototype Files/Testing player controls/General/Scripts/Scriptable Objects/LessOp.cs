using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Less")]
    public class LessOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a < b;
    }
}