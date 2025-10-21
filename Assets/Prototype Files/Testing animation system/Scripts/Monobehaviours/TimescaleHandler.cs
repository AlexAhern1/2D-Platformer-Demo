using UnityEngine;

namespace Game
{
    public class TimescaleHandler : MonoBehaviour
    {
        private float _waitEndTime;

        private void Update()
        {
            if (Time.unscaledTime < _waitEndTime) return;

            Time.timeScale = 1;
            enabled = false;
        }

        private void OnDisable()
        {
            // in case unscaled time never gets pass wait end time in Update.
            if (Time.timeScale != 1) Time.timeScale = 1;
        }

        public void SetTimescale(float scale, float duration)
        {
            enabled = true;
            _waitEndTime = Time.unscaledTime + duration;

            Time.timeScale = scale;
        }
    }
}