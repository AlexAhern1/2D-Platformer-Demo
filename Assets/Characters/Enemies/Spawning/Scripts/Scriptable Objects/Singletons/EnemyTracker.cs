using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    /// <summary>
    /// used to keep track of all currently spawned enemies
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Tracker", menuName = "SO/Managers/Trackers/Enemy")]
    public class EnemyTracker : ScriptableObject
    {
        [SerializeField] private SpawnNewEnemyEvent _spawnNewEnemyEvent;

        //key: guid
        private readonly Dictionary<string, Stack<EnemySpawnableOLD>> _enemyPool = new();

        //key: scene
        private readonly Dictionary<string, List<EnemySpawnableOLD>> _sceneEnemies = new();

        //key: scene
        private readonly Dictionary<string, List<EnemySpawnData>> _sceneEnemyData = new();

        //key: guid
        private readonly Dictionary<string, EnemySpawnableOLD> _spawnableIDTable = new();

        public bool IsSceneRegistered(string scene) => _sceneEnemies.ContainsKey(scene);

        public void SpawnEnemiesFirstTime(string scene, EnemySpawnableOLD[] enemies)
        {
            _sceneEnemies[scene] = new List<EnemySpawnableOLD>();
            for (int i = 0; i < enemies.Length; i++)
            {
                EnemySpawnableOLD originalEnemy = enemies[i];
                string id = originalEnemy.GUID;

                EnemySpawnableOLD poolEnemy = Pull(id);

                if (!_spawnableIDTable.ContainsKey(id))
                {
                    _spawnableIDTable[id] = originalEnemy;
                }

                if (poolEnemy != null)
                {
                    originalEnemy.gameObject.SetActive(false);

                    poolEnemy.gameObject.SetActive(true);
                    poolEnemy.ResetSpawnData();

                    poolEnemy.transform.position = originalEnemy.transform.position;

                    _sceneEnemies[scene].Add(poolEnemy);
                }
                else
                {
                    _sceneEnemies[scene].Add(originalEnemy);
                    _spawnNewEnemyEvent.Raise(originalEnemy);
                }
            }
        }

        public void DespawnSceneEnemies(string scene)
        {
            if (!_sceneEnemies.ContainsKey(scene))
            {
                Logger.Warn("INVESTIGATE");
                return;
            }
            List<EnemySpawnableOLD> enemies = _sceneEnemies[scene];


            if (!_sceneEnemyData.ContainsKey(scene))
            {
                _sceneEnemyData[scene] = new List<EnemySpawnData>();
            }
            else
            {
                _sceneEnemyData[scene].Clear();
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemySpawnableOLD enemy = enemies[i];

                //save data
                EnemySpawnData data = enemy.GetSpawnData();
                _sceneEnemyData[scene].Add(data);

                _enemyPool[enemy.GUID].Push(enemy);

                enemy.gameObject.SetActive(false);
            }

            _sceneEnemies[scene].Clear();
        }

        public void RespawnSceneEnemies(string scene)
        {
            List<EnemySpawnData> spawnData = _sceneEnemyData[scene];

            for (int i = 0; i < spawnData.Count; i++)
            {
                EnemySpawnData data = spawnData[i];
                string id = data.EnemyGuid;

                EnemySpawnableOLD poolEnemy = Pull(id);
                if (poolEnemy == null)
                {
                    poolEnemy = Instantiate(_spawnableIDTable[id]);
                    _spawnNewEnemyEvent.Raise(poolEnemy);
                }
                else
                {
                    poolEnemy.gameObject.SetActive(true);
                }

                _sceneEnemies[scene].Add(poolEnemy);
                poolEnemy.SetSpawnData(data);   
            }

            _sceneEnemyData[scene].Clear();
        }

        private EnemySpawnableOLD Pull(string guid)
        {
            if (!_enemyPool.ContainsKey(guid))
            {
                _enemyPool[guid] = new Stack<EnemySpawnableOLD>();
                return null;
            }
            else if (_enemyPool[guid].Count == 0) return null;
            return _enemyPool[guid].Pop();
        }

        public void ClearSavedEnemyData()
        {
            _enemyPool.Clear();
            _sceneEnemies.Clear();
            _sceneEnemyData.Clear();
            _spawnableIDTable.Clear();
        }
    }
}