using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneLoader : MonoBehaviour, IEnable, IInitializable
    {
        // this class should only be in the persistent scene.

        [Header("SO Events")]
        [SerializeField] private SceneTransitionRequestEvent _requestEvent;

        // state variables
        [SerializeField] private SceneField _currentCentralScene;
        [SerializeField] private SceneField[] _currentAdjacentScenes;

        [SerializeField] private bool _initializeOnAwake;

        private void Awake()
        {
            if (_initializeOnAwake) LoadInitialScenes();
        }

        public void Initialize()
        {
            Logger.Log("SCENE MANAGER INITIALIZED", MoreColors.Emerald);
            LoadInitialScenes();
        }

        public void SetInitialScenes(SceneField center, SceneField[] adjacents)
        {
            _currentCentralScene = center;
            _currentAdjacentScenes = adjacents;
        }

        public void Enable()
        {
            Logger.Log("SCENE MANAGER ENABLED", MoreColors.LimeGreen);
            _requestEvent.AddEvent(OnTransitionRequestReceived);
        }

        public void Disable()
        {
            Logger.Log("SCENE MANAGER DISABLED", MoreColors.PaleTurquoise);
            _requestEvent.RemoveEvent(OnTransitionRequestReceived);
        }

        public void LoadInitialScenes()
        {
            // loading central scene
            LoadSceneIfUnloaded(_currentCentralScene);

            for (int i = 0;  i < _currentAdjacentScenes.Length; i++)
            {
                LoadSceneIfUnloaded(_currentAdjacentScenes[i]);
            }
        }

        private void OnTransitionRequestReceived(SceneTransitionRequest request)
        {
            var context = request.Context;

            // need to check: if the request is to load the same central scene as the current one (which implies also the same adjacnet ones),
            // then do not process the request.
            if (context.NewCentralScene == _currentCentralScene) return;

            // populate the request's context with the remaining data.
            context.SetScenesToUnload(_currentCentralScene, _currentAdjacentScenes);

            // update the current scenes.
            _currentCentralScene = context.NewCentralScene;
            _currentAdjacentScenes = context.ScenesToLoad;

            // context is ready - perform state transition.
            request.Process(); //(this is an asynchronous process)
        }

        private void LoadSceneIfUnloaded(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}