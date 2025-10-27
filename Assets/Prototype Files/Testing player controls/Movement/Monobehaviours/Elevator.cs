using UnityEngine;

namespace Game
{
    public class Elevator : MonoBehaviour, IMovingPlatform
    {
        [SerializeField] private Rigidbody2D _rb2d;

        [SerializeField] AnimationCurve _velocityCurve;
        [SerializeField] private float _movementDuration;

        [SerializeField] private int _movementDirection;
        [SerializeField] private float _totalMovementDistance;

        // properties
        public Vector2 Velocity { get; private set; }

        // state variables
        private Vector2 _endPosition;
        private bool _isMoving;
        private float _currentTime;

        [ContextMenu("Activate Elevator")]
        public void Activate()
        {
            if (_isMoving) return;

            _isMoving = true;
            _currentTime = 0;

            _endPosition = _rb2d.position + _totalMovementDistance * _movementDirection * Vector2.up;
        }

        public void Deactivate()
        {
            // this might get called in scene unloading.
        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;
            else if (_currentTime >= _movementDuration)
            {
                _isMoving = false;
                _movementDirection *= -1;
                Velocity = Vector2.zero;

                _rb2d.position = _endPosition;
            }
            else
            {
                Velocity = _movementDirection * _velocityCurve.Evaluate(_currentTime) * Vector2.up;
                _currentTime += Time.fixedDeltaTime;
            }

            _rb2d.velocity = Velocity;
        }
    }
}