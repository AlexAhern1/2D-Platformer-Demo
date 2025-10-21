using UnityEngine;

namespace Game
{
    public class Ruler : MonoBehaviour
    {
        public enum Shape { Circle, Line }

        [SerializeField] private Transform _t1;
        [SerializeField] private Transform _t2;

        [SerializeField][Min(2)] private int _segments;
        [SerializeField] private Shape _shape;
        [SerializeField] private float _length;
        [SerializeField] private Color _segmentColor;

        [SerializeField] private bool _enabled;

        private void OnDrawGizmos()
        {
            if (!_enabled) return;

            Gizmos.color = _segmentColor;

            Vector2 norm = default;

            if (_shape == Shape.Line)
            {
                Vector2 dir = _t1.position - _t2.position;
                norm = new Vector2(dir.y, -dir.x).normalized;
            }

            for (int i = 0; i < _segments; i++)
            {
                Vector2 position = Vector2.Lerp(_t1.position, _t2.position, ((float)i / (_segments - 1)));
                if (_shape == Shape.Circle)
                {
                    Gizmos.DrawSphere(position, _length);
                }

                else if (_shape == Shape.Line)
                {
                    Gizmos.DrawLine(position, position + _length * norm);
                }
            }
        }
    }
}