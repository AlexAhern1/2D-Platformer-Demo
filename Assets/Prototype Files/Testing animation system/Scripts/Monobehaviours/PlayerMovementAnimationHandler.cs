using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [Header("Walking/Running Blending")]
        [SerializeField] private string _animationID;
        [SerializeField] private float _lerpSpeed;

        // state variables
        private bool _isLerping;
        private float _startValue;
        private float _currentValue;
        private float _endValue;

        private float _normTime;
        private float _lerpEndTimeInverse;

        public void SetMovementValue(float value)
        {
            if (_endValue == value) return;

            _isLerping = true;

            _startValue = _currentValue;
            _endValue = value;

            float dist = Mathf.Abs(_endValue - _startValue);

            _lerpEndTimeInverse = _lerpSpeed / dist;

            _normTime = 0;
        }

        private void Update()
        {
            if (!_isLerping) return;
            else if (_normTime < 1)
            {
                _normTime += Time.deltaTime * _lerpSpeed;
                _currentValue = Mathf.Lerp(_startValue, _endValue, _normTime * _lerpEndTimeInverse);
                
                _animator.SetFloat(_animationID, _currentValue);

                return;
            }

            _isLerping = false;
        }
    }
}