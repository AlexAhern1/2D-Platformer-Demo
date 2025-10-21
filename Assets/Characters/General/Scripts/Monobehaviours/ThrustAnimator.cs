using UnityEngine;

namespace Game
{
    public class ThrustAnimator : MonoBehaviour
    {
        [Header("parameters")]
        [SerializeField] private float _thrustSpeed;
        [SerializeField] private Vector2 _thrustDirection;
        [SerializeField][Min(0f)] private float _duration;
        [SerializeField][Min(0f)] private float _startDelay;
        [SerializeField][Min(0f)] private float _endDelay;

        [Header("dependencies")]
        [SerializeField] private Transform _focus;
        [SerializeField] private TrailRenderer _trail;

        //state variables
        private float _startTime;
        private float _stopTime;
        private float _deactivateTime;

        //fixed variables
        private Vector3 _normalizedDirection;
        private Vector3 _restingPosition;

        public float Duration => _duration;
        public float StartDelay => _startDelay;
        public float EndDelay => _endDelay;
        public float TotalDuration => _duration + _startDelay + _endDelay;

        public void Animate()
        {
            if (enabled) return;
            enabled = true;

            _startTime = Time.time + _startDelay;
            _stopTime = _startTime + _duration;
            _deactivateTime = _stopTime + _endDelay;
        }

        public void Cancel()
        {
            ToggleTrailRenderer(false);
            enabled = false;
            ResetFocusPosition();
            ResetTimes();
        }

        public void SetRotation(float zDegress)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, zDegress);
        }

        private void Awake()
        {
            enabled = false;
            _trail.emitting = false;
            _normalizedDirection = _thrustDirection.normalized;
            _restingPosition = _focus.localPosition;
        }

        private void Update()
        {
            if (Time.time < _startTime) return;
            else if (Time.time < _stopTime)
            {
                ToggleTrailRenderer(true);
                MoveFocusPosition(_thrustSpeed * Time.deltaTime);
                return;
            }
            else if (Time.time < _deactivateTime)
            {
                ToggleTrailRenderer(false);
                return;
            }

            enabled = false;
            ResetFocusPosition();
            ResetTimes();
        }

        private void MoveFocusPosition(float speed)
        {
            _focus.localPosition += _normalizedDirection * speed;
        }

        private void ResetFocusPosition()
        {
            _focus.localPosition = _restingPosition;
        }

        private void ToggleTrailRenderer(bool toggleOn)
        {
            _trail.emitting = toggleOn;
        }

        private void ResetTimes()
        {
            _startTime = 0f;
            _stopTime = 0f;
            _deactivateTime = 0f;
        }
    }
}