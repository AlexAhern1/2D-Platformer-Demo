using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/More")]
    public class MoreOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a > b;
    }
}