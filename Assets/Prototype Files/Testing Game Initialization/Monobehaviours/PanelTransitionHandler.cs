using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class PanelTransitionHandler : MonoBehaviour
    {
        // this gets called when a UI button is pressed, where a panel needs to be unloaded, followed by another panel getting loaded.
        [Header("Game Events")]
        [SerializeField] private GameEvent _disableCurrentInputEvent;
        [SerializeField] private GameEvent _enableMenusInputEvent;
        [SerializeField] private UIControlSchemeEvent _controlSchemeEvent;

        public PanelTransitionMode[] Modes;

        public void Transition(int ID)
        {
            // check if valid id first.

            PanelTransitionMode mode = Modes[ID];
            StartCoroutine(DoTransition(mode));
        }

        private IEnumerator DoTransition(PanelTransitionMode mode)
        {
            // disable input map
            _disableCurrentInputEvent.Raise();

            // wait for delay before unloading
            yield return new WaitForSeconds(mode.FadeOutDelay);

            // fade out current canvas group
            float duration = mode.FadeOutDuration;
            float elapsedTime = 0;
            CanvasGroup canvas = mode.CurrentCanvas;

            float durationReciprocal = 1f / duration;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(1, 0, elapsedTime * durationReciprocal);
                yield return null;
            }
            canvas.alpha = 0;
            canvas.gameObject.SetActive(false);

            // wait for delay
            yield return new WaitForSeconds(mode.FadeInDelay);

            duration = mode.FadeInDuration;
            elapsedTime = 0;
            canvas = mode.TargetCanvas;

            canvas.alpha = 0;
            canvas.gameObject.SetActive(true);

            // fade in
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(0, 1, elapsedTime * durationReciprocal);
                yield return null;
            }
            canvas.alpha = 1;

            // wait for fade in before enabling input map.
            _enableMenusInputEvent.Raise();

            // set ui control scheme
            _controlSchemeEvent.Raise(mode.ControlScheme);

            // need - UNITY EVENT to initialize the target panel.
            mode.TransitionCompleteEvent.Invoke();

            yield return null;
        }
    }

    [Serializable]
    public class PanelTransitionMode
    {
        public string Description;

        [Header("Canvas groups")]
        public CanvasGroup CurrentCanvas;
        public CanvasGroup TargetCanvas;

        [Header("Timing")]
        public float FadeOutDelay;
        public float FadeOutDuration;
        public float FadeInDelay;
        public float FadeInDuration;

        [Header("Control Scheme")]
        [SerializeReference, SubclassSelector]
        public UIControlScheme ControlScheme;

        [Header("Events")]
        public UnityEvent TransitionCompleteEvent;
    }
}