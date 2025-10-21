using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Not Equals")]
    public class NotEqualsOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a != b;
    }
}