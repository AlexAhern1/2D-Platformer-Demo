namespace Game
{
    public interface ICondition
    {
        public bool Evaluate();
    }

    public interface ICondition<T>
    {
        public bool Evaluate(T t);
    }
}