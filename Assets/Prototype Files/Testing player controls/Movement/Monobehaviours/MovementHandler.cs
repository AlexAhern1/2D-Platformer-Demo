using UnityEngine;

namespace Game
{
    public abstract class MovementHandler : MonoBehaviour, IMovement
    {
        public abstract Vector2 Velocity { get; }

        public abstract void Move(float horizontalSpeed, float verticalSpeed, bool @base);

        public abstract void MoveHorizontal(float speed, bool @base);

        public abstract void MoveVertical(float speed, bool @base);
    }
}