using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class FireRangedAttackAction : IStateAction
    {
        [Header("Components")]
        public RangedAttackController Controller;

        public void DoAction() => Controller.Fire();
    }
}