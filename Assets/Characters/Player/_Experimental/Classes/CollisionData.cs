using UnityEngine;

namespace Game
{
    public class CollisionData
    {
        public void SetCollider(Collider2D col) => Collider = col;
        public void SetDirection(Vector2 direction) => Direction = direction;
        public void SetEnter(bool enter) => Enter = enter;

        public Collider2D Collider { get; private set; }
        public Vector2 Direction {  get; private set; }
        public bool Enter { get; private set; }
    }
}