using UnityEngine;

namespace Game
{
    public class TestMover : MonoBehaviour
    {
        [SerializeField] private InputEvent<Vector2> _moveInputEvent;
        [SerializeField] private float _speed;

        private Vector2 _currentSpeed;

        private void OnEnable()
        {
            _moveInputEvent.AddEvent(OnPressMove);
        }

        private void OnDisable()
        {
            _moveInputEvent.RemoveEvent(OnPressMove);
        }

        private void OnPressMove(Vector2 moveData)
        {
            _currentSpeed = _speed * Time.deltaTime * moveData;
        }

        private void Update()
        {
            transform.Translate(_currentSpeed);
        }
    }
}