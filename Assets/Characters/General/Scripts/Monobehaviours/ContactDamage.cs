using UnityEngine;

namespace Game
{
    public class ContactDamage : MonoBehaviour
    {
        [SerializeField] private Damage _contactDamage;
        [SerializeField] private Tag _playerTag;

        [SerializeField] private bool _ignorePlayer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag) && _ignorePlayer) return;
            else if (collision.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_contactDamage);
            }
        }
    }
}