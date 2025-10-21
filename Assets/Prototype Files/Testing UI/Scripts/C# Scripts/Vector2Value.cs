using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Vector2Value : IVector2Getter
    {
        public Vector2 Value;

        public Vector2 Get() => Value;
    }
}