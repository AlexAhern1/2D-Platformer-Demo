using UnityEngine;

namespace Game
{
    public class BreakableObstacleController : MonoBehaviour
    {
        [SerializeField] private BreakableObject _breakable;

        [SerializeField] private Shaker _shaker;

        [Header("Hit FX")]
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private AudioPlayer _hitSound;

        [Header("Break FX")]
        [SerializeField] private ParticleSystem _breakParticles;
        [SerializeField] private AudioPlayer _breakSound;

        private void OnEnable()
        {
            _breakable.HitEvent += OnHit;
            _breakable.BreakEvent += OnBreak;
        }

        private void OnDisable()
        {
            _breakable.HitEvent -= OnHit;
            _breakable.BreakEvent -= OnBreak;
        }

        private void OnBreak(Damage dmg)
        {
            _breakable.gameObject.SetActive(false);

            _breakSound.Play();
            Instantiate(_breakParticles, transform.position, Quaternion.identity).Play();
        }

        private void OnHit(Damage dmg)
        {
            float direction = Mathf.Sign(_shaker.transform.position.x - dmg.AttackSource.transform.position.x);
            _shaker.Shake(direction);

            _hitSound.Play();
            Instantiate(_hitParticles, transform.position, Quaternion.identity).Play();
        }
    }
}