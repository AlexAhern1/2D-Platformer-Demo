using System.Threading.Tasks;
using UnityEngine;

namespace Game.Player
{
    public class PlayerKnockbackHandler : MonoBehaviour, IInitializable
    {
        [Header("Knockback config")]
        [SerializeField] private PlayerMovementPhysics _physics;
        [SerializeField][Min(0)] private int _defaultKnockbackResistance;
        [SerializeField] private float _knockbackDuration;
        [SerializeField][Min(0)] private float _horizontalKnockbackSpeed;
        [SerializeField] private float _verticalKnockbackSpeed;

        [Header("FX config")]
        [SerializeField] private FXPlayer _fxPlayer;
        [SerializeField] private int _takeDamageFXID;

        [Header("Events")]
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private GameEvent _enableInputEvent;

        [Header("Player States")]
        [SerializeField] private StateMachine _playerFSM;
        [SerializeField] private PlayerState _airborneIdleState;

        private int _currentKnockbackResistance;

        public void Initialize()
        {
            _currentKnockbackResistance = _defaultKnockbackResistance;
        }

        public void SetKnockbackResistance(int kbResist) => _currentKnockbackResistance = kbResist;

        public void ResetKnockbackResistance() => _currentKnockbackResistance = _defaultKnockbackResistance;

        public void HandleKnockbackOnTakeDamage(Damage damageData)
        {
            // first, play fx. (standard)
            _fxPlayer.Play(_takeDamageFXID);

            // then, compare damage's attack strength against knockback resistance.
            bool canKnockback = _currentKnockbackResistance < damageData.AttackStrength;
            if (canKnockback) DoKnockback(damageData.AttackSource.transform.position);
        }

        private async void DoKnockback(Vector2 attackSourcePosition)
        {
            float knockbackDirection = Mathf.Sign(transform.position.x - attackSourcePosition.x);
            
            // disable input
            _disableInputEvent.Raise();

            // apply knockback physics
            Vector2 knockbackVelocityGetter(float t) => new (_horizontalKnockbackSpeed * knockbackDirection, _verticalKnockbackSpeed);
            _physics.OverrideBaseVelocity(knockbackVelocityGetter, _knockbackDuration);

            await Wait(_knockbackDuration);

            _playerFSM.DoTransition(_airborneIdleState);

            await Wait(0.01f);

            _physics.CancelOverride();
            _physics.Move(Vector2.zero, true);
            _physics.ToggleFalling(true);

            _enableInputEvent.Raise();
        }

        private async Task Wait(float seconds)
        {
            float elapsedTime = 0f;
            while (elapsedTime < seconds)
            {
                await Task.Yield();
                elapsedTime += Time.deltaTime;
            }
        }
    }
}