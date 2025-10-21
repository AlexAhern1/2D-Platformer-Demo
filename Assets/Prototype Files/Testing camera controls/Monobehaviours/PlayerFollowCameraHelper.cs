using UnityEngine;

namespace Game.Player
{
    public class PlayerFollowCameraHelper : MonoBehaviour
    {
        [Header("Camera Position Reference")]
        [SerializeField] private Vector2Reference _positionReference;

        [Header("Horizontal Movement")]
        [SerializeField] private Transform _forward;
        [SerializeField] private Transform _forwardWRTSpeed;
        [SerializeField] private Transform _verticalFollow;

        [SerializeField] private PlayerMovementPhysics _movementHandler;
        [SerializeField] private float _horizAcc;
        [SerializeField] private float _maxHorizSpeed;
        [SerializeField] private float _horizEpsilon;
        [SerializeField] private float _speedRatio;
        [SerializeField] private AnimationCurve _horizCloseDistanceCurve;

        [Header("Vertical Movement")]
        [SerializeField] private float _upwardsSpeed;
        [SerializeField] private float _downwardsSpeed;
        [SerializeField] private float _relaxationSpeed;
        [SerializeField] private float _decaySpeed;
        [SerializeField] private float _velocityThreshold;
        [SerializeField] private AnimationCurve _verticalCurve;

        [Header("debugging")]
        public TMPro.TMP_Text t_text;
        public TMPro.TMP_Text s_text;

        // state variables - horizontal
        private float _horiz_t;
        private float _horizSpeed;
        private float _horizDir;
        private float _horizDist;
        private float _horizDistThreshold;

        // state variables - vertical
        private float _yPositionInPreviousFrame;

        private float _t;

        private void Awake()
        {
            _positionReference.Set(transform.position);
            _yPositionInPreviousFrame = transform.position.y;

            _t = 0;
        }

        private void Update()
        {
            HandleHorizontalCameraMovement();
            HandleVerticalCameraMovement();
        }

        private void HandleHorizontalCameraMovement()
        {
            var pos = _positionReference.Value;

            float absVel = Mathf.Abs(_movementHandler.Velocity.x);
            float trueRatio = _speedRatio * absVel;
            float spd = Mathf.Sign(_forward.position.x - transform.position.x) * (trueRatio <= 1 ? 0 : trueRatio);

            _forwardWRTSpeed.position = _forward.position + spd * Vector3.right;

            float horizX = pos.x;
            float forwardX = _forwardWRTSpeed.position.x;

            float signedDist = forwardX - horizX;

            _horizDir = Mathf.Sign(signedDist);
            _horizDist = Mathf.Abs(signedDist);

            if (_horizDist < _horizEpsilon)
            {
                pos.x = _forwardWRTSpeed.position.x;
            }
            else
            {
                if (Mathf.Sign(_horiz_t) != _horizDir) _horiz_t = 0;

                _horiz_t = Mathf.Clamp(_horiz_t + _horizDir * _horizAcc * Time.deltaTime, -1, 1);
                _horizDistThreshold = _horizCloseDistanceCurve.Evaluate(_horizDist);
                _horizSpeed = _horiz_t * _maxHorizSpeed * _horizDistThreshold;
            }
            pos.x += _horizSpeed * Time.deltaTime;

            _positionReference.SetX(pos.x);
        }

        private void HandleVerticalCameraMovement()
        {
            float yVelocity = _movementHandler.Velocity.y;
            Vector2 localPos = _verticalFollow.localPosition;

            string s;

            if (yVelocity <= -_velocityThreshold)
            {
                _t = Mathf.Max(-1, _t - _downwardsSpeed * Time.deltaTime);
                s = _downwardsSpeed.ToString();

            }
            else if (yVelocity <= 0 && yVelocity > -_velocityThreshold)
            {
                float speed = yVelocity == 0 ? _decaySpeed : _relaxationSpeed;
                s = speed.ToString();

                if (_t >= 0)
                {
                    _t = Mathf.Max(0, _t - speed * Time.deltaTime);
                }
                else
                {
                    _t = Mathf.Min(0, _t + speed * Time.deltaTime);
                }
            }
            else
            {
                _t = Mathf.Min(1, _t + _upwardsSpeed * Time.deltaTime);
                s = _upwardsSpeed.ToString();
            }


            //string t = _t.ToString();

            //t_text.text = t;
            //s_text.text = s;

            localPos.y = _verticalCurve.Evaluate(_t);
            _verticalFollow.localPosition = localPos;

            _yPositionInPreviousFrame = transform.position.y;
            _positionReference.SetY(_verticalFollow.position.y);
        }
    }
}