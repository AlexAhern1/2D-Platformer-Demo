using UnityEngine;

namespace Game.Player
{
    public class PlayerResourceBarMediator : MonoBehaviour, IEnable
    {
        [Header("Values")]
        [SerializeField] private FloatReference _currentHealth;
        [SerializeField] private Stat _maxHealthStat;

        [Header("UI objects")]
        [SerializeField] private GaugeBarUI _playerHealthBarUI;
        [SerializeField] private GameEvent _playerRespawnEvent;

        public void Enable()
        {
            _playerHealthBarUI.SetMinAndMaxValues(0, _maxHealthStat.Value);
            _playerHealthBarUI.SetCurrentValue(_currentHealth.Value);

            _currentHealth.AddEvent(OnCurrentHealthChanged);
            _maxHealthStat.AddOnChangeEvent(OnMaxHealthChanged);

            _playerRespawnEvent.AddEvent(OnPlayerRespawn);
        }

        public void Disable()
        {
            _currentHealth.RemoveEvent(OnCurrentHealthChanged);
            _maxHealthStat.RemoveOnChangeEvent(OnMaxHealthChanged);

            _playerRespawnEvent.RemoveEvent(OnPlayerRespawn);
        }

        private void OnCurrentHealthChanged(float currentHealth)
        {
            _playerHealthBarUI.SetCurrentValue(currentHealth);
        }

        private void OnMaxHealthChanged(float maxHealth)
        {
            _playerHealthBarUI.SetMaxValue(maxHealth);
        }

        private void OnPlayerRespawn()
        {
            _playerHealthBarUI.SetCurrentValue(_maxHealthStat.Value);
        }
    }
}