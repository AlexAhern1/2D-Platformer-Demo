using System.Collections.Generic;
using System.Linq;

namespace Game.Enemy.AI
{
    public abstract class Composite : Node
    {
        protected Composite(string name) : base(name) { }

        protected Composite() : base("") { }
    }

    public class Sequence : Composite
    {
        //sequence - runs through the list of its children (ON ONE TICK). key idea: behave like a logical AND statement.

        //if all children return success, the sequence returns success.
        //if any one child returns failure, the sequence returns failure.
        //evaulate that child first on the next tick until it either succeeds or fails.

        protected override Node[] Children => _children.ToArray();

        private List<Node> _children;
        private int _firstFailedChildIndex;

        public Sequence(string name, params Node[] children) : base($"SEQUENCE: {name}")
        {
            _children = new List<Node>(children);
        }

        public Sequence(params Node[] children) : base("SEQUENCE")
        {
            _children = new List<Node>(children);
        }

        public override Status OnEvaluate()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                Status childStatus = _children[i].Evaluate();

                switch (childStatus)
                {
                    case Status.Success:
                        //successful child: continue looping
                        continue;

                    case Status.Failure:
                        //failed child: reset running child index, set status to failure and return status
                        _firstFailedChildIndex = i;
                        //Reset();
                        status = Status.Failure;
                        return status;

                    case Status.Running:
                        //running child: set the running child index to the current index in loop, set status to running and return status
                        status = Status.Running;
                        return status;
                }
            }

            //if for loop manages to reach this point, that means all children nodes returned success.
            //Reset();
            status = Status.Success;
            return status;
        }

        public override void Reset()
        {
            if (_firstFailedChildIndex == 0)
            {

                for (int i = 0; i < _children.Count; i++)
                {
                    _children[i].Reset();
                }
            }
            else
            {
                for (int i = 0; i < _firstFailedChildIndex + 1; i++)
                {
                    _children[i].Reset();
                }
            }
            _firstFailedChildIndex = 0;
        }
    }

    public class Selector : Composite
    {
        //selector - runs through the list of its children (ON ONE TICK). key idea: behave like a logical OR statement.

        //if all children return failure, the selector returns failure.
        //if any one child returns success, the selector returns success.
        //if a child returns running, evaluate that child first on the next tick until it either succeeds or fails.

        protected override Node[] Children => _children.ToArray();

        private int _firstSucceededChildIndex;
        private List<Node> _children;

        public Selector(string name, params Node[] children) : base($"SELECTOR: {name}")
        {
            _children = new List<Node>(children);
        }

        public Selector(params Node[] children) : base("SELECTOR")
        {
            _children = new List<Node>(children);
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        public override Status OnEvaluate()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                Status childStatus = _children[i].Evaluate();

                switch (childStatus)
                {
                    case Status.Success:
                        //successful child: reset running child index, set status to success and return status.
                        _firstSucceededChildIndex = i;
                        //Reset();
                        status = Status.Success;
                        return status;

                    case Status.Failure:
                        //failed child: continue looping
                        continue;

                    case Status.Running:
                        //running child: set the running child index to the current index in loop, set status to running and return status
                        status = Status.Running;
                        return status;
                }
            }

            //if for loop manages to reach this point, that means all children nodes returned failure.
            //Reset();
            status = Status.Failure;
            return status;
        }

        public override void Reset()
        {
            if (_firstSucceededChildIndex == 0)
            {
                //reset all children
                for (int i = 0; i < _children.Count; i++)
                {
                    _children[i].Reset();
                }
            }
            else
            {
                //reset all children up to the first successful child
                for (int i = 0; i < _firstSucceededChildIndex + 1; i++)
                {
                    _children[i].Reset();
                }
            }
            _firstSucceededChildIndex = 0;
        }
    }

    public class Library : Composite
    {
        private Dictionary<string, Node> _children;

        protected override Node[] Children => _children.Values.ToArray();

        public Library(string name) : base($"LIBRARY: {name}")
        {
            _children = new Dictionary<string, Node>();
        }

        //state variable
        private string _key;

        public override Status OnEvaluate()
        {
            if (!_children.ContainsKey(_key) || _key == "")
            {
                Logger.Error($"{_key} is not a valid key!");
                status = Status.Failure;
            }
            else
            {
                status = _children[_key].Evaluate();
            }
            return status;
        }

        public void AddChild(string key, Node child)
        {
            if (_children.ContainsKey(key))
            {
                Logger.Warn($"{key} already contained in dictionary!");
                return;
            }
            _children.Add(key, child);
        }

        public Library WithChild(string key, Node child)
        {
            AddChild(key, child);
            return this;
        }

        public void SetKey(string key)
        {
            _key = key;
        }
    }
}