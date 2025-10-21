using UnityEngine;

namespace Game.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Enemy tracker")]
        [SerializeField] private EnemyTracker _enemyTracker;

        [Header("Events")]
        [SerializeField] private GameEvent<EnemySpawnableOLD> _newEnemySpawnedEvent;

        private void OnEnable()
        {
            _newEnemySpawnedEvent.AddEvent(OnNewEnemySpawned);
        }

        private void OnDisable()
        {
            _newEnemySpawnedEvent.RemoveEvent(OnNewEnemySpawned);
        }

        private void Awake()
        {
            _enemyTracker.ClearSavedEnemyData();
        }

        private void OnDestroy()
        {
            _enemyTracker.ClearSavedEnemyData();
        }

        private void OnNewEnemySpawned(EnemySpawnableOLD enemy)
        {
            enemy.transform.parent = transform;
        }
    }
}