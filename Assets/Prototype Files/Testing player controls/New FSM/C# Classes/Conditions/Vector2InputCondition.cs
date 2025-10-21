using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Vector2InputCondition : ICondition
    {
        public Vector2InputEvent InputEvent;
        public bool ReadHorizontal;

        public bool Evaluate()
        {
            float value = ReadHorizontal ? InputEvent.InputValue.x : InputEvent.InputValue.y;
            return Mathf.Abs(value) > 0;
        }
    }
}