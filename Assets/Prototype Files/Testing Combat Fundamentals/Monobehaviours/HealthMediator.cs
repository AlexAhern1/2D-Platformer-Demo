using UnityEngine;

namespace Game.Enemy
{
    [System.Serializable]
    public abstract class HealthMediator
    {
        public abstract void Enable();

        public abstract void Disable();
    }

    [System.Serializable]
    public class NPCLocalHealthMediator : HealthMediator
    {
        [SerializeField] private GaugeBarUI _healthBar;
        [SerializeField] private NPCHealthHandler _healthHandler;

        public override void Enable()
        {
            _healthBar.SetMinAndMaxValues(0, _healthHandler.MaxHealth);
            _healthBar.SetCurrentValue(_healthHandler.CurrentHealth);

            _healthHandler.AddHealthChangeEvent(OnHealthChanged);
        }

        public override void Disable()
        {
            _healthHandler.RemoveHealthChangeEvent(OnHealthChanged);
        }

        private void OnHealthChanged()
        {
            _healthBar.SetCurrentValue(_healthHandler.CurrentHealth);
        }
    }

    [System.Serializable]
    public class NPCHUDHealthMediator : HealthMediator
    {
        [SerializeField] private BoolEvent _toggleHUDBossHealthEvent;
        [SerializeField] private Vector2Event _initializeHealthEvent;
        [SerializeField] private FloatEvent _setCurrentHealthEvent;

        [SerializeField] private NPCHealthHandler _healthHandler;

        public override void Enable()
        {
            _toggleHUDBossHealthEvent.Raise(true);

            _initializeHealthEvent.Raise(new Vector2(0, _healthHandler.MaxHealth));
            _setCurrentHealthEvent.Raise(_healthHandler.CurrentHealth);

            _healthHandler.AddHealthChangeEvent(OnHealthChanged);
        }

        public override void Disable()
        {
            _toggleHUDBossHealthEvent.Raise(false);

            _healthHandler.RemoveHealthChangeEvent(OnHealthChanged);
        }

        private void OnHealthChanged()
        {
            _setCurrentHealthEvent.Raise(_healthHandler.CurrentHealth);
        }
    }
}