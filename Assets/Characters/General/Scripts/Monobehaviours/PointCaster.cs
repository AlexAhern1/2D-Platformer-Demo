using UnityEngine;

namespace Game
{
    public class PointCaster : Caster
    {
        [SerializeField] private Transform _focus;

        private bool _targetsDetected;

        protected override void FindTargets()
        {
            Physics2D.CircleCastNonAlloc(_focus.position, 0.1f, Vector2.zero, hits, 0f, targetMask);
            _targetsDetected = savedHits.Count > 0;
        }

        public void OnDrawGizmosSelected()
        {
            if (!visualizeLine) return;
            Gizmos.color = _targetsDetected ? Color.red : Color.white;
            Gizmos.DrawWireSphere(_focus.position, 0.25f);
        }
    }
}