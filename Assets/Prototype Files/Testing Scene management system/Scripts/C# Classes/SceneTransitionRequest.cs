namespace Game
{
    public class SceneTransitionRequest
    {
        /// <summary>
        /// <para>This should only be called at the initialization stage of any class that can make transition requests.</para>
        /// Any actual requests should reuse the same instance instead of creating a new one per request made.
        /// </summary>
        public SceneTransitionRequest(SceneField CentralScene, SceneField[] adjacentScenes, ISceneTransitionStrategy strategy)
        {
            // partially filled context here, as the remaining data gets filled when an actual request gets made.
            Context = new()
            {
                NewCentralScene = CentralScene,
                ScenesToLoad = adjacentScenes,
            };

            Strategy = strategy;
        }

        public SceneTransitionContext Context;
        public ISceneTransitionStrategy Strategy;

        public void Process()
        {
            Logger.Log("execute strategy start");
            Strategy.ExecuteTransition(Context);
            Logger.Log("execute strategy stop");
        }
    }
}