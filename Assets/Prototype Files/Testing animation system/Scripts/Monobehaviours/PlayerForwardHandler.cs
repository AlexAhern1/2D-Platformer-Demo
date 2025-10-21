using UnityEngine;

namespace Game.Player
{
    public class PlayerForwardHandler : MonoBehaviour, ICondition
    {
        [SerializeField] private Transform _forward;
        [SerializeField] private Transform _forwardParent;
        [SerializeField] private Transform _facing;
        [SerializeField] private Transform _facingParent;

        [SerializeField][Min(0)] private float _deadZone;

        public bool IsFacingForward => Mathf.Sign(_forward.position.x - _forwardParent.position.x) == Mathf.Sign(_facing.position.x - _facingParent.position.x);

        public bool Evaluate() => IsFacingForward;

        public void SetForward(float direction)
        {
            if (Mathf.Abs(direction) <= _deadZone) return;

            _forwardParent.rotation = Quaternion.Euler(0, 90f * (1f - Mathf.Sign(direction)), 0);
        }
    }
}