using System.Threading.Tasks;
using UnityEngine;

namespace Game.Player
{
    public class PlayerCheckpointRespawnHandler : MonoBehaviour
    {
        [Header("Player components")]
        [SerializeField] private PlayerMovementPhysics _physics;
        [SerializeField] private FXPlayer _fxPlayer;
        [SerializeField] private Animator _animator;

        [Header("Positioning")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Vector2Reference _checkpointReference;
        [SerializeField] private Vector2Reference _restpointReference;

        [Header("Input")]
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private GameEvent _enableLevelInputEvent;

        [Header("FX IDs")]
        [SerializeField] private int _nonLethalDamageFXID;
        [SerializeField] private int _lethalDamageFXID;
        [SerializeField] private int _deathDespawnFXID;

        [Header("Animation clips")]
        [SerializeField] private AnimationClip _takeLethalDamageAnimation;
        [SerializeField] private AnimationClip _playerDisappearAnimation;
        [SerializeField] private AnimationClip _playerReappearAnimation;
        [SerializeField] private AnimationClip _idleAnimation;

        [Header("black screen events")]
        [SerializeField] private FloatEvent _fadeInEvent;
        [SerializeField] private FloatEvent _fadeOutEvent;

        [Header("hazard settings")]
        [SerializeField] private float _delayBeforeStarting;
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _lingerTime;
        [SerializeField] private float _fadeOutTime;

        [Header("death settings")]
        [SerializeField] private float _delayBeforeDeathBlackScreen;
        [SerializeField] private float _delayForDeathParticles;

        [Header("Health data")]
        [SerializeField] private ResourceSO _playerCurrentHealth;
        [SerializeField] private Stat _playerMaxHealth;

        public async void HandleDeathRespawning()
        {
            _disableInputEvent.Raise();

            _physics.SetGravityScale(0);
            _physics.SetMaxFallSpeed(0);
            _physics.Move(Vector2.zero, true);

            _animator.Play(_takeLethalDamageAnimation.name);
            _fxPlayer.Play(_lethalDamageFXID);

            await Wait(_delayBeforeDeathBlackScreen);

            _animator.Play(_playerDisappearAnimation.name);
            _fxPlayer.Play(_deathDespawnFXID);

            await Wait(_delayForDeathParticles);

            _fadeInEvent.Raise(_fadeInTime);

            await Wait(_fadeInTime);

            _playerTransform.position = _restpointReference.Value;

            // 8 SNAP CAMERA to player

            // 8.5 let the black screen linger for a short while
            await Wait(_lingerTime);

            // 9 play respawning animation (field: player animation clip + animation handler, similar to 3)
            _animator.Play(_playerReappearAnimation.name);

            // reset player health
            _playerCurrentHealth.Set(_playerMaxHealth.Value);

            // 10 fade out black screen
            _fadeOutEvent.Raise(_fadeOutTime);

            // 11 wait for black screen to fade out completely
            await Wait(_fadeOutTime);

            // 12 remove player invulnerability (similar to 2)

            // 13 re-enable player controls (similar to 1)
            _enableLevelInputEvent.Raise();

            // 14 reset gravity scale and fall speed
            _physics.ResetGravityScale();
            _physics.ResetMaxFallSpeed();
        }

        public async void HandleHazardRespawning()
        {
            // -1 play FX (done here in case additional fx needs to be played AFTER respawning. this is an async method so it is a perfect place to set it up here.
            _fxPlayer.Play(_nonLethalDamageFXID);


            // 0 set gravity scale, fall speed and velocity directions to 0
            _physics.SetGravityScale(0);
            _physics.SetMaxFallSpeed(0);
            _physics.Move(Vector2.zero, true);

            // 1 disable any currently displaying UI (event)

            // 2 disable player controls (event)
            _disableInputEvent.Raise();

            // 3 set player to be invulnerable (field: player damage config class)

            // 4 play damage animation (field: animtion clip + animation handler)

            // 4.5 wait for a tiny delay before fading in the black screen
            await Wait(_delayBeforeStarting);

            // 5 fade in black screen
            _fadeInEvent.Raise(_fadeInTime);

            // 6 wait for black screen to fade in completely (await/coroutine event?)
            await Wait(_fadeInTime);

            // 7 teleport player to spawn point (field: player spawnpoint data container)
            _playerTransform.position = _checkpointReference.Value;

            // 8 SNAP CAMERA to player

            // 8.5 let the black screen linger for a short while
            await Wait(_lingerTime);

            // 9 play respawning animation (field: player animation clip + animation handler, similar to 3)

            // 10 fade out black screen
            _fadeOutEvent.Raise(_fadeOutTime);

            // 11 wait for black screen to fade out completely
            await Wait(_fadeOutTime);

            // 12 remove player invulnerability (similar to 2)

            // 13 re-enable player controls (similar to 1)
            _enableLevelInputEvent.Raise();

            // 14 reset gravity scale and fall speed
            _physics.ResetGravityScale();
            _physics.ResetMaxFallSpeed();
        }

        // waiting method - to be moved into static async helpers
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