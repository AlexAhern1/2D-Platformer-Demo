using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Serializable]
    public class SmoothSceneTransition : ISceneTransitionStrategy
    {
        public async void ExecuteTransition(SceneTransitionContext context)
        {
            var toUnload = context.ScenesToUnload;
            var newCenter = context.NewCentralScene;
            var toLoad = context.ScenesToLoad;
            var oldCenter = context.OldCentralScene;

            // unload scenes that are loaded.
            Logger.Log("unloading started");
            await Unload(toUnload, toLoad, newCenter);
            Logger.Log("unloading finished");

            Logger.Log("loading start");
            // load scenes that are unloaded.
            await Load(toLoad, oldCenter);
            Logger.Log("unloading finished");
        }

        private async Task UnloadIfLoaded(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) return;
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);

            while (!op.isDone) await Task.Yield();
        }

        private async Task LoadIfUnloaded(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) return;
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!op.isDone) await Task.Yield();
        }

        private async Task Unload(SceneField[] toUnload, SceneField[] toLoad, SceneField center)
        {
            List<Task> unloadTasks = new();

            for (int i = 0; i < toUnload.Length; i++)
            {
                var scene = toUnload[i];

                if (scene.Equals(center) || toLoad.Contains(scene)) continue;
                unloadTasks.Add(UnloadIfLoaded(scene));
            }

            await Task.WhenAll(unloadTasks);
        }

        private async Task Load(SceneField[] toLoad, SceneField center)
        {
            List<Task> loadTasks = new();

            for (int i = 0; i < toLoad.Length; i++)
            {
                var scene = toLoad[i];

                if (scene.Equals(center)) continue;
                loadTasks.Add(LoadIfUnloaded(scene));
            }

            await Task.WhenAll(loadTasks);
        }
    }
}