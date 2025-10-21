using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneLoader : MonoBehaviour
    {
        // this class should only be in the persistent scene.

        [Header("SO Events")]
        [SerializeField] private SceneTransitionRequestEvent _requestEvent;

        // state variables
        [SerializeField] private SceneField _currentCentralScene;
        [SerializeField] private SceneField[] _currentAdjacentScenes;

        [Header("Editor Config")]
        [SerializeField] private bool _ignore;

        private void Awake()
        {
            if (_ignore) return;
            LoadInitialScenes();
        }

        private void OnEnable()
        {
            _requestEvent.AddEvent(OnTransitionRequestReceived);
        }

        private void OnDisable()
        {
            _requestEvent.RemoveEvent(OnTransitionRequestReceived);
        }

        private void LoadInitialScenes()
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
            Logger.Log("request received");
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