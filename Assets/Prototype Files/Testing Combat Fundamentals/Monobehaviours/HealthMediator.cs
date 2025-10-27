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
        // toggling boss health bar will be done in a manager script.

        [SerializeField] private Vector2Event _initializeHealthEvent;
        [SerializeField] private FloatEvent _setCurrentHealthEvent;
        [SerializeField] private BoolEvent _toggleHealthBarEvent;

        [SerializeField] private NPCHealthHandler _healthHandler;

        public override void Enable()
        {
            _initializeHealthEvent.Raise(new Vector2(0, _healthHandler.MaxHealth));
            _setCurrentHealthEvent.Raise(_healthHandler.CurrentHealth);

            _healthHandler.AddHealthChangeEvent(OnHealthChanged);

            _toggleHealthBarEvent.Raise(true);
        }

        public override void Disable()
        {
            _healthHandler.RemoveHealthChangeEvent(OnHealthChanged);
            _toggleHealthBarEvent.Raise(false);
        }

        private void OnHealthChanged()
        {
            _setCurrentHealthEvent.Raise(_healthHandler.CurrentHealth);
        }
    }
}