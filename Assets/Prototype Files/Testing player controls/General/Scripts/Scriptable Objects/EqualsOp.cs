using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/Equals")]
    public class EqualsOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a == b;
    }
}