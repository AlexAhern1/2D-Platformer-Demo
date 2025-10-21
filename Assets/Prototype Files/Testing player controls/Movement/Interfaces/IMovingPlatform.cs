using UnityEngine;

namespace Game
{
    public interface IMovingPlatform
    {
        public Vector2 Velocity { get; }

        public void Activate();

        public void Deactivate();
    }
}