using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class CollisionCondition : ICondition
    {
        [Header("Components")]
        public CollisionHandler Collisions;

        [Header("Collision Data")]
        public Vector2 Direction;
        public bool IsEnter;
        public bool UseBothHorizontal;

        public bool Evaluate()
        {
            bool isColliding = UseBothHorizontal ? Collisions.IsColliding(Vector2.left) || Collisions.IsColliding(Vector2.right) : Collisions.IsColliding(Direction);

            return (IsEnter && isColliding) || (!IsEnter && !isColliding);
        }
    }
}