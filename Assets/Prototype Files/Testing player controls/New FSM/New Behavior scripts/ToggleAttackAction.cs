using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public class ToggleAttackAction : IStateAction
    {
        [Header("Components")]
        public TogglableAttackController Controller;
        public bool Value;

        public void DoAction() => Controller.Toggle(Value);
    }
}
