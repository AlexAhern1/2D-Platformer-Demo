using UnityEngine;

namespace Game.Variables
{
    [CreateAssetMenu(fileName = "Float Var", menuName = "SO/Variables/Float")]
    public class FloatVariable : AssetVariable<float>
    {
        private void OnValidate()
        {
            InvokeChangeEvent();
        }
    }
}