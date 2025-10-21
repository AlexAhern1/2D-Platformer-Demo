using UnityEngine;

namespace Game
{
    public class DetectorHub : MonoBehaviour
    {
        [Header("Detectors")]
        [SerializeReference, SubclassSelector]
        private Detector[] _detectors;

        [Header("Facing Direction Transforms")]
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _forward;

        [Header("DEBUGGING - Scene Hitbox Visualisation")]
        [SerializeField] private bool _DebugHitboxes;
        [SerializeField] private bool _DebugOnlyInEditmode;

        private void Awake()
        {
            if (_DebugOnlyInEditmode) _DebugHitboxes = false;
        }

        private float FacingDirection => Mathf.Sign(_forward.position.x - _center.position.x);

        public bool TryDetect(int ID, out GameObject result)
        {
            result = Detect(ID);
            return result != null;
        }

        public bool DoesTargetMatchWith(GameObject target, int ID)
        {
            return target == Detect(ID);
        }

        private GameObject Detect(int ID)
        {
            if (ID < _detectors.Length) return _detectors[ID].Detect(_center.position, FacingDirection);
            Logger.Warn($"Not a valid detection ID - {ID} in {this}", MoreColors.LightRose);
            return null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_DebugHitboxes) return;

            for (int i = 0; i < _detectors.Length; i++)
            {
                var d = _detectors[i];
                if (d.HideInInspector) continue;
                d.DrawGizmos(_center.position, FacingDirection);
               
            }
        }
    }
}