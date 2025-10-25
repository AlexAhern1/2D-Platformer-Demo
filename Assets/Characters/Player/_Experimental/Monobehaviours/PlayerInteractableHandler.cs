using UnityEngine;

namespace Game.Player
{
    public class PlayerInteractableHandler : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private InputEvent<Vector2> _moveInputEvent;
        [SerializeField] private GameEvent _operateEvent;

        private void OnEnable()
        {
            _moveInputEvent.AddEvent(OnPressMove);
        }

        private void OnDisable()
        {
            _moveInputEvent.RemoveEvent(OnPressMove);
        }

        private void OnPressMove(Vector2 input)
        {
            if (input.y < 0.6f) return;
            _operateEvent.Raise();
        }
    }
}