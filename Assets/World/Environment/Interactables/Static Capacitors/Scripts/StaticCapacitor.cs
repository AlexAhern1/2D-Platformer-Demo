using System;
using UnityEngine;

namespace Game.World
{
    public class StaticCapacitor : MonoBehaviour
    {
        [SerializeField] float staticGainOnHit;
        [SerializeField] int availableHits;
        int hitsLeft;

        [SerializeField] private FloatEvent _addStaticEvent;
        [SerializeField] private Damageable _damgeableComponent;

        public Action<int> UpdateCurrentValue { get; set; }
        public Action<int, int> UpdateRange { get; set; }

        public float Get(string id)
        {
            return staticGainOnHit;
        }

        private void OnEnable()
        {
            _damgeableComponent.AddEvent(OnHit);
        }

        private void OnDisable()
        {
            _damgeableComponent.RemoveEvent(OnHit);
        }

        private void OnHit(Damage damage)
        {
            if (hitsLeft == 0) return;
            GivePlayerStatic();
        }

        private void Start()
        {
            hitsLeft = availableHits;

            UpdateRange?.Invoke(0, hitsLeft);
            UpdateCurrentValue?.Invoke(hitsLeft);
        }

        private void GivePlayerStatic()
        {
            if (_addStaticEvent != null) _addStaticEvent.Raise(staticGainOnHit);
            else Logger.Warn("No add static event");

                hitsLeft--;
            UpdateCurrentValue?.Invoke(hitsLeft);
        }
    }
}