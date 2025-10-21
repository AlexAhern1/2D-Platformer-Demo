namespace Game
{
    public interface ISceneTransitionStrategy
    {
        public void ExecuteTransition(SceneTransitionContext context);
    }
}