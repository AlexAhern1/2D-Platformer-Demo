using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Tracker")]
        [SerializeField] private EnemyTracker _tracker;

        [Header("Scene Enemies")]
        [SerializeField] private EnemySpawnableOLD[] _enemies;

        private string SceneName => gameObject.scene.name;

        private void RetreiveEnemies()
        {
            if (_tracker.IsSceneRegistered(SceneName))
            {
                HideDuplicateEnemies();
                RespawnSceneEnemies();
                return;
            }
            SpawnSceneEnemiesFirstTime();
        }

        private void DespawnEnemies()
        {
            _tracker.DespawnSceneEnemies(SceneName);
        }

        private void RespawnSceneEnemies()
        {
            _tracker.RespawnSceneEnemies(SceneName);
        }

        private void SpawnSceneEnemiesFirstTime()
        {
            _tracker.SpawnEnemiesFirstTime(SceneName, _enemies);
        }

        private void HideDuplicateEnemies()
        {
            for (int i = 0; i < _enemies.Length; i++)
            {
                _enemies[i].gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            RetreiveEnemies();
        }

        private void OnDestroy()
        {
            DespawnEnemies();
        }
    }
}