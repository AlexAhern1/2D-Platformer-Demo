using UnityEngine;

namespace Game
{
    [System.Serializable]
    public abstract class StateBehaviour
    {
        public virtual void FindReferences() { }

        public virtual void Start() { }

        public virtual void Stop() { }

        public virtual void FixedUpdate(float normalizedTime) { }
    }

    [System.Serializable]
    public abstract class PhysicsBehaviour : StateBehaviour
    {
        public Rigidbody2D Rb2d;
    }

    #region physics behaviours

    [System.Serializable]
    public class PatrolPhysicsBehaviour : PhysicsBehaviour
    {
        [Header("Physics Config")]
        public float Speed;
        public float Radius;
        public float Epsilon;

        //state data
        public Transform Forward;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState TransitionOnReachTarget;
        public NPCState TransitionOnTooFarAway;

        private bool _centerSet;
        private float _centerX;
        private float _currentTargetX;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        public override void Start()
        {
            if (!_centerSet)
            {
                _centerSet = true;
                _centerX = Rb2d.transform.position.x;
            }

            else if ((Rb2d.position.x > _centerX + Radius && FacingDirection > 0) 
            || (Rb2d.position.x < _centerX - Radius) && FacingDirection < 0)
            {
                TransitionHandler.DoTransition(TransitionOnTooFarAway);
            }

            _currentTargetX = _centerX + FacingDirection * Radius;
        }

        public override void Stop()
        {
            Rb2d.SetHorizontalSpeed(0);
        }

        public override void FixedUpdate(float normalizedTime)
        {
            Logger.DrawCircle(new Vector2(_currentTargetX, Rb2d.position.y), Epsilon * 5f, Time.fixedDeltaTime, Color.green);

            Rb2d.SetHorizontalSpeed(FacingDirection * Speed);
            if (Mathf.Abs(Rb2d.position.x - _currentTargetX) < Epsilon)
            {
                TransitionHandler.DoTransition(TransitionOnReachTarget);
            }
        }
    }

    [System.Serializable]
    public class TurnBehaviour : PhysicsBehaviour
    {
        public AnimationHandler Animator;
        public AnimationContext TurnLeftClip;
        public AnimationContext TurnRightClip;
        public float Duration;

        public Transform Forward;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnTurnCompleteTransition;

        private float _endTime;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        public override void Start()
        {
            if (FacingDirection > 0)
            {
                Animator.Play(TurnLeftClip);
            }
            else if (FacingDirection < 0)
            {
                Animator.Play(TurnRightClip);
            }

            _endTime = Time.time + Duration;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _endTime) return;
            TransitionHandler.DoTransition(OnTurnCompleteTransition);
        }
    }

    [System.Serializable]
    public class FaceTargetBehaviour : PhysicsBehaviour
    {
        [Header("Facing Direction")]
        public Transform Forward;

        [Header("Target Settings")]
        public TargetHandler Targets;
        public int TargetHolderID;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState TransitionStateOnNotFacingTarget;

        // state variables
        private Transform _targetTransform;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        private float DirectionToTarget => Mathf.Sign(_targetTransform.position.x - Rb2d.position.x);

        public override void Start()
        {
            _targetTransform = Targets.GetTarget(TargetHolderID).transform;
            CheckDirectionToTarget();
        }

        public override void FixedUpdate(float normalizedTime)
        {
            CheckDirectionToTarget();
        }

        private void CheckDirectionToTarget()
        {
            if (FacingDirection == DirectionToTarget) return;
            TransitionHandler.DoTransition(TransitionStateOnNotFacingTarget);
        }
    }

    [System.Serializable]
    public class ChaseBehaviour : PhysicsBehaviour
    {
        public float ChaseSpeed;
        public Transform Forward;

        public TargetHandler Targets;
        public int TargetHolderID;

        private Transform _chaseTarget;

        private float DirectionToTarget => Mathf.Sign(_chaseTarget.position.x - Rb2d.position.x);

        public override void Start()
        {
            _chaseTarget = Targets.GetTarget(TargetHolderID).transform;
        }

        public override void Stop()
        {
            Rb2d.SetHorizontalSpeed(0);
        }

        public override void FixedUpdate(float normalizedTime)
        {
            Rb2d.SetHorizontalSpeed(DirectionToTarget * ChaseSpeed);
        }
    }

    [System.Serializable]
    public class StrafeBehaviour : PhysicsBehaviour
    {
        public float Speed;
        public float StrafingRange;
        public float BackoffRange;
        public float Epsilon;

        public Transform Forward;
        public TargetHandler Targets;
        public int TargetHolderID;

        //state data
        private float _strafeGoal;
        private Transform _strafeTarget;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        public override void Start()
        {
            _strafeTarget = Targets.GetTarget(TargetHolderID).transform;

            float strafeCenterDistFromPlayer = BackoffRange + StrafingRange;
            float strafeCenter = _strafeTarget.position.x - FacingDirection * strafeCenterDistFromPlayer;

            _strafeGoal = Random.Range(strafeCenter - StrafingRange, strafeCenter + StrafingRange);
        }

        public override void Stop()
        {
            Rb2d.SetHorizontalSpeed(0);
        }

        public override void FixedUpdate(float normalizedTime)
        {
            var pos = Rb2d.position;
            float y1 = pos.y - 0.5f;
            float y2 = pos.y - 0.25f;

            float enemyX = pos.x;
            float playerX = _strafeTarget.position.x;

            float strafeCenterDistFromPlayer = BackoffRange + StrafingRange;
            float strafeCenter = playerX - FacingDirection * strafeCenterDistFromPlayer;

            var p = new Vector2(strafeCenter + StrafingRange, y1);

            // strafing zone
            Logger.DrawLine(new Vector2(strafeCenter - StrafingRange, y1), p, Time.fixedDeltaTime, Color.yellow);

            // backoff zone
            Logger.DrawLine(p, new Vector2(playerX, y1), Time.fixedDeltaTime, Color.red);

            // close enough to target
            if (Mathf.Abs(enemyX - _strafeGoal) < Epsilon)
            {
                // update strafe target to a random point
                _strafeGoal = Random.Range(strafeCenter - StrafingRange, strafeCenter + StrafingRange);
            }

            // enter backoff zone
            else if (Mathf.Abs(2 * enemyX - playerX - (strafeCenter + FacingDirection * StrafingRange)) < BackoffRange)
            {
                // update strafe target to be the center
                _strafeGoal = strafeCenter;
            }

            Rb2d.SetHorizontalSpeed(Speed * Mathf.Sign(_strafeGoal - enemyX));
            Logger.DrawCircle(new Vector2(_strafeGoal, y2), 0.25f, Time.fixedDeltaTime, Color.green);
        }
    }

    [System.Serializable]
    public class HorizontalMovementBehaviour : PhysicsBehaviour
    {
        public AnimationCurve SpeedCurve;
        public float StartTime;
        public bool IgnoreGravity;

        [Header("Direction Config")]
        public Transform Forward;

        [Header("Targetting")]
        public TargetHandler Targets;
        public int TargetID;
        public bool UseTarget;

        // state variables
        private float _trueStartTime;
        private float _savedGravityScale;
        private float _horizontalDirection;

        public override void Start()
        {
            _trueStartTime = Time.time + StartTime;

            if (UseTarget)
            {
                _horizontalDirection = Mathf.Sign(Targets.GetTarget(TargetID).transform.position.x - Rb2d.position.x);
            }
            else
            {
                _horizontalDirection = Mathf.Sign(Forward.position.x - Rb2d.position.x);
            }


            if (IgnoreGravity)
            {
                _savedGravityScale = Rb2d.gravityScale;
                Rb2d.gravityScale = 0;
            }
        }

        public override void Stop()
        {
            Rb2d.SetHorizontalSpeed(0);
            if (IgnoreGravity)
            {
                Rb2d.gravityScale = _savedGravityScale;
            }
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _trueStartTime) return;
            float elapsedTime = Time.time - _trueStartTime;

            float speed = SpeedCurve.Evaluate(elapsedTime);
            Rb2d.SetHorizontalSpeed(speed * _horizontalDirection);
        }
    }

    [System.Serializable]
    public class VerticalMovementBehaviour : PhysicsBehaviour
    {
        public AnimationCurve SpeedCurve;
        public float StartTime;
        public bool IgnoreGravity;
        public bool StopVerticalMovementOnStart;
        public bool StopVerticalMovementOnStop;

        // state variables
        private float _trueStartTime;
        private float _savedGravityScale;

        public override void Start()
        {
            _trueStartTime = Time.time + StartTime;

            if (StopVerticalMovementOnStart) Rb2d.SetVerticalSpeed(0);
            if (IgnoreGravity)
            {
                _savedGravityScale = Rb2d.gravityScale;
                Rb2d.gravityScale = 0;
            }
        }

        public override void Stop()
        {
            if (StopVerticalMovementOnStop) Rb2d.SetVerticalSpeed(0);
            if (IgnoreGravity)
            {
                Rb2d.gravityScale = _savedGravityScale;
            }
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _trueStartTime) return;
            float elapsedTime = Time.time - _trueStartTime;

            float speed = SpeedCurve.Evaluate(elapsedTime);
            Rb2d.SetVerticalSpeed(speed);
        }
    }

    [System.Serializable]
    public class JumpBehaviour : PhysicsBehaviour
    {
        public float JumpHeight;
        public float JumpDelay;

        // states
        private bool _jumped;

        public override void Start()
        {
            _jumped = false;
            if (JumpDelay == 0)
            {
                Jump();
            }
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (_jumped || normalizedTime < JumpDelay) return;
            Jump();
        }

        private void Jump()
        {
            _jumped = true;
            Rb2d.SetVerticalSpeed(JumpHeight);
        }
    }

    [System.Serializable]
    public class FixedDashBehaviour : PhysicsBehaviour
    {
        public AnimationCurve SpeedCurve;
        public float StartTime;
        public bool IgnoreGravity;
        public float HorizontalDashDirection;
        public float VerticalDashDirection;
        public Transform Forward;

        // state variables
        private float _trueStartTime;
        private float _savedGravityScale;
        private Vector2 _dashDirection;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        public override void Start()
        {
            _trueStartTime = Time.time + StartTime;

            if (IgnoreGravity)
            {
                _savedGravityScale = Rb2d.gravityScale;
                Rb2d.gravityScale = 0;
            }

            _dashDirection = new Vector2(FacingDirection * HorizontalDashDirection, VerticalDashDirection).normalized;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _trueStartTime) return;
            float elapsedTime = Time.time - _trueStartTime;

            float speed = SpeedCurve.Evaluate(elapsedTime);
            Rb2d.velocity = speed * _dashDirection;
        }
    }

    [System.Serializable]
    public class TargetedDashBehaviour : PhysicsBehaviour
    {
        public AnimationCurve SpeedCurve;
        public float StartTime;
        public bool IgnoreGravity;
        public Transform Forward;

        [Header("Targetting")]
        public TargetHandler Targets;
        public int TargetID;

        [Range(0, 90)] public float AngleRange;
        public float HorizontalAimOffset;

        // state variables
        private float _trueStartTime;
        private float _savedGravityScale;
        private Vector2 _dashDirection;

        private float FacingDirection => Mathf.Sign(Forward.position.x - Rb2d.position.x);

        public override void Start()
        {
            _trueStartTime = Time.time + StartTime;

            if (IgnoreGravity)
            {
                _savedGravityScale = Rb2d.gravityScale;
                Rb2d.gravityScale = 0;
            }

            SetDashDirection();
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _trueStartTime) return;
            float elapsedTime = Time.time - _trueStartTime;

            float speed = SpeedCurve.Evaluate(elapsedTime);
            Rb2d.velocity = speed * _dashDirection;
        }

        private void SetDashDirection()
        {
            Vector2 curr = Rb2d.position;
            Vector2 targ = Targets.GetTarget(TargetID).transform.position;

            Vector2 aimPosition = targ + FacingDirection * HorizontalAimOffset * Vector2.right;
            float angleToPlayer = Mathf.Atan2(aimPosition.y - curr.y, aimPosition.x - curr.x);

            float velocityAngle;

            if (FacingDirection > 0)
            {
                float a = (-90f + AngleRange) * Mathf.Deg2Rad;
                Logger.DrawLine(curr, curr + 100f * new Vector2(Mathf.Cos(a), Mathf.Sin(a)), 5f, Color.cyan);
                velocityAngle = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -90f, AngleRange - 90f) * Mathf.Deg2Rad;
            }
            else
            {
                float a = (-90f - AngleRange) * Mathf.Deg2Rad;
                Logger.DrawLine(curr, curr + 100f * new Vector2(Mathf.Cos(a), Mathf.Sin(a)), 5f, Color.cyan);
                velocityAngle = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -90f - AngleRange, -90f) * Mathf.Deg2Rad;
            }

            _dashDirection = new Vector2(Mathf.Cos(velocityAngle), Mathf.Sin(velocityAngle));

        }
    }

    #endregion

    [System.Serializable]
    public class DetectBehaviour : StateBehaviour
    {
        [Header("Timing Config")]
        public bool Always;
        [Min(0)] public float StartTime;
        [Min(0)] public float StopTime;

        [Header("Detect Config")]
        public DetectorHub Hub;
        public int DetectorID;
        public bool CheckIfInside = true;

        [Header("Targetting Config")]
        public TargetHandler Targets;
        public int TargetHolderID;
        public bool SaveTarget = true;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnSuccessTransitionState;

        public override void FixedUpdate(float time)
        {
            if (!Always && (time < StartTime || time > StopTime)) return;

            else if (Hub.TryDetect(DetectorID, out var result) && CheckIfInside)
            {
                OnTargetFoundOrLost(result);
            }
            else if (result == null && !CheckIfInside)
            {
                OnTargetFoundOrLost(null);
            }
        }

        private void OnTargetFoundOrLost(GameObject target)
        {
            if (SaveTarget) Targets.SetTarget(target, TargetHolderID);
            TransitionHandler.DoTransition(OnSuccessTransitionState);
        }
    }

    [System.Serializable]
    public class HandleDamageBehaviour : StateBehaviour
    {
        public Damageable Dmg;

        public NPCDamageProcessor DamageProcessor;
        public NPCStateTransitionHandler TransitionHandler;

        [Header("Damage Processing and State Transitions Config")]
        public DamageBlockingConfig BlockingConfig;
        public HurtTransitionConfig HurtConfig;
        public SerializedDictionary<CombatCollisionResult, NPCState> CombatCollisionTransitions;

        [Header("Combat Collision FX")]
        public CombatCollisionFXHandler FXHandler;

        [Header("Targetting")]
        public TargetHandler Targets;

        public int AttackerTargetID;
        public bool SaveAttackerOnHit;

        public int AttackSourceTargetID;
        public bool SaveAttackSourceOnHit = true;

        public override void Start()
        {
            Dmg.AddEvent(OnTakeDamage);
        }

        public override void Stop()
        {
            Dmg.RemoveEvent(OnTakeDamage);
        }

        private void OnTakeDamage(Damage damage)
        {
            // instead of health handler, now it will be damage processor.
            var result = DamageProcessor.ProcessDamage(damage, this);

            if (SaveAttackerOnHit) Targets.SetTarget(damage.Attacker, AttackerTargetID);
            if (SaveAttackSourceOnHit) Targets.SetTarget(damage.AttackSource, AttackSourceTargetID);

            // then, do transitions based on the result. (no more transition logic in damage processing)
            if (CombatCollisionTransitions.ContainsKey(result))
            {
                TransitionHandler.DoTransition(CombatCollisionTransitions[result]);
            }

            // THEN, play FX.
            FXHandler?.PlayCombatCollisionFX(result);
        }
    }

    [System.Serializable]
    public class WaitBehaviour : StateBehaviour
    {
        public float WaitSeconds;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnWaitCompleteTransition;

        private float _waitUntilTime;

        public override void Start()
        {
            _waitUntilTime = Time.time + WaitSeconds;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _waitUntilTime) return;
            TransitionHandler.DoTransition(OnWaitCompleteTransition);
        }
    }

    [System.Serializable]
    public class UseMeleeAttackBehaviour : PhysicsBehaviour
    {
        public AttackController Controller;
        public float DelaySeconds;

        private float _useTime;
        private bool _attacked;

        public override void Start()
        {
            _useTime = Time.time + DelaySeconds;
            _attacked = false;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _useTime) return;
            else if (!_attacked)
            {
                Controller.UseAttack();
                _attacked = true;
            }
        }
    }

    [System.Serializable]
    public class UseRangedAttackBehaviour : PhysicsBehaviour
    {
        public RangedAttackController Controller;
        public float DelayBeforeAiming;
        public float AimDuration;

        [Header("Direction config")]
        public bool OnlyFireForward;
        public Transform Forward;

        [Header("Targetting")]
        public TargetHandler Targets;
        public int TargetID;

        // state variables
        private float _fireTime;
        private GameObject _aimTarget;

        private bool _fired;

        public override void Start()
        {
            _fireTime = DelayBeforeAiming + AimDuration;
            _aimTarget = Targets.GetTarget(TargetID);
            _fired = false;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (normalizedTime < DelayBeforeAiming || _fired) return;
            else if (normalizedTime < _fireTime)
            {
                Aim();
            }
            else if (!_fired)
            {
                _fired = true;
                Controller.Fire();
            }
        }

        private void Aim()
        {
            if (!OnlyFireForward) Controller.Aim(_aimTarget);
            else Controller.Aim(_aimTarget, Mathf.Sign(Forward.position.x - Rb2d.position.x));
        }
    }

    [System.Serializable]
    public class CheckAttackConditionBehaviour : StateBehaviour
    {
        public ConditionsHandler Conditions;
        public int ConditionID;

        [Header("Timings")]
        public bool AlwaysCheck;
        public float StartTime;
        public float EndTime;
        public float EvaluateDuration;

        [Header("Filter Config")]
        public AttackFilterHandler Filter;
        public bool AddAttackOnFinish;
        public bool AddAttackAfterDelay;
        public float DelayBeforeAdding;
        public int PotentialAttackID;

        // state
        private float _timeOfAddingPotentialAttacks;
        private bool _evaluationSuccess;

        public override void Start()
        {
            _timeOfAddingPotentialAttacks = 0;
            _evaluationSuccess = false;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (AddAttackOnFinish && normalizedTime >= EvaluateDuration && _evaluationSuccess)
            {
                Add();
            }

            else if (_evaluationSuccess && AddAttackAfterDelay && Time.time < _timeOfAddingPotentialAttacks)
            {
                Add();
            }

            else if (!AlwaysCheck && (normalizedTime < StartTime || normalizedTime > EndTime)) return;
            else if (!_evaluationSuccess && Conditions.Get(ConditionID).Evaluate())
            {
                HandleSuccessfulEvaluation();
            }
        }

        private void HandleSuccessfulEvaluation()
        {
            if (DelayBeforeAdding == 0 && AddAttackAfterDelay)
            {
                Add();
                return;
            }

            _timeOfAddingPotentialAttacks = Time.time + DelayBeforeAdding;
            _evaluationSuccess = true;
        }

        private void Add()
        {
            Filter.AddPotentialAttack(PotentialAttackID);
        }
    }

    [System.Serializable]
    public class CheckLineOfSightBehaviour : StateBehaviour
    {
        public Transform RaycastStart;

        [Header("Target Config")]
        public TargetHandler Targets;
        public int TargetHolderID;
        public bool CheckIfWithinLOS;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnSuccessTransition;

        [Min(1)] public float CheckFrequency;
        public LayerMask Layer;

        // readonly
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[2];

        // state variables
        private Transform _desiredTarget;
        private int _hitCount;
        private float _checkPeriod;
        private float _nextCheckTime;

        // cached stuff
        private Vector2 _start;
        private Vector2 _end;

        // debugging variables
        private Color _lineColor;

        public override void Start()
        {
            var target = Targets.GetTarget(TargetHolderID);
            if (target == null && !CheckIfWithinLOS)
            {
                TransitionHandler.DoTransition(OnSuccessTransition);
                return;
            }
            _desiredTarget = target.transform;

            _checkPeriod = 1f / CheckFrequency;
            _nextCheckTime = 0;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (normalizedTime < _nextCheckTime) return;
            _nextCheckTime += _checkPeriod;

            _start = RaycastStart.position;
            _end = _desiredTarget.transform.position;

            _hitCount = Physics2D.LinecastNonAlloc(_start, _end, _hits, Layer);

            if (CheckIfWithinLOS)
            {
                if (_hitCount == 1) TransitionHandler.DoTransition(OnSuccessTransition);
            }
            else
            {
                if (_hitCount > 1) TransitionHandler.DoTransition(OnSuccessTransition);
            }

            // debugging
            _lineColor = _hitCount > 1 ? Color.red : Color.green;
            Logger.DrawLine(_start, _end, Time.deltaTime, _lineColor);
        }
    }

    [System.Serializable]
    public class SetCooldownBehaviour : StateBehaviour
    {
        public CooldownsHandler Cooldowns;
        public int CooldownID;

        [Header("Custom cooldowns")]
        public bool UseCustomCooldown;
        public bool UseRandomCooldown;

        public float CustomCooldownTime;
        [Min(0)] public float CooldownRange;

        public override void Start()
        {
            if (!UseCustomCooldown) Cooldowns.StartCooldown(CooldownID);

            else if (!UseRandomCooldown) Cooldowns.StartCooldown(CooldownID, CustomCooldownTime);

            else
            {
                float randomCooldown = UnityEngine.Random.Range(CustomCooldownTime - CooldownRange, CustomCooldownTime + CooldownRange);
                Cooldowns.StartCooldown(CooldownID, randomCooldown);
            }
        }
    }

    [System.Serializable]
    public class RecoverPostureBehavior : StateBehaviour
    {
        public NPCHealthHandler HealthHandler;
        public bool ResetStaggerBuildup;
        public bool StartKnockbackCooldown;

        public override void Stop()
        {
            if (ResetStaggerBuildup) HealthHandler.ResetStaggerBuildup();

            if (StartKnockbackCooldown) HealthHandler.StartKnockbackCooldown();
        }
    }

    [System.Serializable]
    public class PlayAnimationBehaviour : StateBehaviour
    {
        public AnimationContext Animation;
        public AnimationHandler Animator;

        public float DelayBeforePlaying;
        private bool _played;

        public override void Start()
        {
            _played = false;
            if (DelayBeforePlaying == 0)
            {
                Play();
            }
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (_played || normalizedTime < DelayBeforePlaying) return;
            Play();
        }

        private void Play()
        {
            _played = true;
            Animator.Play(Animation);
        }
    }

    [System.Serializable]
    public class DespawnBehaviour : StateBehaviour
    {
        public Spawnable SpawnableGameObject;
        public float DespawnDelay;

        private bool _despawned;

        public override void Start()
        {
            _despawned = false;
            if (DespawnDelay == 0) Despawn();
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (_despawned || normalizedTime < DespawnDelay) return;
            Despawn();
        }

        private void Despawn()
        {
            _despawned = true;
            SpawnableGameObject.Despawn();
        }
    }

    [System.Serializable]
    public class TelegraphUIBehaviour : StateBehaviour
    {
        public AttackTelegrapher Telegrapher;
        public float StartTime;
        public float Duration;

        private bool _started;

        public override void Start()
        {
            _started = false;
        }

        public override void Stop()
        {
            Telegrapher.Stop();
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (_started || normalizedTime < StartTime) return;

            _started = true;
            Telegrapher.Telegraph(Duration);
        }
    }

    [System.Serializable]
    public class WaitAndCheckForTargetBehaviour : StateBehaviour
    {
        public float WaitSeconds;

        [Header("Transition Config")]
        public NPCStateTransitionHandler TransitionHandler;
        public TargetHandler Targets;
        public int TargetHolderID;
        public NPCState TransitionOnFound;
        public NPCState TransitionOnMissing;

        // state variables
        private float _waitUntilTime;

        public override void Start()
        {
            _waitUntilTime = Time.time + WaitSeconds;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _waitUntilTime) return;

            if (Targets.GetTarget(TargetHolderID) == null)
            {
                TransitionHandler.DoTransition(TransitionOnMissing);
            }
            else
            {
                TransitionHandler.DoTransition(TransitionOnFound);
            }
        }
    }

    [System.Serializable]
    public class ClearTargetsBehaviour : StateBehaviour
    {
        public TargetHandler Targets;
        public int TargetHolderID;
        public bool ClearOnStart;
        public bool ClearOnStop;

        public override void Start()
        {
            if (ClearOnStart) Targets.SetTarget(null, TargetHolderID);
        }

        public override void Stop()
        {
            if (ClearOnStop) Targets.SetTarget(null, TargetHolderID);
        }
    }

    /// <summary>
    /// This class is for PATROLLING enemies that are on the lookout for a potential target (player)
    /// </summary>
    [System.Serializable]
    public class SearchForTargetBehaviour : StateBehaviour
    {
        [Header("Targetting")]
        public TargetHandler Targets;
        public int TargetID;

        [Header("Detection")]
        public DetectorHub Detectors;
        public int DetectorID;

        [Header("Line of Sight")]
        public LayerMask Layer;
        public Transform RaycastStart;

        [Header("Performance")]
        [Min(1)] public int CheckFrequency;

        [Header("Transition")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnDetectTransitionState;

        // state variables
        private float _checkPeriod;
        private float _nextCheckTime;
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[2];

        //for debugging
        public bool DebugActivity { get; set; } = false;



        public override void Start()
        {
            _checkPeriod = 1f / CheckFrequency;
            _nextCheckTime = Time.time;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (DebugActivity) Logger.Log($"time: {Time.time}, next: {_nextCheckTime}");

            if (Time.time < _nextCheckTime) return;
            _nextCheckTime += _checkPeriod;


            if (Detectors.TryDetect(DetectorID, out var result))
            {
                if (MyExtensions.HasLineOfSight(RaycastStart.position, result.transform.position, _hits, Layer))
                {
                    if (DebugActivity) Logger.Log("player in range and in LOS");
                    TransitionHandler.DoTransition(OnDetectTransitionState);
                    Targets.SetTarget(result, TargetID);
                }
                else
                {
                    if (DebugActivity) Logger.Log("player in range but no LOS");
                }
            }
            else
            {
                if (DebugActivity) Logger.Log("player out of range.");
            }



            //if (!Detectors.TryDetect(DetectorID, out var result)) return;
            //else if (MyExtensions.HasLineOfSight(RaycastStart.position, result.transform.position, _hits, Layer))
            //{
            //    TransitionHandler.DoTransition(OnDetectTransitionState);
            //    Targets.SetTarget(result, TargetID);
            //}
        }
    }

    [System.Serializable]
    public class MonitorTargetBehaviour : StateBehaviour
    {
        [Header("Targetting")]
        public TargetHandler Targets;
        public int TargetID;

        [Header("Detection")]
        public DetectorHub Detectors;
        public int DetectorID;

        [Header("Line of Sight")]
        public LayerMask Layer;
        public Transform RaycastStart;
        public float MaxLossDuration;

        [Header("Performance")]
        [Min(1)] public int CheckFrequency;

        [Header("Transition")]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnLostTargetTransitionState;

        // state variables
        private GameObject _monitorTarget;
        private float _lostStartTime;
        private float _lostStopTime;
        private bool _outOfLOS;
        private float _checkPeriod;
        private float _nextCheckTime;
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[2];

        public override void Start()
        {
            _monitorTarget = Targets.GetTarget(TargetID);
            _lostStopTime = 0;
            _outOfLOS = false;

            _checkPeriod = 1f / CheckFrequency;
            _nextCheckTime = Time.time;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (Time.time < _nextCheckTime) return;
            _nextCheckTime += _checkPeriod;

            bool hasLOS = MyExtensions.HasLineOfSight(RaycastStart.position, _monitorTarget.transform.position, _hits, Layer);

            // check if in range. if true, then return early.
            if (!Detectors.DoesTargetMatchWith(_monitorTarget, DetectorID))
            {
                TransitionHandler.DoTransition(OnLostTargetTransitionState);
            }

            else if (hasLOS)
            {
                if (_outOfLOS)
                {
                    _outOfLOS = false;
                }
            }
            else
            {
                if (_outOfLOS && Time.time >= _lostStopTime)
                {
                    TransitionHandler.DoTransition(OnLostTargetTransitionState);
                }
                else if (!_outOfLOS)
                {
                    _outOfLOS = true;
                    _lostStopTime = Time.time + MaxLossDuration;
                }
            }
        }
    }

    [System.Serializable]
    public class SetResourceBehaviour : StateBehaviour
    {
        public AgentResourceHandler Resources;
        public int ResourceID;

        [Header("Setting Config")]
        public bool SetOnStart;
        public bool SetOnStop;
        [Separator]
        public bool Add;
        public bool UseRange;
        [Separator]
        public float Value;
        public float Range;

        public override void Start()
        {
            if (SetOnStart) UpdateResource();
        }

        public override void Stop()
        {
            if (SetOnStop) UpdateResource();
        }

        private void UpdateResource()
        {
            float value = UseRange ? Random.Range(Value - Range, Value + Range) : Value;

            if (Add) Resources.Set(ResourceID, Resources.Get(ResourceID) + value);
            else Resources.Set(ResourceID, value);
        }
    }

    [System.Serializable]
    public class ReactionBehaviour : StateBehaviour
    {
        public NPCReactionHandler Reactions;
        public int ReactionID;
        public float ReactionStartTime;
        public float ReactionEndTime;
        public GameEvent ReactionEvent;

        public override void Start()
        {
            ReactionEvent.AddEvent(OnReact);
        }

        public override void Stop()
        {
            ReactionEvent.RemoveEvent(OnReact);
        }

        private void OnReact()
        {
            Reactions.SetReactionWindow(ReactionID, ReactionStartTime, ReactionEndTime);
        }
    }

    [System.Serializable]
    public class SpawnItemsBehaviour : StateBehaviour
    {
        public ItemSpawner Spawner;

        public float SpawnAfterSeconds;

        // state variables
        private bool _spawned;

        public override void Start()
        {
            if (SpawnAfterSeconds == 0)
            {
                Spawner.Spawn();
                return;
            }

            _spawned = false;
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (normalizedTime < SpawnAfterSeconds || _spawned) return;

            Spawner.Spawn();
            _spawned = true;
        }
    }

    [System.Serializable]
    public class WaitForTarget : StateBehaviour
    {
        public TargetHandler TargetHandler;
        public int TargetID;
        public bool CheckIfNotNull;

        [Separator]
        public NPCStateTransitionHandler TransitionHandler;
        public NPCState OnSuccessTransition;

        // state variables
        private TargetHolder _targetHolder;

        public override void Start()
        {
            _targetHolder = TargetHandler.GetTargetHolder(TargetID);
        }

        public override void FixedUpdate(float normalizedTime)
        {
            if (CheckIfNotNull)
            {
                if (_targetHolder.Target != null) TransitionHandler.DoTransition(OnSuccessTransition);
            }
            else if (_targetHolder.Target == null) TransitionHandler.DoTransition(OnSuccessTransition);
        }
    }

    [System.Serializable]
    public class ShowTextBehaviour : StateBehaviour
    {
        public string Text;
        public TMPro.TMP_Text Textbox;

        public override void Start()
        {
            Textbox.text = Text;
        }
    }
}