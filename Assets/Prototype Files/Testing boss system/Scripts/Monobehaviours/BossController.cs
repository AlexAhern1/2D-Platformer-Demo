using Game.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class BossController : MonoBehaviour
    {
        [Header("State machine")]
        [SerializeField] private NPCStateMachine FSM;
        [SerializeField] private AttackFilterHandler AttackFilter;

        [Header("Components")]
        [SerializeField] private NPCHealthHandler _enemyHealthHandler;

        [Header("Mediators")]
        [SerializeReference, SubclassSelector]
        private HealthMediator _healthMediator;

        public void EnableHealthMediator()
        {
            _healthMediator.Enable();
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
        }

        public void Disable()
        {
            FSM.Disable();

            _healthMediator.Disable();
        }

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