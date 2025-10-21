using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Math/More or Equal")]
    public class MoreqOp : ComparisonOperator
    {
        public override bool Evaluate(float a, float b) => a >= b;
    }
}