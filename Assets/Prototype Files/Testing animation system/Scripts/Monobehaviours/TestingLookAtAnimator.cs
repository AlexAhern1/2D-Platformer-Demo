using UnityEngine;

namespace Game
{
    public class TestingLookAtAnimator : MonoBehaviour
    {
        // from the center position (need to cache it)
        // get the looker to look at a target.

        [SerializeField] private Transform _lookerTransform;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _maxRadius;

        // cached variables
        private Vector2 _center;


        private Vector2 _direction;
        private float _distance;

        private void Start()
        {
            _center = _lookerTransform.position;
        }

        private void LateUpdate()
        {
            // if the distance from the 


            // get the distance and direction between the looker and target.
            Vector2 fullDirection = (Vector2)_targetTransform.position - _center;
            _direction = fullDirection.normalized;
            _distance = fullDirection.magnitude;

            if (_distance < _maxRadius) _lookerTransform.position = _targetTransform.position;
            else
            {
                _lookerTransform.position = _center + _maxRadius * _direction;
            }
        }
    }
}