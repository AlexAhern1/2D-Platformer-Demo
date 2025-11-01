using System.Collections;
using UnityEngine;

namespace Game
{
    public class SecretRoom : MonoBehaviour
    {
        [Header("Collision config")]
        [SerializeField] private Tag _playerTag;

        [Header("Fading config")]
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private SpriteRenderer _fadingCover;

        private float _currentAlpha;
        private int _currentFadeDirection; // either 1 or -1
        private bool _isFading;

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            _currentAlpha = 1;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag)) return;

            _currentFadeDirection = -1;

            if (_fadeCoroutine == null)
            {
                _isFading = true;
                _fadeCoroutine = StartCoroutine(FadeSecretRoom());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag)) return;

            _currentFadeDirection = 1;

            if (_fadeCoroutine == null)
            {
                _isFading = true;
                _fadeCoroutine = StartCoroutine(FadeSecretRoom());
            }
        }

        private IEnumerator FadeSecretRoom()
        {
            var c = _fadingCover.color;

            while (_isFading)
            {
                _currentAlpha = Mathf.Clamp01(_currentAlpha + Time.deltaTime * _fadeSpeed * _currentFadeDirection);
                _fadingCover.color = new Color(c.r, c.g, c.b, _currentAlpha);

                if (_currentFadeDirection == 1 && _currentAlpha >= 1)
                {
                    _isFading = false;
                    _fadeCoroutine = null;
                    yield break;

                }
                else if (_currentFadeDirection == -1 && _currentAlpha <= 0)
                {
                    _isFading = false;
                    _fadeCoroutine = null;
                    yield break;
                }

                yield return null;
            }
        }
    }
}