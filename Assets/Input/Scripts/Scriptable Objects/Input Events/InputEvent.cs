namespace Game
{
    public abstract class InputEvent<T> : GameEvent<T>
    {
        public T InputValue { get; protected set; }

        public override void Raise(T value)
        {
            InputValue = value;
            base.Raise(value);
        }
    }
}