using System;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("State machine")]
        [SerializeField] private NPCStateMachine FSM;
        [SerializeField] private AttackFilterHandler AttackFilter;

        [Header("Components")]
        [SerializeField] private NPCHealthHandler _enemyHealthHandler;

        [Header("Mediators")]
        [SerializeReference, SubclassSelector] 
        private HealthMediator _healthMediator;
        [SerializeField] private bool _enableExternally;

        // events
        private Action _despawnEvent;

        public void RestoreHealth()
        {
            _enemyHealthHandler.Initialize();
        }

        public void Initialize()
        {
            AttackFilter.Initialize();
            FSM.Initialize();

            _enemyHealthHandler.Initialize();
        }

        public void Enable()
        {
            FSM.Enable();

            if (_enableExternally) return;
            _healthMediator.Enable();
        }

        public void Disable()
        {
            FSM.Disable();

            _despawnEvent?.Invoke();

            if (_enableExternally) return;
            _healthMediator.Disable();
        }

        public void AddDespawnObserver(Action e) => _despawnEvent += e;

        public void RemoveDespawnObserver(Action e) => _despawnEvent -= e;

        public void EnableHealthMediatorExternally() => _healthMediator.Enable();

        public void DisableHealthMediatorExternally() => _healthMediator.Disable();

        private void Update()
        {
            FSM.Run();
        }

        private void FixedUpdate()
        {
            FSM.FixedRun();

            AttackFilter.FixedRun();
        }
    }
}