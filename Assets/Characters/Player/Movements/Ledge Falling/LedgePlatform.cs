using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LedgePlatform : MonoBehaviour
    {
        public bool CollisionEnabled { get; set; }

        [SerializeField] private Collider2D _platformCollider;

        private void Awake()
        {
            CollisionEnabled = true;
        }

        public void EnableCollision(Collider2D playerCollider)
        {
            if (CollisionEnabled) return;
            CollisionEnabled = true;
            Physics2D.IgnoreCollision(_platformCollider, playerCollider, false);
        }

        public void DisableCollision(Collider2D playerCollider)
        {
            if (!CollisionEnabled) return;
            CollisionEnabled = false;
            Physics2D.IgnoreCollision(_platformCollider, playerCollider, true);
        }
    }
}