using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LaserBeam : TogglableAttackController
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LineCaster _laserCaster;
        [SerializeField] private Collider2D _laserCollider;
        [SerializeField] private Transform _launcherParent;
        [SerializeField] private Transform _laserPointA;
        [SerializeField] private Transform _laserPointB;

        [Header("Laser Config")]
        [SerializeField] private Damage _initialDamage;
        [SerializeField] private Damage _continuousDamage;
        [SerializeField] private float _laserLength;
        [SerializeField] private float _expansionDuration;
        [SerializeField] private float _damagePeriod;

        [Header("Ignore Player")]
        [SerializeField] private Tag _playerTag;
        [SerializeField] private bool _ignorePlayer;

        //continuous damage applier tracker
        private List<IDamageable> _damageables = new(16);
        private float _nextDamageTime;

        //others
        private Coroutine _expansionCoroutine;

        public override void Aim(GameObject target)
        {
            // the aim target is already placed correctly

            Vector2 direction = target.transform.position - transform.position;
            if (direction.y != 0)
            {
                float verticalDirection = MyExtensions.TrueSign(direction.y);
                _launcherParent.rotation = Quaternion.Euler(0f, 0f, 90f * verticalDirection);
            }
            else
            {
                float horizontalDirection = MyExtensions.TrueSign(direction.x);
                _launcherParent.rotation = Quaternion.Euler(0f, (1f - horizontalDirection) * 90f, 0f);
            }
        }

        public override void Toggle(bool on)
        {
            if (on) ToggleOn();
            else ToggleOff();
        }

        private void ToggleOn()
        {
            //enable game object
            gameObject.SetActive(true);
            _laserCollider.enabled = false;

            ////set the laser's emission point and firing direction
            //transform.localPosition = _direction * _offsetDistance;

            //float firingAngle = _direction.Arctan();
            //transform.localRotation = Quaternion.Euler(0, 0, firingAngle);

            float laserRadius = 0.5f * _spriteRenderer.size.y;
            _laserPointA.transform.localPosition = laserRadius * Vector2.up;
            _laserPointB.transform.localPosition = laserRadius * Vector2.down;

            _expansionCoroutine = StartCoroutine(ExpandLaser(_laserLength, _expansionDuration));
        }

        private void ToggleOff()
        {
            if (_expansionCoroutine != null)
            {
                StopCoroutine(_expansionCoroutine);
                _expansionCoroutine = null;
            }

            _laserCollider.enabled = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Time.time < _nextDamageTime) return;
            _nextDamageTime = Time.time + _damagePeriod;

            for (int i = 0; i < _damageables.Count; i++)
            {
                _damageables[i].TakeDamage(_continuousDamage);
            }
        }

        private void OnLaserHitWhileExpanding(Collider2D collider)
        {
            if (collider.CompareTag(_playerTag) && _ignorePlayer) return;
            else if (collider.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.TakeDamage(_initialDamage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(_playerTag) && _ignorePlayer) return;
            else if (!collider.TryGetComponent<IDamageable>(out var dmg)) return;
            else if (!_damageables.Contains(dmg))
            {
                _damageables.Add(dmg);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag(_playerTag) && _ignorePlayer) return;
            else if (!collider.TryGetComponent<IDamageable>(out var dmg)) return;
            else if (_damageables.Contains(dmg))
            {
                _damageables.Remove(dmg);
            }
        }

        private IEnumerator ExpandLaser(float length, float duration)
        {
            _laserCaster.Toggle(true);
            _laserCaster.AddDetectionEvents(OnLaserHitWhileExpanding, null);

            float normTime = 0;
            float distance = 0;
            float step;

            Vector2 size = new(0, _spriteRenderer.size.y);

            Vector2 aPos = _laserPointA.transform.localPosition;
            Vector2 bPos = _laserPointB.transform.localPosition;

            while (normTime < 1)
            {
                step = Time.deltaTime / duration;
                normTime += step;
                distance = length * normTime;

                size.x = distance;

                _spriteRenderer.size = size;

                aPos.x += length * step;
                bPos.x += length * step;

                _laserPointA.transform.localPosition = aPos;
                _laserPointB.transform.localPosition = bPos;

                yield return null;
            }

            _laserCaster.RemoveDetectionEvents(OnLaserHitWhileExpanding, null);
            _laserCaster.Toggle(false);

            _spriteRenderer.size = new Vector2(distance, size.y);

            _laserCollider.enabled = true;

            _nextDamageTime = Time.time + _damagePeriod;

            _expansionCoroutine = null;
        }
    }
}