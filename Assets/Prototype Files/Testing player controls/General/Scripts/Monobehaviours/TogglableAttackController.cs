using UnityEngine;

namespace Game
{
    public abstract class TogglableAttackController : MonoBehaviour, IToggle
    {
        public abstract void Aim(GameObject target);

        public abstract void Toggle(bool on);
    }
}