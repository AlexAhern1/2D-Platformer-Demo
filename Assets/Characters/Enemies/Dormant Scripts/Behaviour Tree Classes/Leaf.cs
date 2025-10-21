using System;

namespace Game.Enemy.AI
{
    public abstract class Leaf : Node
    {
        private Action _resetCallback;

        protected override Node[] Children => null;

        public Leaf(string name, Action resetCallback = null) : base(name)
        {
            _resetCallback = resetCallback;
        }

        public Leaf() : base("") { }

        public override void Reset()
        {
            _resetCallback?.Invoke();
        }
    }

    public sealed class ConditionLeaf : Leaf
    {
        private Func<bool> _conditionCallback;

        public ConditionLeaf(string name, Func<bool> conditionCallback) : base(name)
        {
            _conditionCallback = conditionCallback;
        }

        public ConditionLeaf(Func<bool> conditionCallback)
        {
            _conditionCallback = conditionCallback;
        }

        public override Status OnEvaluate()
        {
            bool result = _conditionCallback();
            status = result ? Status.Success : Status.Failure;
            return status;
        }
    }

    public sealed class CallbackLeaf : Leaf
    {
        private Action _callback;

        public CallbackLeaf(string name, Action callback) : base(name)
        {
            _callback = callback;
        }

        public CallbackLeaf(Action callback)
        {
            _callback = callback;
        }

        public override Status OnEvaluate()
        {
            _callback?.Invoke();
            status = Status.Success;
            return status;
        }
    }

    /// <summary>
    /// does nothing but return a fixed state.
    /// </summary>
    public sealed class Constant : Leaf
    {
        public Constant(Status constantStatus) : base($"CONSTANT")
        {
            status = constantStatus;
        }

        public override Status OnEvaluate()
        {
            return status;
        }
    }
}