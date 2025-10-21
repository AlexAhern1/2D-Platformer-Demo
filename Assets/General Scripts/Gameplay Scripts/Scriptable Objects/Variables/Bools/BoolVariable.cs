using UnityEngine;

namespace Game.Variables
{
    [CreateAssetMenu(fileName = "Bool Var", menuName = "SO/Variables/Bool")]
    public class BoolVariable : AssetVariable<bool>, ICondition
    {
        public bool Evaluate() => Value;
    }
}