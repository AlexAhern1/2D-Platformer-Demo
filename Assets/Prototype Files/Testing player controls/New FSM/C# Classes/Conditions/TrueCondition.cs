namespace Game
{
    [System.Serializable]
    public class TrueCondition : ICondition
    {
        public bool Evaluate() => true;
    }
}