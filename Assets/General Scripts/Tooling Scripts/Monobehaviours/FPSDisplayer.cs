using UnityEngine;
using TMPro;

namespace Game
{
    public class FPSDisplayer : MonoBehaviour
    {
        [SerializeField] TMP_Text textBox;
        [SerializeField] float updateRate;

        private float _updatePeriod;
        private float _nextDisplayTime;

        private void Awake()
        {
            _updatePeriod = 1f / updateRate;
            _nextDisplayTime = Time.time + _updatePeriod;
        }

        private void Update()
        {
            if (Time.time < _nextDisplayTime) return;
            _nextDisplayTime += _updatePeriod;

            textBox.text = $"FPS: {Mathf.RoundToInt(1f / Time.unscaledDeltaTime)}";
        }
    }
}