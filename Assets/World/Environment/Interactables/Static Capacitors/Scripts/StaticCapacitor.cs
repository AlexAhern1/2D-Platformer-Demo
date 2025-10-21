using System;
using UnityEngine;

namespace Game.World
{
    public class StaticCapacitor : MonoBehaviour
    {
        [SerializeField] float staticGainOnHit;
        [SerializeField] int availableHits;
        int hitsLeft;

        [SerializeField] FloatEvent giveStaticEvent;

        //private IDamageableOLD _damageable;

        public Action<int> UpdateCurrentValue { get; set; }
        public Action<int, int> UpdateRange { get; set; }

        public float Get(string id)
        {
            return staticGainOnHit;
        }

        private void Awake()
        {
            //_damageable = GetComponent<IDamageableOLD>();
        }

        private void OnEnable()
        {
            //_damageable.ReceiveDamageEvent += OnHit;
        }

        private void OnDisable()
        {
            //_damageable.ReceiveDamageEvent -= OnHit;
        }

        //private void OnHit(DamageDataOLD data)
        //{
        //    if (hitsLeft == 0) return;
        //    GivePlayerStatic();
        //}

        private void Start()
        {
            hitsLeft = availableHits;

            UpdateRange?.Invoke(0, hitsLeft);
            UpdateCurrentValue?.Invoke(hitsLeft);
        }

        private void GivePlayerStatic()
        {
            giveStaticEvent.Raise(staticGainOnHit);
            hitsLeft--;
            UpdateCurrentValue?.Invoke(hitsLeft);
        }
    }
}