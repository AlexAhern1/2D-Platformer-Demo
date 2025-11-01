using UnityEngine;

namespace Game.UI
{
    public class PauseHandler : MonoBehaviour, IEnable
    {
        [SerializeField] private InputEvent<float> _pauseInputEvent;
        [SerializeField] private GameEvent _enableMenusInputEvent;
        [SerializeField] private GameEvent _enableLevelsInputEvent;
        [SerializeField] private GameEvent _enableTitleInputEvent;
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private UIPanelGraph _pauseGraph;
        [SerializeField] private UIControlSchemeEvent _controlSchemeEvent;
        [SerializeField] private float _delayBeforeResuming;
        [SerializeField] private SceneLoader _sceneHandler;
        [SerializeField] private GameObject _playerGameObject;
        [SerializeField] private GameInitiator _levelsInitator;
        [SerializeField] private GameObject _blackScreenBackground;
        [SerializeField] private MainMenuInitialization _mainMenu;

        [SerializeField] private GameObject[] _objectsToDisableOnExit;

        [SerializeReference, SubclassSelector] private UIControlScheme _controlScheme;

        private bool _paused;

        public void Enable()
        {
            _pauseInputEvent.AddEvent(OnPressPause);
        }

        public void Disable()
        {
            _pauseInputEvent.RemoveEvent(OnPressPause);
        }

        private void OnDestroy()
        {
            if (_paused)
            {
                Time.timeScale = 1f;
            }
        }

        private void OnPressPause(float inputData)
        {
            if (inputData == 0) return;

            _paused = true;

            // set timescale to 0 (all pause animations use unscaled delta time)
            Time.timeScale = 0;

            // switch to menus input map
            _enableMenusInputEvent.Raise();

            // two ui buttons appear (left(default): unpause, right: main menu)
            _pauseGraph.gameObject.SetActive(true);
            _pauseGraph.InitializeDefaultObject();

            // update ui control scheme
            _controlSchemeEvent.Raise(_controlScheme);

            // if press unpause, just unpause the game. if press right, return to main menu (without extra prompt)

        }

        public async void OnPressResume()
        {
            // wait for a delay before resuming.

            await AsyncHelpers.WaitUnscaled(_delayBeforeResuming);

            // set timescale to 1.
            Time.timeScale = 1;

            _enableLevelsInputEvent.Raise();

            _pauseGraph.gameObject.SetActive(false);

            _controlSchemeEvent.Raise(null);
        }

        public void OnPressMainMenu()
        {
            // disable input
            _disableInputEvent.Raise();

            // fade in the black screen (using unscaled time) (TODO. for now, just set it to inactive.
            _pauseGraph.gameObject.SetActive(false);

            // disable ui control scheme.
            _controlSchemeEvent.Raise(null);

            // set timescale to 1 
            Time.timeScale = 1;

            _playerGameObject.SetActive(false);

            // unload everything level-related
            _sceneHandler.UnloadAllScenes();

            // disable some gameobjects
            foreach (var obj in _objectsToDisableOnExit)
            {
                obj.SetActive(false);
            }

            // after some steps, need to call level game manager.disable.
            _levelsInitator.Disable();

            // enable the black screen background
            _blackScreenBackground.SetActive(true);

            // then here need to get the title screen to animate in again.
            _mainMenu.Initialize();

            // enable title input map
            _enableTitleInputEvent.Raise();
        }
    }
}