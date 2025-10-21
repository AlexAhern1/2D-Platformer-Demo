using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// can configure what type of damage to apply to anything that gets passed in one of the process damageable methods
    /// </summary>
    public class DamageApplier : MonoBehaviour
    {
        [Header("Damage Config")]
        [SerializeField] Damage _damage;

        [Obsolete]
        public void ProcessDamage(Collider2D damageableCollider)
        {
            if (!damageableCollider.TryGetComponent<IDamageable>(out var damageable))
            {
                Logger.Error($"No Damageable Interface found on {damageableCollider}!");
                return;
            }

            damageable.TakeDamage(_damage);
        }

        public void ProcessDamage(IDamageable damageable)
        {
            damageable.TakeDamage(_damage);
        }
    }
}