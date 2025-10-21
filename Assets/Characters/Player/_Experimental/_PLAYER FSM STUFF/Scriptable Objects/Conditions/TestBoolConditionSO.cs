using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Conditions/Test")]
    public class TestBoolConditionSO : ConditionSO
    {
        public bool Value;

        public override bool Evaluate() => Value;
    }
}