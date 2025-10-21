namespace Game
{
    [System.Serializable]
    public class FloatInputCondition : ICondition
    {
        public FloatInputEvent InputEvent;
        public bool RequiresRelease;

        public bool Evaluate()
        {
            float value = InputEvent.InputValue;
            return (RequiresRelease && value == 0) || (!RequiresRelease && value > 0);
        }
    }
}