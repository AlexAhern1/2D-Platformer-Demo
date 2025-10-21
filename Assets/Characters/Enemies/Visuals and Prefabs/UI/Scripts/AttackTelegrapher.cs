using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// debugging
    /// </summary>
    public class AttackTelegrapher : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Image _fillBar;
        [SerializeField] private RectTransform _entireTelegraphBar;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _flashColor;

        [SerializeField] private float _flashDuration;
        private float _flashDurationInverse;

        [SerializeField] private bool _enableTelegraph;

        //state variables
        private bool _telegraphing;
        private Coroutine _telegraphCoroutine;

        private void Awake()
        {
            _flashDurationInverse = 1f / _flashDuration;
            _fillBar.fillAmount = 0f;

            if (_enableTelegraph) { _entireTelegraphBar.gameObject.SetActive(true); return; }
            _entireTelegraphBar.gameObject.SetActive(false);
        }

        public void Telegraph(float seconds)
        {
            if (_telegraphing || !gameObject.activeSelf) return;
            else if (_telegraphCoroutine != null)
            {
                StopCoroutine(_telegraphCoroutine);
                _telegraphCoroutine = null;
            }
            _telegraphCoroutine = StartCoroutine(DoTelegraph(seconds));
        }

        public void Stop()
        {
            if (!_telegraphing) return;

            _telegraphing = false;
            StopCoroutine(_telegraphCoroutine);
            _fillBar.fillAmount = 0f;
        }

        private IEnumerator DoTelegraph(float seconds)
        {
            _telegraphing = true;
            float secondsInverse = 1f / seconds;

            float elapsedTime = 0f;
            while (elapsedTime < seconds)
            {
                elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, seconds);

                _fillBar.fillAmount = elapsedTime * secondsInverse;
                yield return null;
            }

            _telegraphing = false;
            elapsedTime = 0f;
            while (elapsedTime < _flashDuration)
            {
                elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, _flashDuration);

                _fillBar.fillAmount = 1f - elapsedTime * _flashDurationInverse;
                _fillBar.color = Color.Lerp(_defaultColor, _flashColor, _fillBar.fillAmount);
                yield return null;
            }
            _telegraphCoroutine = null;
        }
    }
}