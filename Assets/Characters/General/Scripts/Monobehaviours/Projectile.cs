using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _rb2d;

        [Header("Trigger config")]
        [SerializeField] private Tag _obstacleTag;

        [Header("Orientation Config")]
        [SerializeField] private bool _alignWithInitialVelocity;

        [Header("Do this for as long as there's no projectile pool.")]
        [SerializeField] private bool _destroyOnLifetimeOver = true;
        [SerializeField][Range(-180, 180)] private float _angleOffset;

        private Damage _projectileDamage;
        private float _lifetime;

        private float _lifeEndTime;

        public Action<Projectile> DespawnEvent { get; set; }

        public void SetProjectileData(Damage damage, float lifetime)
        {
            damage.AttackSource = gameObject;
            _projectileDamage = damage;
            _lifetime = lifetime;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rb2d.velocity = velocity;
            Logger.Log($"set velocity: {velocity}, rb velocity: {_rb2d.velocity}", Logger.EnemyCombat);

            // check if the projectile should align itself with its velocity.
            if (_alignWithInitialVelocity)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(velocity.y, velocity.x) + _angleOffset);
            }
        }

        public void SetGravityScale(float gravityScale) => _rb2d.gravityScale = gravityScale;

        public void Spawn()
        {
            _lifeEndTime = Time.time + _lifetime;
            gameObject.SetActive(true);

            Logger.Log($"actual velocity: {_rb2d.velocity}", Logger.EnemyCombat);
        }

        public void Despawn()
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (Time.time < _lifeEndTime) return;
            DespawnEvent?.Invoke(this);

            if (_destroyOnLifetimeOver) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_obstacleTag))
            {
                DespawnEvent?.Invoke(this);
                _lifeEndTime = 0;


                return;
            }

            //check for an IDamageable interface.
            if (collision.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_projectileDamage);
            }
        }
    }
}