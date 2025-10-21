namespace Game
{
    public class SceneTransitionContext
    {
        public SceneField NewCentralScene;
        public SceneField[] ScenesToLoad;

        public SceneField OldCentralScene;
        public SceneField[] ScenesToUnload;

        public void SetScenesToUnload(SceneField center, SceneField[] adjacent)
        {
            Logger.Log("set scenes to unload start");
            OldCentralScene = center;
            ScenesToUnload = adjacent;
            Logger.Log("set scenees to unload end");
        }

        public void ClearScenesToUnload()
        {
            OldCentralScene = null;
            ScenesToUnload = null;
        }
    }
}