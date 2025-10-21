namespace Game.Enemy.AI
{
    public sealed class Root : Node //root will only have one child, which will be evaluated continuously.
    {
        private Node _child;
        private Node[] _childArray;

        protected override Node[] Children => _childArray;

        public Root(string name) : base(name) { }

        /// <summary>
        /// A root can only have one child.
        /// </summary>
        /// <param name="child"></param>
        public void SetChild(Node child)
        {
            _child = child;
            _childArray = new Node[1] { child };
        }

        public override Status OnEvaluate()
        {
            status = _child.Evaluate();
            return status;
        }
    }
}