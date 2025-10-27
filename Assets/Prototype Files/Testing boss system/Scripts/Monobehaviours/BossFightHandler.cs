using Game.Enemy;
using UnityEngine;

namespace Game
{
    public class BossFightHandler : MonoBehaviour
    {
        [SerializeField] private Tag _playerTag;

        [SerializeField] private Collider2D _triggerZone;

        [SerializeField] private GameObject[] _walls;

        [SerializeField] private EnemyController _enemy;

        [SerializeField] private ChangeCameraEvent _changeCameraEvent;
        [SerializeField] private CameraChangeSettings _changeSettings;
        [SerializeField] private FloatEvent _useDefaultCameraEventWithTransitionDuration;

        [SerializeField] private float _cameraTransitionDuration;

        [SerializeField] private GameEvent _playerDeathEvent;

        // state variables
        private bool _bossFightIsOngoing = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag)) StartBossFight();
        }

        private void OnDisable()
        {
            if (_bossFightIsOngoing)
            {
                RemoveObservers();
            }

            _bossFightIsOngoing = false;
        }

        private void StartBossFight()
        {
            _bossFightIsOngoing = true;

            // disable trigger zone
            _triggerZone.enabled = false;

            // enable walls
            for (int i = 0; i < _walls.Length; i++) _walls[i].SetActive(true);

            // activate boss healthbar.
            _enemy.EnableHealthMediatorExternally();

            // swap camera
            _changeCameraEvent.Raise(_changeSettings);

            // observe events - enemy die or player die
            _playerDeathEvent.AddEvent(OnPlayerDefeated);
            _enemy.AddDespawnObserver(OnBossDefeated);
        }

        private void OnBossDefeated()
        {
            _bossFightIsOngoing = false;

            // disable boss health
            _enemy.DisableHealthMediatorExternally();

            // disable walls
            for (int i = 0; i < _walls.Length; i++) _walls[i].SetActive(false);

            // reset camera
            _useDefaultCameraEventWithTransitionDuration.Raise(_cameraTransitionDuration);

            RemoveObservers();
        }

        private void OnPlayerDefeated()
        {
            _bossFightIsOngoing = false;

            // reset boss health
            _enemy.RestoreHealth();

            // disable boss hp bar
            _enemy.DisableHealthMediatorExternally();

            // disable walls
            for (int i = 0; i < _walls.Length; i++) _walls[i].SetActive(false);

            // re-enable trigger zone
            _triggerZone.enabled = true;

            RemoveObservers();
        }

        private void RemoveObservers()
        {
            _playerDeathEvent.RemoveEvent(OnPlayerDefeated);
            _enemy.RemoveDespawnObserver(OnBossDefeated);
        }
    }
}