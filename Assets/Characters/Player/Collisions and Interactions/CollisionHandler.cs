using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    
    ///<summary>
    ///Handles collision detection and updates the direction with which direction the game object collided towards.
    /// </summary>
    public class CollisionHandler : MonoBehaviour, IEnable
    {
        [Header("Collider component")]
        [SerializeField] private Collider2D _collider;

        [Header("Collision layers")]
        [SerializeField] private LayerMask _collisionMask;

        [Header("Events")]
        [SerializeField] private CollisionEvent _collisionEvent;
        [SerializeField] private GameEvent _enablePlayerColliderEvent;
        [SerializeField] private GameEvent _disablePlayerColliderEvent;

        // state variables
        private readonly CollisionData _data = new();

        // collections
        private readonly Dictionary<Collider2D, Vector2> _collisions = new();
        private readonly Dictionary<Vector2, int> _collisionCounts = new()
        {
            { Vector2.down, 0 },
            { Vector2.right, 0 },
            { Vector2.left, 0 },
            { Vector2.up, 0 },
        };

        // events
        private event Action<CollisionData> _localCollisionEvent;

        public void Enable()
        {
            _enablePlayerColliderEvent.AddEvent(OnEnableCollider);
            _disablePlayerColliderEvent.AddEvent(OnDisableCollider);
        }

        public void Disable()
        {
            _enablePlayerColliderEvent.RemoveEvent(OnEnableCollider);
            _disablePlayerColliderEvent.RemoveEvent(OnDisableCollider);
        }

        public void AddCollisionEvent(Action<CollisionData> e) => _localCollisionEvent += e;

        public void RemoveCollisionEvent(Action<CollisionData> e) => _localCollisionEvent -= e;

        public void RemoveCollision(Collider2D collider)
        {
            if (!_collisions.ContainsKey(collider))
            {
                Logger.Error($"cannot remove collider -{collider} as it does not exist in the dictionary!");
                return;
            }

            Vector2 direction = _collisions[collider];
            _collisions.Remove(collider);

            _collisionCounts[direction]--;
        }

        public void AddManualCollision(Collider2D collider, Vector2 direction)
        {
            //verifying if this direction is valid.
            if (!_collisionCounts.ContainsKey(direction))
            {
                Logger.Error($"Invalid direction when trying to manually set collision data - {direction}. direction must be one of UP/DOWN/LEFT/RIGHT");
                return;
            }

            else if (_collisions.ContainsKey(collider))
            {
                Logger.Error($"Cannot add a collision already existing - {collider}, remove it first if necessary.");
                return;
            }

            _collisions[collider] = direction;
            _collisionCounts[direction]++;
        }


        public bool IsColliding(Vector2 direction) => _collisionCounts[direction] > 0;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contactPoint = collision.GetContact(0);

            Vector2 normal = contactPoint.normal;
            Vector2 enterDirection;

            if (normal.y > 0)
            {
                enterDirection = Vector2.down;
            }
            else if (normal.y < 0)
            {
                enterDirection = Vector2.up;
            }
            else if (normal.x > 0)
            {
                enterDirection = Vector2.left;
            }
            else enterDirection = Vector2.right;

            if (_collisionCounts[enterDirection] == 0) RaiseCollisionEvent(collision.collider, enterDirection, true);

            _collisionCounts[enterDirection]++;
            _collisions.Add(collision.collider, enterDirection);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Vector2 exitDirection = _collisions[collision.collider];

            _collisionCounts[exitDirection]--;
            if (_collisionCounts[exitDirection] == 0)
            {
                RaiseCollisionEvent(collision.collider, exitDirection, false);
            }

            _collisions.Remove(collision.collider);
        }

        private void RaiseCollisionEvent(Collider2D col, Vector2 direction, bool enter)
        {
            _data.SetCollider(col);
            _data.SetDirection(direction);
            _data.SetEnter(enter);

            _localCollisionEvent?.Invoke(_data);
            _collisionEvent.Raise(_data);
        }

        private void OnEnableCollider() => _collider.enabled = true;

        private void OnDisableCollider() => _collider.enabled = false;
    }
}