using System;
using UnityEngine;

namespace Game
{
    public class BreakableObject : Damageable
    {
        //  can - change the way in which hits are calculated to destroy the object.
        //  currently - count number of hits.
        //  potentially - calculate based on total damage from Damage struct input data

        [SerializeField] private int _hitsToBreak;
        [SerializeField] private Tag _playerTag;

        // state variables
        private int _hitCount;

        // events
        public Action<Damage> HitEvent { get; set; }
        public Action<Damage> BreakEvent { get; set; }

        public override void TakeDamage(Damage dmg)
        {
            if (dmg.Attacker.tag != _playerTag) return;

            _hitCount++;
            if (_hitCount < _hitsToBreak)
            {
                // play sound
                // emit particles
                // shake the object
                // others (if any)

                HitEvent?.Invoke(dmg);

                return;
            }

            // play sound
            // emit particles
            // screen shake
            // others (if any)

            BreakEvent?.Invoke(dmg);
        }
    }
}