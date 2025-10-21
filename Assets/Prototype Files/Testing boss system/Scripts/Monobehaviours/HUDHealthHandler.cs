using UnityEngine;

namespace Game
{
    public class HUDHealthHandler : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent _toggleBarEvent;
        [SerializeField] private Vector2Event _initializeEvent;
        [SerializeField] private FloatEvent _healthChangeEvent;

        [Header("UI objects")]
        [SerializeField] private GameObject _barUIObject;

        [Header("UI components")]
        [SerializeField] private GaugeBarUI _playerHealthBarUI;

        private void OnEnable()
        {
            _toggleBarEvent.AddEvent(OnToggleBar);
            _initializeEvent.AddEvent(OnInitializeHealthValues);
            _healthChangeEvent.AddEvent(OnHealthChange);
        }

        private void OnDisable()
        {
            _toggleBarEvent.RemoveEvent(OnToggleBar);
            _initializeEvent.RemoveEvent(OnInitializeHealthValues);
            _healthChangeEvent.RemoveEvent(OnHealthChange);
        }

        private void OnToggleBar(bool value)
        {
            _barUIObject.SetActive(value);
        }

        private void OnInitializeHealthValues(Vector2 values)
        {
            float min = values.x;
            float max = values.y;

            _playerHealthBarUI.SetMinAndMaxValues(min, max);
        }

        private void OnHealthChange(float health)
        {
            _playerHealthBarUI.SetCurrentValue(health);
        }
    }
}