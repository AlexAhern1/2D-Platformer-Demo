using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class SceneField : System.IEquatable<SceneField>
    {
        [SerializeField] private Object _sceneAsset;

        [SerializeField] private string _sceneName;

        public string SceneName { get { return _sceneName; } set { _sceneName = value; } }

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField._sceneName;
        }

        public bool Equals(SceneField other) => SceneName == other._sceneName;
    }
}