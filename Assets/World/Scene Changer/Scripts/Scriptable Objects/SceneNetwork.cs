using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Scene Network", menuName = "SO/Data/Scene Network")]
    public class SceneNetwork : ScriptableObject
    {
        [Header("Central Scene")]
        public SceneField CentralScene;

        [Header("Adjacent Scenes")]
        public List<SceneField> AdjacentScenes;

        public string[] AdjacentScenesArray => AdjacentScenes.Select(scene => scene.SceneName).ToArray();

        public List<string> GetAllScenes()
        {
            List<string> sceneNames = AdjacentScenes.Select(scene => scene.SceneName).ToList();
            sceneNames.Add(CentralScene.SceneName);
            return sceneNames;
        }
    }
}