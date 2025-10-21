using UnityEngine;

namespace Game
{
    public class SlashAnimator : MonoBehaviour
    {
        [Header("parameters")]
        [SerializeField] private float _sweepAngle;
        [SerializeField][Min(0f)] private float _duration;
        [SerializeField][Min(0f)] private float _endDelay;

        [Header("dependencies")]
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _tip;
        [SerializeField] private TrailRenderer _trail;

        [Header("Debugging")]
        [SerializeField] private bool _visualiseSlashPathInInspector;
        [SerializeField][Min(1)] private int _samplePoints;
        [SerializeField][Min(0.1f)] private float _minRadius = 0.1f;
        [SerializeField][Min(0.1f)] private float _maxRadius = 0.3f;
        [SerializeField] private Color _startSlashColor;
        [SerializeField] private Color _endSlashColor;

        private Quaternion _restingRotation;

        private float _stopTime;
        private float _deactivateTime;

        private float _durationInverse;

        public float Duration => _duration;
        public float EndDelay => _endDelay;
        public float TotalDuration => _duration + _endDelay;

        public void Animate()
        {
            if (enabled) return;
            enabled = true;

            _stopTime = Time.time + _duration;
            _deactivateTime = _stopTime + _endDelay;
        }

        public void Stop()
        {
            if (!enabled) return;

            ToggleTrailRenderer(false);
            enabled = false;
            ResetPivotAngle();
            ResetTimes();
        }

        private void Awake()
        {
            enabled = false;
            _trail.emitting = false;
            _durationInverse = 1f / _duration;
            _restingRotation = _pivot.localRotation;
        }

        private void OnValidate()
        {
            _durationInverse = 1f / _duration;
        }

        private void Update()
        {
            if (Time.time < _stopTime)
            {
                ToggleTrailRenderer(true);
                RotatePivot(_sweepAngle * _durationInverse * Time.deltaTime);
                return;
            }
            else if (Time.time < _deactivateTime)
            {
                ToggleTrailRenderer(false);
                return;
            }

            enabled = false;
            ResetPivotAngle();
            ResetTimes();
        }

        private void RotatePivot(float degrees)
        {
            Vector3 eulers = _pivot.transform.rotation.eulerAngles;
            eulers.z -= degrees;

            _pivot.transform.rotation = Quaternion.Euler(eulers);
        }

        private void ResetPivotAngle()
        {
            _pivot.localRotation = _restingRotation;
        }

        private void ToggleTrailRenderer(bool toggleOn)
        {
            _trail.emitting = toggleOn;
        }

        private void ResetTimes()
        {
            _stopTime = 0f;
            _deactivateTime = 0f;
        }

        private void OnDrawGizmos()
        {
            if (!_visualiseSlashPathInInspector || Application.isPlaying) return;

            // debugging values
            float startAngle = _pivot.rotation.eulerAngles.z;

            float xRot = _pivot.rotation.eulerAngles.x;
            float yRot = _pivot.rotation.eulerAngles.y;
            Vector3 initialDirection = _tip.position - _pivot.position;

            Quaternion initialRotation = _pivot.rotation;

            float ratio;
            float rotation;

            Quaternion newRotation;
            Quaternion diff;

            Vector3 newDirection;
            Vector3 samplePosition;

            for (int i = 0; i < _samplePoints; i++)
            {
                ratio = ((float)i / _samplePoints);
                rotation = startAngle - ratio * _sweepAngle;

                newRotation = Quaternion.Euler(xRot, yRot, rotation);
                diff = newRotation * Quaternion.Inverse(initialRotation);

                newDirection = diff * initialDirection;
                samplePosition = _pivot.position + newDirection;

                Gizmos.color = Color.Lerp(_startSlashColor, _endSlashColor, ((float)i)/_samplePoints);
                Gizmos.DrawSphere(samplePosition, _minRadius + ratio * (_maxRadius - _minRadius));
            }
        }
    }
}