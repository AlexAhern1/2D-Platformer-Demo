using UnityEngine;

namespace Game
{
    public class TargetHandler : MonoBehaviour
    {
        [SerializeField] private TargetHolder[] _targets;

        public GameObject GetTarget(int index)
        {
            if (index < 0 || index > _targets.Length)
            {
                Logger.Warn("Invalid target index for GETTING target.");
                return null;
            }

            return _targets[index].Target;
        }

        public TargetHolder GetTargetHolder(int index)
        {
            if (index < 0 || index > _targets.Length)
            {
                Logger.Warn("Invalid target index for getting TARGET HOLDER.");
                return null;
            }

            return _targets[index];
        }

        public void SetTarget(GameObject target, int index)
        {
            if (index < 0 || index > _targets.Length)
            {
                Logger.Warn("Invalid target index for SETTING target.");
            }

            _targets[index].SetTarget(target);
        }
    }
}