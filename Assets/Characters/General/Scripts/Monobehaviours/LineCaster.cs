using UnityEngine;

namespace Game
{
    /// <summary>
    /// handles trigger detection using line casts.
    /// </summary>
    public class LineCaster : Caster
    {
        private bool _targetsDetected;

        [SerializeField] protected Transform start;
        [SerializeField] protected Transform end;

        protected override void FindTargets()
        {
            Physics2D.LinecastNonAlloc(start.position, end.position, hits, targetMask);
            _targetsDetected = savedHits.Count > 0;
        }

        public void SetStartLocalPosition(Vector3 position)
        {
            start.localPosition = position;
        }

        public void SetEndLocalPosition(Vector3 position)
        {
            end.localPosition = position;
        }

        public void OnDrawGizmosSelected()
        {
            if (!visualizeLine) return;
            Gizmos.color = _targetsDetected ? Color.red : Color.white;
            Gizmos.DrawLine(start.position, end.position);
        }
    }
}