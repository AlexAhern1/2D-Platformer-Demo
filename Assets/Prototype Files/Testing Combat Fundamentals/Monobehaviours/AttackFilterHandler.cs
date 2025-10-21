using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttackFilterHandler : MonoBehaviour
    {
        [SerializeField] private int _listSize;
        [SerializeField] private NPCStateTransitionHandler _transitionHandler;

        private List<int> _potentialAttacks;

        private bool PendingForFiltration => _potentialAttacks.Count > 0;

        public void Initialize()
        {
            _potentialAttacks = new List<int>(_listSize);
        }

        public void AddPotentialAttack(int attackID)
        {
            _potentialAttacks.Add(attackID);
        }

        public void FixedRun()
        {
            while (PendingForFiltration)
            {
                // filtration process happens here
                DoFiltration();

                _potentialAttacks.Clear();
            }
        }

        private void DoFiltration()
        {
            int randomIndex = Random.Range(0, _potentialAttacks.Count);
            int randomAttackID = _potentialAttacks[randomIndex];

            _transitionHandler.DoTransition(randomAttackID);
        }
    }
}