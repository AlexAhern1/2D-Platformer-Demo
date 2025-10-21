using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HealthDisplay
    {
        [Header("Monobehaviours")]
        [SerializeField] private GaugeBarUI _healthBar;

        public void InitializeHealth(float max, float min)
        {
            _healthBar.SetMinAndMaxValues(min, max);
        }

        public void UpdateHealth(float currentHealth)
        {
            _healthBar.SetCurrentValue(currentHealth);
        }
    }
}