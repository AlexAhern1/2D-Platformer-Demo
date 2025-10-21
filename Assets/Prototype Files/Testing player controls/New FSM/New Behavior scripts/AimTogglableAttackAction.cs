using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class AimTogglableAttackAction : IStateAction
    {
        [Header("Components")]
        public TogglableAttackController Controller;
        public GameObject Target;
        public void DoAction()
        {
            Controller.Aim(Target);
        }
    }
}