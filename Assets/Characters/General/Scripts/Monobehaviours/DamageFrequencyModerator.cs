using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DamageFrequencyModerator : MonoBehaviour
    {
        [SerializeField][Min(0.01f)] private float _ignorePeriod;
        [SerializeField][Min(1)] private int _damageablesSize;

        // collections
        private Dictionary<IDamageable, float> _damageables;

        private void Awake()
        {
            _damageables = new(_damageablesSize);
        }

        public bool UpdateAndCheckIfCanDamage(IDamageable damageable)
        {
            if (!_damageables.ContainsKey(damageable))
            {
                _damageables.Add(damageable, Time.time + _ignorePeriod);
                return true;
            }
            else if (Time.time < _damageables[damageable])
            {
                return false;
            }

            _damageables[damageable] = Time.time + _ignorePeriod;
            return true;
        }

        public void ClearDamageables() => _damageables.Clear();
    }
}