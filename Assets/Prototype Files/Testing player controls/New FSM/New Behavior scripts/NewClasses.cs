using System;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    public class StateBehaviour
    {
        public string Description;

        [Separator(3)]
        [Header("Action Selection")]
        [SerializeReference, SubclassSelector]
        public IStateAction Action;

        [Separator(3)]
        [Header("Trigger Selection")]
        [SerializeReference, SubclassSelector]
        public ActionTrigger Trigger;
        
        public void Enter()
        {
            Trigger?.Start(OnTrigger);
        }

        public void Exit()
        {
            Trigger?.Stop(OnTrigger);
        }

        private void OnTrigger()
        {
            Action.DoAction();
        }
    }

    #region Actions

    public interface IStateAction
    {
        public void DoAction();
    }

    [Serializable]
    public class StateActionArray : IStateAction
    {
        [Separator]
        [Header("Actions")]
        [SerializeReference, SubclassSelector]
        public IStateAction[] Actions;

        public void DoAction()
        {
            for (int i = 0; i < Actions.Length; i++) Actions[i].DoAction();
        }
    }

    [Serializable]
    public class PhysicsAction : IStateAction
    {
        [Header("Physics Component")]
        public PlayerMovementPhysics PhysicsHandler;

        [Header("Physics Manipulation")]
        [SerializeReference, SubclassSelector]
        public PhysicsManipulator Manipulator;

        public void DoAction()
        {
            Manipulator.DoPhysicsAction(PhysicsHandler);
        }
    }

    #region physics manipulation types

    [Serializable]
    public abstract class PhysicsManipulator
    {
        public abstract void DoPhysicsAction(PlayerMovementPhysics physics);
    }

    [Serializable]
    public class PhysicsManipulatorArray : PhysicsManipulator
    {
        [SerializeReference, SubclassSelector]
        public PhysicsManipulator[] Manipulators;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            for (int i = 0; i < Manipulators.Length; i++) Manipulators[i].DoPhysicsAction(physics);
        }
    }

    [Serializable]
    public class HorizontalVelocityManipulator : PhysicsManipulator
    {
        [Separator]
        [Header("Speed")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter SpeedGetter;

        [Separator]
        [Header("Direction")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter DirectionGetter;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            physics.MoveHorizontal(SpeedGetter.GetFloat() * DirectionGetter.GetFloat(), true);
        }
    }

    [Serializable]
    public class VerticalVelocityManipulator : PhysicsManipulator
    {
        [Separator]
        [Header("Speed Data")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter SpeedGetter;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            physics.MoveVertical(SpeedGetter.GetFloat(), true);
        }
    }

    [Serializable]
    public class HorizontalVelocityCurveManipulator : PhysicsManipulator
    {
        [Separator]
        [Header("Speed")]
        public AnimationCurve NormalizedHorizontalVelocityCurve;
        public float Duration;
        [SerializeReference, SubclassSelector]
        public IFloatGetter InitialSpeedGetter;

        [Separator]
        [Header("Direction")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter DirectionGetter;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            float durationInverse = 1f / Duration;
            float func(float t) => InitialSpeedGetter.GetFloat() * DirectionGetter.GetFloat() * NormalizedHorizontalVelocityCurve.Evaluate(t * durationInverse);

            physics.OverrideHorizontalBaseVelocity(func, Duration);
        }
    }

    [Serializable]
    public class VelocityCurveManipulator : PhysicsManipulator
    {
        [Separator]
        [Header("Velocity Curve Data")]
        public AnimationCurve VelocityCurve;
        public float Duration;

        [Separator]
        [Header("Initial Velocity Data")]
        public bool UseInitialVelocity;
        public Vector2 InitialVelocity;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            Vector2 initialVelocity = UseInitialVelocity ? InitialVelocity : physics.Velocity;

            float durationInverse = 1f / Duration;

            physics.OverrideBaseVelocity((float t) => initialVelocity * VelocityCurve.Evaluate(t * durationInverse), Duration);
        }
    }

    [Serializable]
    public class PhysicsSettingManipulator : PhysicsManipulator
    {
        private const float DEFAULT_VALUE = -1;

        [Separator]
        [Header("Gravity Scale")]
        public bool SetGravityScale;
        public float GravityScaleValue;

        [Separator]
        [Header("Max Fall Speed")]
        public bool SetMaxFallSpeed;
        public float MaxFallSpeedValue;

        [Separator]
        [Header("Falling")]
        public bool ToggleFalling;
        public bool FallingValue;

        [Separator]
        [Header("Overriding")]
        public bool CancelOverridingBaseVelocioty;

        public override void DoPhysicsAction(PlayerMovementPhysics physics)
        {
            if (SetGravityScale)
            {
                if (GravityScaleValue == DEFAULT_VALUE)
                {
                    physics.ResetGravityScale();
                }
                else
                {
                    physics.SetGravityScale(GravityScaleValue);
                }
            }

            if (SetMaxFallSpeed)
            {
                if (MaxFallSpeedValue == DEFAULT_VALUE)
                {
                    physics.ResetMaxFallSpeed();
                }
                else
                {
                    physics.SetMaxFallSpeed(MaxFallSpeedValue);
                }
            }

            if (ToggleFalling)
            {
                physics.ToggleFalling(FallingValue);
            }

            if (CancelOverridingBaseVelocioty)
            {
                physics.CancelOverride();
            }
        }
    }


    #endregion

    [Serializable]
    public class StateTransitionAction : IStateAction
    {
        [Separator]
        [Header("Components")]
        public StateMachine StateMachine;

        [Separator]
        [Header("Transition Handler Algorithm")]
        [SerializeReference, SubclassSelector]
        public TransitionHandler Handler;

        public void DoAction()
        {
            var target = Handler.GetTargetState();
            if (target != null) StateMachine.DoTransition(target);
        }
    }

    #region transition types

    [Serializable]
    public abstract class TransitionHandler
    {
        public abstract PlayerState GetTargetState();
    }

    [Serializable]
    public class SingleTargetTransition : TransitionHandler
    {
        public TransitionContext Context;

        public override PlayerState GetTargetState()
        {
            var transitionCondition = Context.Condition;

            return (transitionCondition == null || transitionCondition.Evaluate()) ? Context.Target : null;
        }
    }

    [Serializable]
    public class AbilityFinishTransition : TransitionHandler
    {
        [Header("Components")]
        public CollisionHandler Collisions;
        public Transform ForwardTransform;
        public Transform ForwardParent;
        public PlayerMovementPhysics Physics;

        [Header("Data Assets")]
        public Vector2InputEvent MoveInput;

        [Header("States")]
        public PlayerState IdleState;
        public PlayerState WalkingState;
        public PlayerState TurnState;
        public PlayerState AirborneIdleState;
        public PlayerState AirborneMovingState;
        public PlayerState AirborneTurnState;
        public PlayerState WalledState;

        [Header("Restrictions")]
        public bool CanTransitionIntoWalled;

        public override PlayerState GetTargetState()
        {
            bool touchingGround = Collisions.IsColliding(Vector2.down);
            bool touchingWalls = Collisions.IsColliding(Vector2.left) || Collisions.IsColliding(Vector2.right);

            if (touchingWalls && !touchingGround && CanTransitionIntoWalled)
            {
                Physics.ToggleFalling(true);

                return WalledState;
            }
            return touchingGround ? GetGroundedState() : GetAirborneState();
        }

        private PlayerState GetGroundedState()
        {
            Physics.ToggleFalling(false);
            float inputValue = MoveInput.InputValue.x;

            // check if the horizontal movement input is pressed. 
            if (inputValue == 0) return IdleState;

            // check if the facing direction and input value are the same.
            float facingDirection = Mathf.Sign(ForwardTransform.position.x - ForwardParent.position.x);
            if (facingDirection != Mathf.Sign(inputValue)) return TurnState;
            return WalkingState;
        }

        private PlayerState GetAirborneState()
        {
            Physics.ToggleFalling(true);
            float inputValue = MoveInput.InputValue.x;

            if (inputValue == 0) return AirborneIdleState;

            float facingDirection = Mathf.Sign(ForwardTransform.position.x - ForwardParent.position.x);
            if (facingDirection != Mathf.Sign(inputValue)) return AirborneTurnState;
            return AirborneMovingState;
        }
    }

    [Serializable]
    public class MultiTargetTransition : TransitionHandler
    {
        public TransitionContext[] Contexts;

        public override PlayerState GetTargetState()
        {
            for (int i = 0; i < Contexts.Length; i++)
            {
                var t = Contexts[i];
                var condition = t.Condition;

                if (condition == null || condition.Evaluate()) return t.Target;
            }
            return null;
        }
    }

    //  start attack state: needs to start a timer, while the 6 core substates need to wait for that timer (but not start it on enter)
    //  idea 1: create a transitionhandler class that waits before transitioning.
    //  problem: need to handle interruptions, so the cancellation callback must be known to all other scripts.

    //  idea 2: create a dictionary of timers for the timer, which other classes can access 

    //  note: for other abilities like the downward laser, the states don't rely on a timer, but instead an input EVENT.
    //  specifically, in any of the downward laser substates, listen for input -> transition.

    //  can we do the same for the timer?
    //  possibility: give the attack an end event.
    //  in any of the attack substates, listen for that event.




    [Serializable]
    public class TransitionContext
    {
        public string Description;

        public PlayerState Target;

        [SerializeReference, SubclassSelector]
        public ICondition Condition;
    }

    #endregion

    [Serializable]
    public class LogMessageAction : IStateAction
    {
        public string Message;
        public Color Color;

        public void DoAction()
        {
            Logger.Log(Message, Color);
        }
    }

    #endregion

    #region Triggers

    [Serializable]
    public abstract class ActionTrigger
    {
        protected Action callback;

        public virtual void Start(Action callback) { }

        public virtual void Stop(Action callback) { }
    }

    [Serializable]
    public class EnterActionTrigger : ActionTrigger
    {
        public override void Start(Action callback) => callback.Invoke();
    }

    [Serializable]
    public class ExitActionTrigger : ActionTrigger
    {
        public override void Stop(Action callback) => callback.Invoke();
    }

    [Serializable]
    public class WaitActionTrigger : ActionTrigger
    {
        [Separator]
        [Header("Components")]
        public Timer Timer;

        [Separator]
        [Header("Timing data")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter WaitSecondsGetter;

        // state variable
        private Action _timerCancellationCallback;

        public override void Start(Action callback)
        {
            _timerCancellationCallback = Timer.StartTimer(WaitSecondsGetter.GetFloat(), callback);
        }

        public override void Stop(Action callback)
        {
            _timerCancellationCallback?.Invoke();
        }
    }

    [Serializable]
    public class CollisionActionTrigger : ActionTrigger
    {
        [Header("Collision Handler")]
        public CollisionHandler Collisions;

        [Header("Required collision data")]
        public Vector2 Direction;
        public bool IsEnter;
        public bool UseBothHorizontal;

        public override void Start(Action callback)
        {
            this.callback = callback;
            Collisions.AddCollisionEvent(OnCollide);
        }

        public override void Stop(Action callback)
        {
            this.callback = null;
            Collisions.RemoveCollisionEvent(OnCollide);
        }

        private void OnCollide(CollisionData data)
        {
            if (!CheckDirection(data.Direction) || data.Enter != IsEnter) return;
            callback.Invoke();
        }

        private bool CheckDirection(Vector2 dir)
        {
            if (UseBothHorizontal) return dir.x != 0;
            else return dir == Direction;
        }
    }

    [Serializable]
    public class GameEventActionTrigger : ActionTrigger
    {
        public GameEvent Event;

        public override void Start(Action callback)
        {
            this.callback = callback;
            Event.AddEvent(OnEventRaised);
        }

        public override void Stop(Action callback)
        {
            this.callback = null;
            Event.RemoveEvent(OnEventRaised);
        }

        private void OnEventRaised()
        {
            callback.Invoke();
        }
    }

    [Serializable]
    public class InputTrigger : ActionTrigger
    {
        [Header("Input Trigger")]
        [SerializeReference, SubclassSelector]
        public InputTriggerType Type;

        public override void Start(Action callback)
        {
            Type.EnableInputs(callback);
        }

        public override void Stop(Action callback)
        {
            Type.DisableInputs(callback);
        }
    }

    #region input trigger types

    [Serializable]
    public abstract class InputTriggerType
    {
        protected Action onInputSuccessCallback;

        public abstract void EnableInputs(Action callback);

        public abstract void DisableInputs(Action callback);
    }

    [Serializable]
    public class FloatInputTrigger : InputTriggerType
    {
        [Separator]
        [Header("Input Event")]
        public FloatInputEvent InputEvent;

        [Separator]
        [Header("Input Config")]
        [Tooltip("If set to false, action callback will be invoked on button PRESSED.")]
        public bool InvokeOnRelease;

        public override void EnableInputs(Action callback)
        {
            onInputSuccessCallback = callback;
            InputEvent.AddEvent(OnReceiveInput);
        }

        public override void DisableInputs(Action callback)
        {
            onInputSuccessCallback = null;
            InputEvent.RemoveEvent(OnReceiveInput);
        }

        private void OnReceiveInput(float inputValue)
        {
            if (ProcessInputValue(inputValue)) onInputSuccessCallback?.Invoke();
        }

        private bool ProcessInputValue(float inputValue)
        {
            return (inputValue > 0 && !InvokeOnRelease) || (inputValue == 0 && InvokeOnRelease);
        }
    }

    [Serializable]
    public class Vector2InputTrigger : InputTriggerType
    {
        [Separator]
        [Header("Input Event")]
        public Vector2InputEvent InputEvent;
        public bool ReadInputOnEnter = true;

        [Separator]
        [Header("Input Arithmetic")]
        public bool UseVerticalAxis;
        public bool UseAbsoluteValue;
        public ComparisonOperator Comparer;
        public float ComparisonValue;

        public override void EnableInputs(Action callback)
        {
            onInputSuccessCallback = callback;
            InputEvent.AddEvent(OnReceiveInput);

            if (ReadInputOnEnter) OnReceiveInput(InputEvent.InputValue);
        }

        public override void DisableInputs(Action callback)
        {
            onInputSuccessCallback = null;
            InputEvent.RemoveEvent(OnReceiveInput);
        }

        private void OnReceiveInput(Vector2 inputValue)
        {
            if (ProcessInputValue(inputValue)) onInputSuccessCallback?.Invoke();
        }

        private bool ProcessInputValue(Vector2 inputValue)
        {
            float axisValue = UseVerticalAxis ? inputValue.y : inputValue.x;
            if (UseAbsoluteValue) axisValue = Mathf.Abs(axisValue);

            return Comparer.Evaluate(axisValue, ComparisonValue);
        }
    }

    #endregion

    #endregion
}