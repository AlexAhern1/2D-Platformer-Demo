using UnityEngine;

namespace Game.World
{
    /// <summary>
    /// class that handles only the setting of the velocity of a platform
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObjectMover : MonoBehaviour
    {
        private Rigidbody2D _rb2d;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        public void SetTargetDuration(Vector3 targetPosition, float timeToReach)
        {
            Vector3 direction = (targetPosition - transform.position);
            Vector3 targetVelocity = direction / timeToReach;

            _rb2d.velocity = targetVelocity;
        }

        public void SetTargetSpeed(Vector3 targetPosition, float speed)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 targetVelocity = direction * speed;

            _rb2d.velocity = targetVelocity;
        }

        public void StopMovement()
        {
            _rb2d.velocity = Vector2.zero;
        }
    }
}