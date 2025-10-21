using UnityEngine;

namespace Game
{
    public class PlatformVelocityRegister : MonoBehaviour
    {
        [SerializeField] private Tag _movingTag;
        [SerializeField] private MovementHandler _movementHandler;

        // state variables
        private IMovingPlatform _movingPlatform;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(_movingTag) || _movingPlatform != null) return;
            else if (!collision.gameObject.TryGetComponent(out _movingPlatform))
            {
                Logger.Error("Moving platform collided but it doesn't have a platform mover component.", MoreColors.BrightRed);
                return;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(_movingTag) || _movingPlatform == null) return;
            _movingPlatform = null;

            _movementHandler.Move(0, 0, false);
        }

        private void FixedUpdate()
        {
            if (_movingPlatform == null) return;
            Vector2 velocity = _movingPlatform.Velocity;

            _movementHandler.Move(velocity.x, Mathf.Min(velocity.y, 0), false);
        }
    }
}