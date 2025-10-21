using System;
using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    /// <summary>
    /// spawnable gameObject used for all enemies(except maybe bosses) in the game.
    /// </summary>
    public class EnemySpawnableOLD : MonoBehaviour
    {
        [SerializeField] private float _secondsBeforeDespawning;
        [ReadOnly][SerializeField] private string _guid;

        public string GUID => _guid;

        public Action<EnemySpawnableOLD> DespawnEvent { get; set; }

        public Action ResetSpawnDataEvent { get; set; }

        public Action<EnemySpawnData> SetSpawnDataEvent { get; set; }

        public Func<EnemySpawnData> SpawnDataGetter { get; set; }

        public void GenerateGuid()
        {
            _guid = Guid.NewGuid().ToString();
            Logger.Log($"New Guid generated for {gameObject.name} - {_guid}");
        }

        public void DespawnEnemyAfterSeconds()
        {
            StartCoroutine(WaitBeforeDespawning(_secondsBeforeDespawning));
        }

        public void DespawnEnemyImmediately()
        {
            DespawnEvent?.Invoke(this);
        }

        public void ResetSpawnData()
        {
            ResetSpawnDataEvent?.Invoke();
        }

        public void SetSpawnData(EnemySpawnData spawnData)
        {
            SetSpawnDataEvent?.Invoke(spawnData);
        }

        public EnemySpawnData GetSpawnData()
        {
            return SpawnDataGetter();
        }

        private IEnumerator WaitBeforeDespawning(float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            DespawnEnemyImmediately();
        }
    }
}