using System.Collections.Generic;

namespace Game.Enemy.AI
{
    public abstract class Node
    {
        //debugging fields
        public readonly string Name;
        public bool WasTicked = false;

        //important fields
        protected Status status = Status.Failure;

        protected abstract Node[] Children { get; }

        public Node(string name)
        {
            Name = name;
        }

        public Status Evaluate()
        {
            WasTicked = true;
            return OnEvaluate();
        }

        public abstract Status OnEvaluate();

        public virtual void Reset() { }

        #region debugging

        public virtual string DebugTreeActivity(int depth)
        {
            //get own activity
            string nodeActivity = GetColoredText($"{GetDepthString(depth)}{Name}");
            string childActivity = "";

            //check if this node was ticked.
            if (WasTicked) { WasTicked = false; }
            if (Children == null) return nodeActivity;

            //get children activity
            foreach (Node child in Children)
            {
                //first child: just add child'a debugtreeactivity
                childActivity += "\n" + child.DebugTreeActivity(depth + 1);
            }

            return nodeActivity + childActivity;
        }

        private static string GetDepthString(int depth)
        {
            string depthRepresentation = "";
            for (int i = 0; i < depth; i++)
            {
                depthRepresentation += "   ";
            }
            return depthRepresentation;
        }

        private string GetColoredText(string text)
        {
            if (!WasTicked) return $"<color=grey>{text}</color>";

            switch (status)
            {
                case Status.Success:
                    return $"<color=green>{text} ({status})</color>";
                case Status.Failure:
                    return $"<color=red>{text} ({status})</color>";
                case Status.Running:
                    return $"<color=yellow>{text} ({status})</color>";
                default: return text;
            }
        }

        #endregion
    }
}