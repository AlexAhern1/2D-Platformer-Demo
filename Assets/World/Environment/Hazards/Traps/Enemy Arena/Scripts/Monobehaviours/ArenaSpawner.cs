using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class ArenaSpawner : MonoBehaviour, ITrapComponent
    {
        [SerializeField] private string _sceneName;
        [Separator]

        [SerializeField] private float _delayBeforeStarting;
        private int _waveNumber;

        private int _waveKillCount;
        private int _waveSize;

        [Separator]
        [SerializeField] private List<EnemyWave> _waves;

        [Header("Scriptable Objects")]
        [SerializeField] private EnemyTracker _enemyTracker;
        //[SerializeField] private SpawnEnemyEvent _spawnEnemyEvent;

        public Action ReleaseEvent { get; set; }

        public void Entrap()
        {
            SpawnNextWave();
        }

        private void SpawnEnemyWave(int waveNumber)
        {
            EnemyWave enemyWave = _waves[waveNumber];
            _waveSize = enemyWave.EnemyCount();
            _waveKillCount = 0;

            foreach (WaveDistribution distribution in enemyWave.WaveDistribution)
            {
                SpawnEnemies(distribution);
            }
        }

        private void SpawnEnemies(WaveDistribution distribution)
        {
            EnemySpawnableOLD spawnable = distribution.Spawnable;
            //EnemyConfigAsset config = distribution.Config;

            foreach (Transform spawnpoint in distribution.Spawnpoints)
            {
                //EnemyDefenseConfig defenseConfig = new EnemyDefenseConfig(config.Defenses);
                //defenseConfig.RestoreHealth();

                //EnemyStatus status = new EnemyStatus(defenseConfig, _sceneName, spawnable, spawnpoint.position);

                EnemySpawnableOLD enemyInstance = GetEnemyInstance(spawnable);

                //enemyInstance.Spawn();

                //enemyInstance.SetSpawnData(status);
                //enemyInstance.DespawnEvent += OnEnemyDespawned;

                //_enemyTracker.StartTrackingEnemy(_sceneName, enemyInstance);
                //_enemyTracker.AddEnemyVariant(spawnable, enemyInstance);
            }
        }

        private void OnEnemyDespawned(EnemySpawnableOLD instance)
        {
            //instance.DespawnEvent -= OnEnemyDespawned;

            _waveKillCount++;
            if (_waveKillCount == _waveSize)
            {
                OnWaveCleared();
            }
        }

        private void OnWaveCleared()
        {
            _waveNumber++;
            if (_waveNumber == _waves.Count)
            {
                EndArenaAfterDelay();
                return;
            }
            SpawnNextWave();
        }

        private void EndArena()
        {
            ReleaseEvent?.Invoke();
        }

        private EnemySpawnableOLD GetEnemyInstance(EnemySpawnableOLD spawnable)
        {
            //EnemySpawnable instance = _enemyTracker.GetInactiveEnemyOfVariant(spawnable);
            //if (instance == null) { instance = _spawnEnemyEvent.Raise(spawnable); }
            //return instance;
            return null;
        }

        private void SpawnNextWave()
        {
            StartCoroutine(StartSpawningAfterDelay(_delayBeforeStarting));
        }

        private IEnumerator StartSpawningAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            SpawnEnemyWave(_waveNumber);
        }

        private void EndArenaAfterDelay()
        {
            StartCoroutine(EndingArenaAfterDelay(_delayBeforeStarting));
        }

        private IEnumerator EndingArenaAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            EndArena();
        }
    }

    [Serializable]
    public class EnemyWave
    {
        public List<WaveDistribution> WaveDistribution;

        public int EnemyCount()
        {
            int count = 0;
            for (int i = 0; i < WaveDistribution.Count; i++)
            {
                count += WaveDistribution[i].Spawnpoints.Length;
            }
            return count;
        }
    }

    [Serializable]
    public class WaveDistribution
    {
        public EnemySpawnableOLD Spawnable;
        //public EnemyConfigAsset Config;

        [Separator]
        public Transform[] Spawnpoints;
    }
}