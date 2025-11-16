using UnityEngine;

namespace Game.Player
{
    public class PlayerCheckpointRespawnHandler : MonoBehaviour
    {
        [Header("Player components")]
        [SerializeField] private PlayerMovementPhysics _physics;
        [SerializeField] private FXPlayer _fxPlayer;
        [SerializeField] private Animator _animator;
        [SerializeField] private StateMachine _playerFSM;

        [Header("Relevant states")]
        [SerializeField] private PlayerState _hazardRespawnState;
        [SerializeField] private PlayerState _deathState;
        [SerializeField] private PlayerState _idleState;

        [Header("Positioning")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Vector2Reference _checkpointReference;
        [SerializeField] private Vector2Reference _restpointReference;

        [Header("Events")]
        [SerializeField] private GameEvent _disableInputEvent;
        [SerializeField] private GameEvent _enableLevelInputEvent;
        [SerializeField] private GameEvent _enableCollisionEvent;
        [SerializeField] private GameEvent _disableCollisionEvent;
        [SerializeField] private GameEvent _playerDeathEvent;
        [SerializeField] private GameEvent _useDefaultCameraEvent;

        [Header("FX IDs")]
        [SerializeField] private int _nonLethalDamageFXID;
        [SerializeField] private int _lethalDamageFXID;
        [SerializeField] private int _deathDespawnFXID;

        [Header("Animation clips")]
        [SerializeField] private AnimationClip _takeLethalDamageAnimation;
        [SerializeField] private AnimationClip _playerDisappearAnimation;
        [SerializeField] private AnimationClip _playerReappearAnimation;
        [SerializeField] private string _idleAnimationName;

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

        [Header("Static data")]
        [SerializeField] private ResourceSO _playerCurrentStatic;
        [SerializeField] private Stat _playerMaxStatic;

        public async void HandleDeathRespawning()
        {
            _disableInputEvent.Raise();
            _disableCollisionEvent.Raise();

            _playerFSM.DoTransition(_deathState);

            _animator.Play(_takeLethalDamageAnimation.name);
            _fxPlayer.Play(_lethalDamageFXID);

            await AsyncHelpers.Wait(0.01f);

            _physics.SetGravityScale(0);
            _physics.SetMaxFallSpeed(0);
            _physics.Move(Vector2.zero, true);


            await AsyncHelpers.Wait(_delayBeforeDeathBlackScreen);

            _animator.Play(_playerDisappearAnimation.name);
            _fxPlayer.Play(_deathDespawnFXID);

            await AsyncHelpers.Wait(_delayForDeathParticles);

            _fadeInEvent.Raise(_fadeInTime);

            await AsyncHelpers.Wait(_fadeInTime);

            _playerDeathEvent.Raise();
            _playerTransform.position = _restpointReference.Value;

            // 8 SNAP CAMERA to player AND set camera mode to default. (just transition to default follow camera with transition time = 0 seconds)
            _useDefaultCameraEvent.Raise();

            // 8.5 let the black screen linger for a short while
            await AsyncHelpers.Wait(_lingerTime);

            // 9 play respawning animation (field: player animation clip + animation handler, similar to 3)
            _animator.Play(_playerReappearAnimation.name);

            // reset player health and static
            _playerCurrentHealth.Set(_playerMaxHealth.Value);
            _playerCurrentStatic.Set(_playerMaxStatic.Value);

            // 10 fade out black screen
            _fadeOutEvent.Raise(_fadeOutTime);

            // 11 wait for black screen to fade out completely
            await AsyncHelpers.Wait(_fadeOutTime);

            // 12 remove player invulnerability (similar to 2)

            // 13 re-enable player controls (similar to 1)
            _enableLevelInputEvent.Raise();
            _enableCollisionEvent.Raise();

            // 14 reset gravity scale and fall speed
            _physics.ResetGravityScale();
            _physics.ResetMaxFallSpeed();

            _animator.Play(_idleAnimationName);

            _playerFSM.DoTransition(_idleState);
        }

        public async void HandleHazardRespawning()
        {
            _disableCollisionEvent.Raise();
            _disableInputEvent.Raise();

            // here, need to force a transition into the hurt phase to ensure no airborne logic gets ran.
            _playerFSM.DoTransition(_hazardRespawnState);

            // -1 play FX (done here in case additional fx needs to be played AFTER respawning. this is an async method so it is a perfect place to set it up here.
            _fxPlayer.Play(_nonLethalDamageFXID);

            await AsyncHelpers.Wait(0.01f);

            // 0 set gravity scale, fall speed and velocity directions to 0
            _physics.SetGravityScale(0);
            _physics.SetMaxFallSpeed(0);
            _physics.Move(Vector2.zero, true);
            // 3 set player to be invulnerable (field: player damage config class)

            // 4 play damage animation (field: animtion clip + animation handler)

            // 4.5 wait for a tiny delay before fading in the black screen
            await AsyncHelpers.Wait(_delayBeforeStarting);

            // 5 fade in black screen
            _fadeInEvent.Raise(_fadeInTime);

            // 6 wait for black screen to fade in completely (await/coroutine event?)
            await AsyncHelpers.Wait(_fadeInTime);

            // 7 teleport player to spawn point (field: player spawnpoint data container)
            _playerTransform.position = _checkpointReference.Value;

            // 8 SNAP CAMERA to player

            // 8.5 let the black screen linger for a short while
            await AsyncHelpers.Wait(_lingerTime);

            // 9 play respawning animation (field: player animation clip + animation handler, similar to 3)

            // 10 fade out black screen
            _fadeOutEvent.Raise(_fadeOutTime);

            // 11 wait for black screen to fade out completely
            await AsyncHelpers.Wait(_fadeOutTime);

            // 12 remove player invulnerability (similar to 2)

            // 13 re-enable player controls (similar to 1)
            _enableLevelInputEvent.Raise();
            _enableCollisionEvent.Raise();

            // 14 reset gravity scale and fall speed
            _physics.ResetGravityScale();
            _physics.ResetMaxFallSpeed();

            _animator.Play(_idleAnimationName);

            _playerFSM.DoTransition(_idleState);
        }
    }
}