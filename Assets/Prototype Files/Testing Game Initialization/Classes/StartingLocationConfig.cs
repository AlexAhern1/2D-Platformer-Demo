using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StartingLocationConfig
    {
        public string Description;

        public Vector2 Spawnpoint;
        public SceneField CentralScene;
        public SceneField[] AdjacentScenes;
    }
}