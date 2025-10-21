using System;

namespace Game.Enemy.AI
{
    public abstract class Decorator : Node
    {
        protected Node child;
        private Node[] _childArray;

        protected override Node[] Children => _childArray;

        public Decorator(string name, Node child) : base(name)
        {
            this.child = child;
            _childArray = new Node[1] { child };
        }

        public override void Reset()
        {
            child.Reset();
        }
    }

    /// <summary>
    /// Inverts the status of the child node unless it returns running, in which case this node also returns running.
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(Node child) : base("INVERTER", child) { }

        public override Status OnEvaluate()
        {
            Status childStatus = child.Evaluate();
            switch (childStatus)
            {
                case Status.Success:
                    status = Status.Failure;
                    break;
                case Status.Failure:
                    status = Status.Success;
                    break;
                default:
                    status = Status.Running;
                    break;
            }

            return status;
        }
    }

    /// <summary>
    /// evaluates the child node but always returns a fixed status.
    /// </summary>
    public class Fixator : Decorator
    {
        public Fixator(Node child, Status fixedStatus) : base(GetNameBasedOnStatus(fixedStatus), child)
        {
            status = fixedStatus;
        }

        public override Status OnEvaluate()
        {
            child.Evaluate();
            return status;
        }

        private static string GetNameBasedOnStatus(Status status)
        {
            switch (status)
            {
                case Status.Success: return "SUCCESSOR";
                case Status.Failure: return "DISAPPOINTMENT";
                case Status.Running: return "RUNNER";
                default: throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }

    /// <summary>
    /// Returns Success only when the child returns Success, otherwise returns Running.
    /// </summary>
    public class UntilSuccess : Decorator
    {
        public UntilSuccess(Node child) : base("UNTIL SUCCESS", child) { }

        public override Status OnEvaluate()
        {
            if (child.OnEvaluate() == Status.Success) status = Status.Success;
            else status = Status.Running;
            return status;
        }
    }
}