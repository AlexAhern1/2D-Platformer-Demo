using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class AimRangedAttackAction : IStateAction
    {
        [Header("Components")]
        public RangedAttackController Controller;
        public GameObject Target;
        public void DoAction()
        {
            Controller.Aim(Target);
        }
    }
}