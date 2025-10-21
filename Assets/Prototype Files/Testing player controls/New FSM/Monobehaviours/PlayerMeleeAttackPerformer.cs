using UnityEngine;

namespace Game.Player
{
    public class PlayerMeleeAttackPerformer : MonoBehaviour, IToggle
    {
        // NOTE - the action of performing an attack is deliberately done in a monobehaviour so that
        // choosing between different kinds of melee attacks (based on equipment/unlock conditions) can be more easily supported.

        [Header("Attack Controller")]
        public AttackController Controller;

        [Header("FX")]
        public FXPlayer Fxs;
        public int FXId;

        // eventually add animations too.

        public void Toggle(bool on)
        {
            if (on) StartAttack();
            else StopAttack();
        }

        private void StartAttack()
        {
            Controller.UseAttack();
            Fxs.Play(FXId);
        }

        private void StopAttack()
        {
            // also handles cancelling.
            Controller.StopAttack();
        }
    }
}