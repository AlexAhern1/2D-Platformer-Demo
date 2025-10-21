using UnityEngine;

namespace Game
{
    [System.Serializable]
    public abstract class AttackCondition : ICondition
    {
        public abstract bool Evaluate();
    }

    [System.Serializable]
    public class AllConditionsArray : ICondition
    {
        [SerializeReference, SubclassSelector]
        public ICondition[] Conditions;

        public bool Evaluate()
        {
            for (int i = 0; i < Conditions.Length; i++)
            {
                if (!Conditions[i].Evaluate()) return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class AnyConditionsArray : ICondition
    {
        [SerializeReference, SubclassSelector]
        public ICondition[] Conditions;

        public bool Evaluate()
        {
            for (int i = 0; i < Conditions.Length; i++)
            {
                if (Conditions[i].Evaluate()) return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class InRangeCondition : AttackCondition
    {
        public DetectorHub Detectors;
        public int DetectorID;

        public TargetHandler Targets;
        public int TargetID;

        public bool CheckIfInside = true;

        public override bool Evaluate()
        {
            bool inRange = Detectors.DoesTargetMatchWith(Targets.GetTarget(TargetID), DetectorID);
            return inRange == CheckIfInside;
        }
    }

    [System.Serializable]
    public class CooldownCondition : AttackCondition
    {
        public CooldownsHandler Cooldowns;
        public int CooldownID;

        public override bool Evaluate()
        {
            return Cooldowns.IsReady(CooldownID);
        }
    }

    [System.Serializable]
    public class HealthCondition : AttackCondition
    {
        public NPCHealthHandler HealthHandler;
        [Range(0, 1)] public float Ratio;
        //public NumericOperator Operator;
        public ComparisonOperator Comparer;

        public override bool Evaluate() => Comparer.Evaluate(HealthHandler.CurrentHealthRatio, Ratio);
    }


    [System.Serializable]
    public class LineOfSightCondition : AttackCondition
    {
        public Transform RaycastStart;
        public TargetHandler Targets;
        public int TargetHolderID;
        public LayerMask Layer;

        // readonly
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[2];

        // cached stuff
        private Vector2 _start;
        private Vector2 _end;

        public override bool Evaluate()
        {
            _start = RaycastStart.position;
            _end = Targets.GetTarget(TargetHolderID).transform.position;

            int hitCount = Physics2D.LinecastNonAlloc(_start, _end, _hits, Layer);

            // debugging
            Color lineColor = hitCount > 1 ? Color.red : Color.green;
            Logger.DrawLine(_start, _end, Time.deltaTime, lineColor);

            return hitCount == 1;
        }
    }

    [System.Serializable]
    public class ResourceCondition : AttackCondition
    {
        public AgentResourceHandler Resources;
        public int ResourceID;

        [Header("Comparing config")]
        public bool CompareWithScaledTime;
        public float CompareValue;
        //public NumericOperator Operator;

        public ComparisonOperator Comparer;

        public override bool Evaluate()
        {
            float actualCompareValue = CompareWithScaledTime ? Time.time : CompareValue;
            return Comparer.Evaluate(Resources.Get(ResourceID), actualCompareValue);
        }
    }

    [System.Serializable]
    public class ReactionCondition : AttackCondition
    {
        public NPCReactionHandler Reactions;
        public int ReactionID;

        public override bool Evaluate() => Reactions.CanReact(ReactionID);
    }
}