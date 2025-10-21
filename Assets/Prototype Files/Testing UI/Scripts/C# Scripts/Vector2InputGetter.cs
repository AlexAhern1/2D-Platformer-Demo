using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Vector2InputGetter : IVector2Getter
    {
        public Vector2InputEvent InputEvent;

        public Vector2 Get() => InputEvent.InputValue;
    }
}