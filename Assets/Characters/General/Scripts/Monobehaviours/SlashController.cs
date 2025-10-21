using System.Collections;
using UnityEngine;

namespace Game
{
    public class SlashController : AttackController
    {
        [Header("Components")]
        [SerializeField] private LineCaster _lineCaster;
        [SerializeField] private DamageApplier _damageHandler;
        [SerializeField] private SlashAnimator _animator;
        [SerializeField] private DamageFrequencyModerator _frequencyModerator;

        [Header("Delays")]
        [SerializeField] private float _linecastStartDelay;
        [SerializeField] private float _linecastEarlyEnd;

        public override void UseAttack()
        {
            _animator.Animate();
            StartTogglingLineCaster(_linecastStartDelay, _animator.Duration, _animator.EndDelay - _linecastEarlyEnd);
        }

        public override void StopAttack()
        {
            _animator.Stop();
            _lineCaster.Toggle(false);
        }

        private void OnEnable()
        {
            _lineCaster.AddDetectionEvents(OnTargetFound, null);
        }

        private void OnDisable()
        {
            _lineCaster.RemoveDetectionEvents(OnTargetFound, null);
        }

        private void OnTargetFound(Collider2D target)
        {
            RaiseHitTargetEvent(target.gameObject);

            if (!target.TryGetComponent<IDamageable>(out var damageable)) return;
            else if (_frequencyModerator.UpdateAndCheckIfCanDamage(damageable))
            {
                _damageHandler.ProcessDamage(damageable);
            }
        }

        private void StartTogglingLineCaster(float startDelay, float toggleOffTime, float disableTime)
        {
            StartCoroutine(ToggleLinecaster(startDelay, toggleOffTime, disableTime));
        }

        private IEnumerator ToggleLinecaster(float startDelay, float toggleOffTime, float disableTime)
        {
            yield return new WaitForSeconds(startDelay);

            _lineCaster.Toggle(true);

            yield return new WaitForSeconds(toggleOffTime);

            _lineCaster.Toggle(false);
            _frequencyModerator.ClearDamageables();

            yield return new WaitForSeconds(disableTime);
        }
    }
}