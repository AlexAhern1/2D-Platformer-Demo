using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Input/Events/Float")]
    public class FloatInputEvent : InputEvent<float>, IFloatGetter
    {
        public float GetFloat() => InputValue;
    }
}