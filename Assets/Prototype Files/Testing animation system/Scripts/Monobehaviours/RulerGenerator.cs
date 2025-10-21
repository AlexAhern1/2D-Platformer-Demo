using UnityEngine;

namespace Game
{
    public class RulerGenerator : MonoBehaviour
    {
        [SerializeField] private float _interval;

        [SerializeField] private Transform _t1;
        [SerializeField] private Transform _t2;
        [SerializeField] private Transform _pivot;

        [SerializeField][ReadOnly] private float _distance;

        private float _modTime;
        private int _transformCount;

        private void Awake()
        {
            _transformCount = 1;
        }

        private void Update()
        {
            _modTime += Time.deltaTime;
            if (_modTime < _interval) return;

            _modTime = 0;

            _distance = Vector2.Distance(_t1.position, _t2.position);

            SetTransform(_transformCount == 1 ? _t1 : _t2);
            _transformCount *= -1;
        }

        private void SetTransform(Transform t)
        {
            t.position = _pivot.position;
        }
    }
}