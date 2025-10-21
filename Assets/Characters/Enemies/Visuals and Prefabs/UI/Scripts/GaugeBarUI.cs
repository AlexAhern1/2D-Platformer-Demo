using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GaugeBarUI : MonoBehaviour
    {
        [SerializeField] private Image _barFill;

        private float _maxValue;
        private float _minValue;
        private float _rangeInverse;

        public void SetMinAndMaxValues(float min, float max)
        {
            _maxValue = max;
            _minValue = min;
            _rangeInverse = 1f / (max - min);
        }

        public void SetCurrentValue(float value)
        {
            _barFill.fillAmount = Mathf.Clamp((value - _minValue) * _rangeInverse, _minValue, _maxValue);
        }

        public void SetMaxValue(float newMaxValue)
        {
            _barFill.fillAmount = _barFill.fillAmount.LinearMap(_minValue, newMaxValue, _minValue, _maxValue);
            
            _rangeInverse = 1f / (newMaxValue - _minValue);
            _maxValue = newMaxValue;
        }
    }
}