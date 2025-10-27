using UnityEngine;

namespace Game.Player
{
    public class PlayerHealHandler : MonoBehaviour, IStateAction
    {
        [SerializeField] private float _healAmount;
        [SerializeField] private float _healCost;

        [SerializeField] private ResourceSO _currentHealth;
        [SerializeField] private ResourceSO _currentStatic;


        public void DoAction()
        {
            if (_currentStatic.Current < _healCost || _currentHealth.Current == _currentHealth.Max) return;

            _currentHealth.Add(_healAmount);
            _currentStatic.Add(-_healCost);
        }
    }
}