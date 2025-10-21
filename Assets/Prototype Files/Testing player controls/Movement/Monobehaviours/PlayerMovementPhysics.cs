using System;
using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementPhysics : MovementHandler
    {
        [SerializeField] private Rigidbody2D _rb2d;

        [SerializeField][ReadOnly] private Vector2 _baseVelocity;
        [SerializeField][ReadOnly] private Vector2 _externallyAppliedVelocity;

        [SerializeField] private float _gravityScale;
        [SerializeField] private float _maxFallSpeed;
        [SerializeField] private float _gravityCorrectionStrength;

        [Header("Dynamic variables")]
        [SerializeField] private bool _gravityFallingEnabled;

        // properties
        public override Vector2 Velocity => _baseVelocity + _externallyAppliedVelocity;

        // state variables
        private float _fallSpeed;
        private float _gravity;
        private Vector2 _gravityCorrection;

        // coroutines
        private Coroutine _baseVelocityOverriderCoroutine;

        // dynamic parameter setters
        private IFloatGetter _gravityScaleSetter;

        public void SetGravityScale(float scale)
        {
            _gravity = Physics2D.gravity.y * scale * Time.fixedDeltaTime;

            if (scale == 0)
            {
                _gravityCorrection.y = 0;
            }
        }

        public void ResetGravityScale()
        {
            _gravity = Physics2D.gravity.y * _gravityScale * Time.fixedDeltaTime;
            _gravityCorrection.y = -_gravityCorrectionStrength;
        }

        public void ToggleFalling(bool toggle)
        {
            _gravityFallingEnabled = toggle;
            _gravityCorrection.y = !toggle ? 0 : -_gravityCorrectionStrength;

            if (!toggle) _baseVelocity.y = 0;
        }

        public void SetMaxFallSpeed(float fallSpeed) => _fallSpeed = fallSpeed;

        public void ResetMaxFallSpeed() => _fallSpeed = _maxFallSpeed;

        public override void MoveHorizontal(float speed, bool @base)
        {
            if (@base) _baseVelocity.x = speed;
            else _externallyAppliedVelocity.x = speed;
        }

        public override void MoveVertical(float speed, bool @base)
        {
            if (@base) _baseVelocity.y = speed;
            else _externallyAppliedVelocity.y = speed;
        }

        public void Move(Vector2 velocity, bool @base)
        {
            if (@base) _baseVelocity = velocity;
            else _externallyAppliedVelocity = velocity;
        }

        public void OverrideBaseVelocity(Func<float, Vector2> velocityGetter, float duration)
        {
            if (_baseVelocityOverriderCoroutine != null) StopCoroutine(_baseVelocityOverriderCoroutine);
            _baseVelocityOverriderCoroutine = StartCoroutine(BaseVelocityOverrider(velocityGetter, duration));
        }

        public void CancelOverride()
        {
            if (_baseVelocityOverriderCoroutine == null) return;
            StopCoroutine(_baseVelocityOverriderCoroutine);
            _baseVelocityOverriderCoroutine = null;
        }

        public void OverrideHorizontalBaseVelocity(Func<float, float> speedGetter, float duration)
        {
            // here, the anonymous function needs:
            //  move input event E
            //  animation curve C
            //  movement speed S
            //  time t

            // return: E.x == 0 ? 0 : sign(E.x) * S * C.evaluate(t)
            //  return: TrueSign(E.x) * S * C.evaluate(t)



            if (_baseVelocityOverriderCoroutine != null) StopCoroutine(_baseVelocityOverriderCoroutine);
            _baseVelocityOverriderCoroutine = StartCoroutine(BaseVelocityOverrider((float t) => new Vector2(speedGetter(t), _baseVelocity.y), duration));
        }

        public override void Move(float horizontalSpeed, float verticalSpeed, bool @base)
        {
            if (@base) _baseVelocity = new Vector2(horizontalSpeed, verticalSpeed);
            else _externallyAppliedVelocity = new Vector2(horizontalSpeed, verticalSpeed);
        }

        public void SetGravityScaleSetter(IFloatGetter setter) => _gravityScaleSetter = setter;

        private void Awake()
        {
            _rb2d.gravityScale = 0;
            _fallSpeed = _maxFallSpeed;

            _gravity = Physics2D.gravity.y * _gravityScale * Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            _rb2d.velocity = _baseVelocity + _externallyAppliedVelocity + _gravityCorrection;

            if (_gravityFallingEnabled)
            {
                _baseVelocity.y = Mathf.Max(_baseVelocity.y + _gravity, -_fallSpeed);
            }

            if (_gravityScaleSetter != null) SetGravityScale(_gravityScaleSetter.GetFloat());
        }

        private IEnumerator BaseVelocityOverrider(Func<float, Vector2> velocityGetter, float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                _baseVelocity = velocityGetter(elapsedTime);
                elapsedTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            _baseVelocity = velocityGetter(duration);

            _baseVelocityOverriderCoroutine = null;
        }
    }
}