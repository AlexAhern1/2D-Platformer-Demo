using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BlackScreenUI : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;

        [Header("Events (TO DELEGATE TO ANOTHER CLASS)")]
        [SerializeField] private FloatEvent _fadeInEvent;
        [SerializeField] private FloatEvent _fadeOutEvent;

        private float _fadeSpeed;

        private float _alpha;
        private bool _isFading;

        private void Awake()
        {
            _isFading = false;
            _alpha = 0f;
        }

        private void OnEnable()
        {
            _fadeInEvent.AddEvent(OnFadeIn);
            _fadeOutEvent.AddEvent(OnFadeOut);
        }

        private void OnDisable()
        {
            _fadeInEvent.RemoveEvent(OnFadeIn);
            _fadeOutEvent.RemoveEvent(OnFadeOut);
        }

        private void OnFadeIn(float seconds)
        {
            FadeIn(seconds);
        }

        private void OnFadeOut(float seconds)
        {
            FadeOut(seconds);
        }

        public void FadeIn(float seconds)
        {
            _fadeSpeed = 1f / seconds;
            if (_isFading) return;
            StartCoroutine(Fading());
        }

        public void FadeOut(float seconds)
        {
            _fadeSpeed = -1f / seconds;
            if (_isFading) return;
            StartCoroutine(Fading());
        }

        public void SetAlpha(float alpha)
        {
            var col = _blackScreen.color;
            col.a = alpha;
            _alpha = alpha;
            _blackScreen.color = col;
        }

        private IEnumerator Fading()
        {
            _isFading = true;
            Color c;

            //change the fade amount (increase or decrease) until it either reaches 0 or 1.
            while (_isFading)
            {
                _alpha += _fadeSpeed * Time.deltaTime;

                if (_alpha <= 0f)
                {
                    _isFading = false;
                    _alpha = 0f;
                }
                else if (_alpha >= 1f)
                {
                    _isFading = false;
                    _alpha = 1f;
                }

                c = _blackScreen.color;
                _blackScreen.color = new Color(c.r, c.b, c.g, _alpha);

                yield return null;
            }
        }
    }
}