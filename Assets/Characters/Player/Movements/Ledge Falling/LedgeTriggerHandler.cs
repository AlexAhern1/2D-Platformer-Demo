using UnityEngine;

namespace Game
{
    public class LedgeTriggerHandler : MonoBehaviour
    {
        [SerializeField] private LedgePlatform _platform;
        [SerializeField] private Tag _playerTag;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag)) return;
            else if (_platform.CollisionEnabled)
            {
                _platform.DisableCollision(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag)) return;
            else if (!_platform.CollisionEnabled)
            {
                _platform.EnableCollision(collision);
            }
        }
    }
}