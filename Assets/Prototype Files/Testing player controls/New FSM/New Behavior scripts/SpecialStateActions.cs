using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class SetStaticAttackDirection : IStateAction
    {
        [Header("Transforms")]
        public Transform DirectionParent;

        [Header("Direction getters")]

        [SerializeReference, SubclassSelector]
        public IFloatGetter HorizontalDirectionGetter;

        [SerializeReference, SubclassSelector]
        public IFloatGetter VerticalDirectionGetter;

        public void DoAction()
        {
            float v = VerticalDirectionGetter.GetFloat();

            if (v != 0) DirectionParent.rotation = Quaternion.Euler(0, 0, 90f * Mathf.Sign(v));
            else DirectionParent.rotation = Quaternion.Euler(0, 90f * (1f - HorizontalDirectionGetter.GetFloat()), 0);
        }
    }

    [System.Serializable]
    public class OrientateAction : IStateAction
    {
        public CharacterObjectOrientator Orientator;

        [SerializeReference, SubclassSelector]
        public IFloatGetter DirectionGetter;

        public void DoAction()
        {
            Orientator.Orientate(DirectionGetter.GetFloat());
        }
    }

    [System.Serializable]
    public class ComponentAction : IStateAction
    {
        public MonoBehaviour Behaviour;

        public void DoAction()
        {
            if (Behaviour is IStateAction behaviourAction) behaviourAction.DoAction();
            else Logger.Warn($"Attached Component {Behaviour} is not a state action!", MoreColors.LightOrange);
        }
    }

    [System.Serializable]
    public class SetDynamicGravityScaleAction : IStateAction
    {
        public PlayerMovementPhysics Physics;

        [SerializeReference, SubclassSelector]
        public IFloatGetter Getter;

        public void DoAction()
        {
            Physics.SetGravityScaleSetter(Getter);
        }
    }

    [System.Serializable]
    public class QuickFallFloatGetter : IFloatGetter
    {
        public float NormalGravityScale;
        public float QuickFallGravityScale;
        public FloatInputEvent InputEvent;
        public PlayerMovementPhysics Physics;

        public float GetFloat()
        {
            if (InputEvent.InputValue == 0 && Physics.Velocity.y > 0) return QuickFallGravityScale;
            else return NormalGravityScale;
        }
    }

    [System.Serializable]
    public class SetResourceAction : IStateAction
    {
        public AgentResourceHandler Resources;

        [SerializeReference, SubclassSelector]
        public ResourceSetter Setter;

        public void DoAction()
        {
            Setter.Set(Resources);
        }
    }

    [System.Serializable]
    public class ConditionalAction : IStateAction
    {
        [Separator]
        [SerializeReference, SubclassSelector]
        public IStateAction Action;

        [Separator]
        [SerializeReference, SubclassSelector]
        public ICondition ActionCondition;

        public void DoAction()
        {
            if (ActionCondition.Evaluate()) Action.DoAction();
        }
    }

    [System.Serializable]
    public class ToggleAction : IStateAction
    {
        [SerializeReference, SubclassSelector]
        public IToggle ToggleObject;
        public bool ToggleValue;

        public void DoAction() => ToggleObject.Toggle(ToggleValue);
    }

    [System.Serializable]
    public class SetTimeEventAction : IStateAction
    {
        public GameEvent TimeEvent;
        public Timer Timer;

        [SerializeReference, SubclassSelector]
        public IFloatGetter Seconds;

        public void DoAction()
        {
            Timer.StartTimer(Seconds.GetFloat(), TimeEvent.Raise);
        }
    }

    [System.Serializable]
    public class TransitionWithCleanupAction : IStateAction
    {
        [Separator]
        [Header("Transition context")]
        public StateMachine StateMachine;
        public TransitionContext Context;

        [Separator]
        [Header("Cleanup action")]
        [SerializeReference, SubclassSelector]
        public IStateAction CleanupAction;

        public void DoAction()
        {
            var condition = Context.Condition;

            if (condition == null || condition.Evaluate())
            {
                CleanupAction.DoAction();
                StateMachine.DoTransition(Context.Target);
            }
        }
    }
}