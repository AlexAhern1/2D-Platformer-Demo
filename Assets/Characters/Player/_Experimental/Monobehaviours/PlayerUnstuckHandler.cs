using UnityEngine;

namespace Game.Player
{
    public class PlayerUnstuckHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerState _airborneState;
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private CollisionHandler _collisionHandler;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private PlayerMovementPhysics _physics;

        [Header("values")]
        [SerializeField] private float _nudgeDist;

        [Header("Events")]
        [SerializeField] InputEvent<float> _unstuckInputEvent;


        public void Enable()
        {
            _unstuckInputEvent.AddEvent(OnPressUnstuck);
        }

        public void Disable()
        {
            _unstuckInputEvent.RemoveEvent(OnPressUnstuck);
        }

        private void OnPressUnstuck(float data)
        {
            if (data <= 0) return;

            if (_collisionHandler.IsColliding(Vector2.down))
            {
                // move transform up just a tiny bit
                _playerTransform.position += Vector3.up * _nudgeDist;
            }

            _collisionHandler.ToggleCollisions(false);

            _collisionHandler.ClearCollisions();
            _collisionHandler.ToggleCollisions(true);

            _stateMachine.DoTransition(_airborneState);
            _physics.ToggleFalling(true);
        }
    }
}