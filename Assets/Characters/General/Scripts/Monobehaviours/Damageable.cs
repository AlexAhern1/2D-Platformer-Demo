using System;
using UnityEngine;

namespace Game
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        protected event Action<Damage> damageEvent;

        public void AddEvent(Action<Damage> e) => damageEvent += e;

        public void RemoveEvent(Action<Damage> e) => damageEvent -= e;

        public virtual void TakeDamage(Damage damage)
        {
            RaiseDamageEvent(damage);
        }

        protected void RaiseDamageEvent(Damage damage)
        {
            damageEvent?.Invoke(damage);
        }
    }
}