using System.Collections;
using UnityEngine;

namespace Game
{
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private float _duration;
        [SerializeField] private float _shakeFrequency;
        [SerializeField] private Transform _shakeTransform;

        // is shaking
        private Coroutine _shakeCoroutine;

        public void Shake(float direction)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(DoShake(direction));
        }

        private IEnumerator DoShake(float direction)
        {
            float elapsedTime = 0;
            Vector2 initialPosition = _shakeTransform.position;

            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime;
                _shakeTransform.position = initialPosition + direction * _speedCurve.Evaluate(elapsedTime) * Mathf.Cos(elapsedTime * _shakeFrequency * 2 * Mathf.PI) * Vector2.right;
                yield return null;
            }

            _shakeTransform.position = initialPosition;

            _shakeCoroutine = null;
            yield return null;
        }
    }
}