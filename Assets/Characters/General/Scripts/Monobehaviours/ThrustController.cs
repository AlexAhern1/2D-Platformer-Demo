using System.Collections;
using UnityEngine;

namespace Game
{
    public class ThrustController : AttackController
    {
        [Header("Dependencies")]
        [SerializeField] private PointCaster _pointCaster;
        [SerializeField] private DamageApplier _damageHandler;
        [SerializeField] private ThrustAnimator _animator;

        public override void UseAttack()
        {
            _animator.Animate();

            StartPointCaster(_animator.StartDelay, _animator.Duration, _animator.EndDelay);
        }

        public override void StopAttack()
        {
            _animator.Cancel();
            _pointCaster.Toggle(false);
        }

        public void SetRotation(float zDegrees)
        {
            _animator.SetRotation(zDegrees);
        }

        private void OnEnable()
        {
            _pointCaster.AddDetectionEvents(OnTargetFound, null);
        }

        private void OnDisable()
        {
            _pointCaster.RemoveDetectionEvents(OnTargetFound, null);
        }

        private void OnTargetFound(Collider2D target)
        {
            RaiseHitTargetEvent(target.gameObject);

            if (!target.TryGetComponent<IDamageable>(out var damageable)) return;
            _damageHandler.ProcessDamage(damageable);
        }

        private void StartPointCaster(float toggleOnTime, float toggleOffTime, float disableTime)
        {
            StartCoroutine(ToggleLinecaster(toggleOnTime, toggleOffTime, disableTime));
        }

        private IEnumerator ToggleLinecaster(float toggleOnTime, float toggleOffTime, float disableTime)
        {
            yield return new WaitForSeconds(toggleOnTime);
            _pointCaster.Toggle(true);
            yield return new WaitForSeconds(toggleOffTime);
            _pointCaster.Toggle(false);
            yield return new WaitForSeconds(disableTime);
        }
    }
}