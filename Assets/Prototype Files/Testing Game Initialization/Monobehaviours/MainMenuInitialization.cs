using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class MainMenuInitialization : MonoBehaviour, IInitializable
    {
        [Header("Main menu canvas groups")]
        [SerializeField] private CanvasGroup _titleCanvasGroup;
        [SerializeField] private CanvasGroup _startButtonCanvasGroup;

        [Header("Title timings")]
        [SerializeField] private float _titleAlphaChangeStartTime;
        [SerializeField] private float _titleAlphaChangeDuration;

        [Header("Start button timings")]
        [SerializeField] private float _buttonAlphaChangeStartTime;
        [SerializeField] private float _buttonAlphaChangeDuration;

        [Header("Input Handling")]
        [SerializeField] private InputEvent<float> _anyInputEvent;
        [SerializeField] private UIObject _anyButtonObject;

        [Header("Overall canvas groups")]
        [SerializeField] private CanvasGroup _gameTitleCanvasGroup;
        [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
        [SerializeField] private CanvasGroup _spawnLocationCanvasGroup;
        [SerializeField] private CanvasGroup _controlsCanvasGroup;
        [SerializeField] private CanvasGroup _quitGameCanvasGroup;

        [Header("Black screen")]
        [SerializeField] private GameObject _blackScreenBackground;

        // other canvas groups: // spawnpoint selection

        // state variables
        private bool _canPressAnyButton;

        public void Initialize()
        {
            _blackScreenBackground.SetActive(true);

            _gameTitleCanvasGroup.alpha = 1;
            _gameTitleCanvasGroup.gameObject.SetActive(true);

            _mainMenuCanvasGroup.alpha = 0;
            _mainMenuCanvasGroup.gameObject.SetActive(false);

            _spawnLocationCanvasGroup.alpha = 0;
            _spawnLocationCanvasGroup.gameObject.SetActive(false);

            _controlsCanvasGroup.alpha = 0;
            _controlsCanvasGroup.gameObject.SetActive(false);

            _quitGameCanvasGroup.alpha = 0;
            _quitGameCanvasGroup.gameObject.SetActive(false);

            _titleCanvasGroup.alpha = 0;
            _startButtonCanvasGroup.alpha = 0;

            _canPressAnyButton = false;

            StartCoroutine(InitializationCoroutine());
        }

        private IEnumerator InitializationCoroutine()
        {
            yield return new WaitForSeconds(_titleAlphaChangeStartTime);

            float elapsedTime = 0;
            float durationReciprocal = 1f / _titleAlphaChangeDuration;

            while (elapsedTime < _titleAlphaChangeDuration)
            {
                elapsedTime += Time.deltaTime;
                _titleCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime * durationReciprocal);
                yield return null;
            }

            yield return new WaitForSeconds(Mathf.Max(0, _buttonAlphaChangeStartTime - _titleAlphaChangeStartTime - _titleAlphaChangeDuration));

            _titleCanvasGroup.alpha = 1;

            elapsedTime = 0;
            durationReciprocal = 1f / _buttonAlphaChangeDuration;

            while (elapsedTime < _buttonAlphaChangeDuration)
            {
                elapsedTime += Time.deltaTime;
                _startButtonCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime * durationReciprocal);
                yield return null;
            }

            _startButtonCanvasGroup.alpha = 1;

            _canPressAnyButton = true;
            _anyInputEvent.AddEvent(OnPressAnyKey);

            yield return null;
        }

        private void OnDisable()
        {
            if (_canPressAnyButton)
            {
                _anyInputEvent.RemoveEvent(OnPressAnyKey);
                _canPressAnyButton = false;
            }
        }

        private void OnPressAnyKey(float value)
        {
            if (value == 0) return;
            _canPressAnyButton = false;
            _anyInputEvent.RemoveEvent(OnPressAnyKey);

            _anyButtonObject.Select();
        }
    }
}