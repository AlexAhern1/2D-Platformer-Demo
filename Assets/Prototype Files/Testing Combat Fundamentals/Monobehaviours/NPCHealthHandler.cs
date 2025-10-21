using System;
using UnityEngine;

namespace Game
{
    public class NPCHealthHandler : MonoBehaviour
    {
        [Header("Health Config")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _physicalResist;
        [SerializeField] private float _electricalResist;
        [SerializeField] private float _thermalResist;
        [SerializeField] private float _chemicalResist;
        [SerializeField] private float _staggerTolerance;
        [SerializeField] private float _attackStrengthResist;
        [SerializeField] private float _knockbackCooldown;

        [Header("facing direction config")]
        [SerializeField] private Transform _forward;

        [Header("Default Transion Configs")]
        [SerializeField] private int _staggerTransitionID;
        [SerializeField] private int _lightKBTransitionID;
        [SerializeField] private int _heavyKBTransitionID;
        [SerializeField] private int _deathTransitionID;

        [SerializeField] private NPCStateTransitionHandler _transitionHandler;

        // events
        private event Action _healthChangeEvent;

        // state data
        private float _currentHealth;
        private float _staggerBuildup;
        private float _knockbackAvailableTime;
        private GameObject _recentStaggerSourceObject;

        // properties
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float CurrentHealthRatio => _currentHealth / _maxHealth;

        public float PhysicalResist => _physicalResist;
        public float ThermalResist => _thermalResist;
        public float ElectricalResist => _electricalResist;
        public float ChemicalResist => _chemicalResist;

        public bool CanStagger => _staggerBuildup >= _staggerTolerance;
        public bool KnockbackReady => Time.time >= _knockbackAvailableTime;
        public float AttackStrengthResistance => _attackStrengthResist;

        // to be initialized later.
        public void Initialize()
        {
            _currentHealth = _maxHealth;
            _staggerBuildup = 0;
        }

        public void AddHealthChangeEvent(Action e) => _healthChangeEvent += e;

        public void RemoveHealthChangeEvent(Action e) => _healthChangeEvent -= e;

        public void ResetStaggerBuildup()
        {
            _staggerBuildup = 0;
        }

        public void StartKnockbackCooldown()
        {
            _knockbackAvailableTime = Time.time + _knockbackCooldown;
        }

        public void AddHealth(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
            _healthChangeEvent?.Invoke();
        }

        public void AddStaggerDamage(float damage)
        {
            _staggerBuildup = Mathf.Clamp(_staggerBuildup + damage, 0, _staggerTolerance);
        }
    }
}