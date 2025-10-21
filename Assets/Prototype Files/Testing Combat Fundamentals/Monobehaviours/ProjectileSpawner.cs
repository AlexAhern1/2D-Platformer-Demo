using UnityEngine;

namespace Game
{
    public class ProjectileSpawner : RangedAttackController
    {
        [SerializeField] private Projectile _projectilePrefab;

        [Header("Aiming Config")]
        [SerializeField] private GameObject _aimSource;
        [SerializeField][Range(0, 1)] private float _aimingSpeed;

        [Header("Firing Config")]
        [SerializeField] private float _initialSpeed;
        [SerializeField] private bool _affectedByGravity;
        [SerializeField] private float _gravityScale;
        [SerializeField] private float _globalGravityScaleError;

        [Header("Projectile Properties Config")]
        [SerializeField] private Damage _projectileDamage;
        [SerializeField] private float _projectileLifetime;

        [Header("Default settings")]
        [SerializeField][Range(-90, 90)] private float _defaultFiringAngle;

        [Header("Debugging")]
        [Separator]
        public GameObject DebugTarget;
        public Transform DebugForward;
        public float DebugAimDuration;
        public bool DrawAimLine;
        public bool ShowAimTrajectory;
        public bool ShowDebugTrajectory;
        public int SamplePoints;
        public float Timestep;
        public float SampleRadius;
        public float DebugFireTime;
        public float AimRadius;
        public float FireRadius;

        // state variables
        private bool _aimStarted;
        private bool _firingInDefaultDirection;
        private float _defaultFiringDirection;
        private Vector3 _newAimPosition;
        private Vector3 _currentAimPosition;
        private bool _waitingForAimingToStop;
        private float _debugFireEndTime;

        // properties
        public GameObject AimSource => _aimSource;

        // cached constants;
        private float _k;

        private float _globalGravity => Physics2D.gravity.y - _globalGravityScaleError;

        private void Awake()
        {
            InitializeConstants();
        }

        private void OnValidate()
        {
            InitializeConstants();
        }

        public override void Aim(GameObject target)
        {
            UpdateAimPosition(target);
        }

        public override void Aim(GameObject target, float direction)
        {
            float directionToTarget = Mathf.Sign(_currentAimPosition.x - AimSource.transform.position.x);
            _firingInDefaultDirection = (directionToTarget != direction);
            _defaultFiringDirection = direction;
            UpdateAimPosition(target);
        }

        private void InitializeConstants()
        {
            _k = (_globalGravity * _gravityScale) / (2f * _initialSpeed * _initialSpeed);
        }

        private void UpdateAimPosition(GameObject target)
        {
            _newAimPosition = target.transform.position;

            if (!_aimStarted)
            {
                _aimStarted = true;
                _waitingForAimingToStop = true;
                _currentAimPosition = _newAimPosition;
                return;
            }
            _currentAimPosition = Vector3.Slerp(_currentAimPosition, _newAimPosition, _aimingSpeed);
        }

        public override void Fire()
        {
            _aimStarted = false;

            // 0.5) check if we are firing in the default direction.
            Vector2 fireDirection;
            if (_firingInDefaultDirection)
            {
                float firingDegrees = 90 * (1 - _defaultFiringDirection) + _defaultFiringDirection * _defaultFiringAngle;
                float firingRadians = firingDegrees * Mathf.Deg2Rad;

                fireDirection = new Vector2(Mathf.Cos(firingRadians), Mathf.Sin(firingRadians));
                Logger.Log($"default: {fireDirection}", Logger.EnemyCombat);
            }
            else
            {
                fireDirection = GetFireDirection(AimSource.transform.position, _currentAimPosition);
                Logger.Log($"NOT default: {fireDirection}", Logger.EnemyCombat);
            }
            
            var projectile = Instantiate(_projectilePrefab);

            // 0.75) set the projectile's position to the source position
            projectile.transform.position = AimSource.transform.position;

            // 1) set projectile data
            projectile.SetProjectileData(_projectileDamage, _projectileLifetime);

            // 2) set projectile velocity
            Logger.Log($"init speed: {_initialSpeed}x{fireDirection}", Logger.EnemyCombat);

            projectile.SetVelocity(_initialSpeed * fireDirection);

            // 2.5) set gravity scale if affected by gravity
            if (_affectedByGravity) projectile.SetGravityScale(_gravityScale);

            // 3) spawn projectile.
            projectile.Spawn();
        }

        public void FireAtDebugTarget()
        {
            Vector2 fireDirection = GetFireDirection(AimSource.transform.position, DebugTarget.transform.position);

            var proj = Instantiate(_projectilePrefab);
            proj.transform.position = AimSource.transform.position;

            proj.SetProjectileData(_projectileDamage, _projectileLifetime);
            proj.SetVelocity(_initialSpeed * fireDirection);
            if (_affectedByGravity) proj.SetGravityScale(_gravityScale);
            proj.Spawn();
        }

        private Vector2 GetFireDirection(Vector2 source, Vector2 target)
        {
            if (_affectedByGravity)
            {
                float launchAngle = Mathf.Atan(GetGravityAffectedLaunchAngleTAN(source, target));
                if (Mathf.Sign(target.x - source.x) < 0) launchAngle += Mathf.PI;

                Logger.Log($"GRAVITY -> angle: {launchAngle}, X: {Mathf.Cos(launchAngle)}, Y: {Mathf.Sin(launchAngle)}", Logger.EnemyCombat);


                return new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle));
            }
            else
            {
                return (target - source).normalized;
            }
        }

        private float GetGravityAffectedLaunchAngleTAN(Vector2 source, Vector2 target)
        {
            float X0 = source.x;
            float Y0 = source.y;

            float X = target.x - X0;
            float Y = target.y - Y0;

            float a = _k * X * X;
            float b = X;
            float c = a - Y;

            float r = Mathf.Sign(target.x - source.x);

            float value = (-b + r * Mathf.Sqrt(Mathf.Max(0, b * b - 4 * a * c))) / (2f * a);

            Logger.Log($"Seriously? {X0}, {Y0}, {X}, {Y}, {a}, {b}, {c}, {r}, {value}", Logger.EnemyCombat);
            return value;
        }

        private void OnDrawGizmos()
        {
            if (ShowAimTrajectory) OnGizmosShowAimingTrajectory();
            if (DrawAimLine && _aimStarted) OnGizmosShowAiming();

            if (DebugTarget == null) return;
            if (ShowDebugTrajectory) OnGizmosShowDebugTrajectory();
        }

        private void OnGizmosShowDebugTrajectory()
        {
            Gizmos.color = Color.white;
            if (!_affectedByGravity)
            {
                Vector2 sourcePos = AimSource.transform.position;
                Vector2 fullLengthDirection = (Vector2)DebugTarget.transform.position - sourcePos;

                float dist = fullLengthDirection.magnitude;
                Vector2 direction = fullLengthDirection.normalized;

                for (int i = 0; i < SamplePoints; i++)
                {
                    float distIncrement = ((float)i / SamplePoints) * dist;

                    Vector3 samplePoint = sourcePos + distIncrement * direction;
                    Gizmos.DrawSphere(samplePoint, SampleRadius);
                }
            }
            else
            {
                {
                    Vector2 source = AimSource.transform.position;
                    Vector2 targetPos = DebugTarget.transform.position;

                    float angle = Mathf.Atan(GetGravityAffectedLaunchAngleTAN(source, targetPos));
                    float t = 0;
                    float x;
                    float y;

                    for (int i = 0; i <= SamplePoints; i++)
                    {
                        x = t * _initialSpeed * Mathf.Cos(angle) + source.x;
                        y = t * _initialSpeed * Mathf.Sin(angle) + 0.5f * _globalGravity * _gravityScale * (t*t) + source.y;

                        Gizmos.DrawSphere(new Vector2(x, y), SampleRadius);

                        t += Timestep;
                    }
                }
            }
        }

        private void OnGizmosShowAiming()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_currentAimPosition, AimRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_aimSource.transform.position, _currentAimPosition);
        }

        private void OnGizmosShowAimingTrajectory()
        {
            if ((_waitingForAimingToStop || Time.time < _debugFireEndTime) && !_aimStarted)
            {
                if (_waitingForAimingToStop)
                {
                    _debugFireEndTime = Time.time + DebugFireTime;
                    _waitingForAimingToStop = false;
                }

                Gizmos.color = MoreColors.Orange;
                Gizmos.DrawWireSphere(_currentAimPosition, FireRadius);

            }

            else if (!_aimStarted) return;
            // aim started and waiting for aim to finish.

            Gizmos.color = Color.white;
            if (!_affectedByGravity)
            {
                Vector2 sourcePos = AimSource.transform.position;
                Vector2 fullLengthDirection = (Vector2)_currentAimPosition - sourcePos;

                float dist = fullLengthDirection.magnitude;
                Vector2 direction = fullLengthDirection.normalized;

                for (int i = 0; i < SamplePoints; i++)
                {
                    float distIncrement = ((float)i / SamplePoints) * dist;

                    Vector2 samplePoint = sourcePos + distIncrement * direction;
                    Gizmos.DrawSphere(samplePoint, SampleRadius);
                }
            }
            else
            {
                {
                    Vector2 source = AimSource.transform.position;

                    float angle = Mathf.Atan(GetGravityAffectedLaunchAngleTAN(source, _currentAimPosition));

                    float dir = Mathf.Sign(_currentAimPosition.x - source.x);
                    if (dir < 0) angle += Mathf.PI;

                    float t = 0;
                    float x;
                    float y;

                    for (int _ = 0; _ <= SamplePoints; _++)
                    {
                        x = t * _initialSpeed * Mathf.Cos(angle) + source.x;
                        y = t * _initialSpeed * Mathf.Sin(angle) + 0.5f * _globalGravity * _gravityScale * (t * t) + source.y;

                        Gizmos.DrawSphere(new Vector2(x, y), SampleRadius);

                        t += Timestep;
                    }
                }
            }
        }
    }
}