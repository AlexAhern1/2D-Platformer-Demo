using UnityEngine;

namespace Game.World
{
    public class StaticZone : MonoBehaviour
    {
        [SerializeField] private Tag _playerTag;
        [SerializeField] private float _staticRegenerationRate;

        [SerializeField] private FloatEvent _staticRegenerationEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != _playerTag) return;
            _staticRegenerationEvent.Raise(_staticRegenerationRate);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag != _playerTag) return;
            _staticRegenerationEvent.Raise(-_staticRegenerationRate);
        }
    }
}