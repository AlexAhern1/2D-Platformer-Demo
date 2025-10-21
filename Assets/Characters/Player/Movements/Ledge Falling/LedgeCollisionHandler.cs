using UnityEngine;

namespace Game.World
{
    public class LedgeCollisionHandler : MonoBehaviour
    {
        [SerializeField] private Tag _playerTag;
        [SerializeField] private LedgePlatform _platform;

        [SerializeField] private float _dropDownAfterSeconds;
        [SerializeField] private bool _dropDown;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag(_playerTag)) return;
            else if (collision.contacts[0].normal.x != 0 && _platform.CollisionEnabled)
            {
                _platform.DisableCollision(collision.collider);
            }
            else if (collision.contacts[0].normal == Vector2.down && _dropDown)
            {
                Invoke("D", _dropDownAfterSeconds);
            }
        }

        //this is just for testing. Can delete after player can drop down ledges at will.
        private void D()
        {
            _platform.DisableCollision(FindObjectOfType<Player.PlayerController>().GetComponent<Collider2D>());
        }
    }
}