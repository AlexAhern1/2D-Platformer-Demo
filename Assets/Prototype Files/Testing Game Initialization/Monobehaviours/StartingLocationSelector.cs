using System.Collections;
using UnityEngine;

namespace Game
{
    public class StartingLocationSelector : MonoBehaviour
    {
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private float _delaySeconds;
        [SerializeField] private float _fadeOutSeconds;
        [SerializeField] private CanvasGroup _startingLocationCanvasGroup;
        [SerializeField] private BlackScreenUI _blackScreenOverlay;
        [SerializeField] private GameObject _blackScreenBackground;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private GameInitiator _levelsInitiator;
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private float _delayBeforeShowingLevel;
        [SerializeField] private float _levelFadeInDuration;
        [SerializeField] private GameEvent _enableLevelsInputEvent;
        [SerializeField] private GameObject[] _HUDelements;

        [Header("Starting locations")]
        [SerializeField] private StartingLocationConfig[] Locations;

        private bool _levelsLoaded;

        private void Awake()
        {
            _levelsLoaded = false;
        }

        public async void LoadStartingLocation(int ID)
        {
            // disable input
            _disableInputEvent.Raise();

            // wait for delay (to let UI animation finish)
            await AsyncHelpers.Wait(_delaySeconds);

            // fade out location selector UI (and disable gameobject)
            StartCoroutine(FadeOutScreen(_fadeOutSeconds));
            await AsyncHelpers.Wait(_fadeOutSeconds);

            // set black screen alpha to 1
            _blackScreenBackground.SetActive(false);
            _blackScreenOverlay.SetAlpha(1);

            var startingLocation = Locations[ID];

            // set the initial scenes for scene loader.
            _sceneLoader.SetInitialScenes(startingLocation.CentralScene, startingLocation.AdjacentScenes);

            // need to keep track of if the levels initializer is enabled or disabled.
            _levelsLoaded = true;

            // place the player at spawn position
            _playerObject.transform.position = startingLocation.Spawnpoint;

            // initialize other stuff.
            _levelsInitiator.Initialize(); //note - this loads the initial scenes (might not be ideal as we may want to wait for all scenes to load first)
            _levelsInitiator.Enable();

            foreach (var obj in _HUDelements)
            {
                obj.SetActive(true);
            }

            await AsyncHelpers.Wait(_delayBeforeShowingLevel);

            _blackScreenOverlay.FadeOut(_levelFadeInDuration);
            await AsyncHelpers.Wait(_levelFadeInDuration);

            _enableLevelsInputEvent.Raise();
        }

        public void UnloadCurrentScenes()
        {
            // this is called when unloading scenes to return to the main menu.
            Logger.Log("unloading scenes before returning to main menu.");

            _levelsLoaded = false;
        }

        private void OnDestroy()
        {
            if (_levelsLoaded)
            {
                _levelsLoaded = false;
                Logger.Log("editor-cleanup must be done here.");
                _levelsInitiator.Disable();
            }
        }

        private IEnumerator FadeOutScreen(float duration)
        {
            float elapsedTime = 0;
            float durationReciprocal = 1f / duration;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _startingLocationCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime * durationReciprocal);
                yield return null;
            }

            _startingLocationCanvasGroup.alpha = 0;
            _startingLocationCanvasGroup.gameObject.SetActive(false);

            yield return null;
        }
    }
}