using UnityEngine;

namespace Game
{
    public abstract class RangedAttackController : MonoBehaviour
    {
        public abstract void Aim(GameObject target);

        public abstract void Aim(GameObject target, float direction);

        public abstract void Fire();
    }
}