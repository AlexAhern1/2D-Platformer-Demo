using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class GameQuitter : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private float _fadeOutDelay;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _quitDelay;

        [Header("Canvas group")]
        [SerializeField] private CanvasGroup _quitScreenCanvasGroup;

        [Header("Events")]
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private UnityEvent _disableGameInitiationEvent;

        public void DoQuitGameSequence()
        {
            _disableInputEvent.Raise();
            _disableGameInitiationEvent.Invoke();
            StartCoroutine(QuitSequence());
        }

        private IEnumerator QuitSequence()
        {
            yield return new WaitForSeconds(_fadeOutDelay);

            float elapsedTime = 0;
            float durationReciprocal = 1f / _fadeOutDuration;

            while (elapsedTime < _fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                _quitScreenCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime * durationReciprocal);
                yield return null;
            }
            _quitScreenCanvasGroup.alpha = 0;

            yield return new WaitForSeconds(_quitDelay);

            Application.Quit();
            Logger.Log("Quitted out of game.", MoreColors.LightRed);

            yield return null;
        }
    }
}