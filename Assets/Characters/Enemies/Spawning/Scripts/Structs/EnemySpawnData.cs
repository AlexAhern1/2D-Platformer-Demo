using UnityEngine;

namespace Game.Enemy
{
    public struct EnemySpawnData
    {
        public string EnemyGuid { get; set; }

        public Vector2 Spawnpoint { get; set; }

        public float CurrentHealth { get; set; }

        public bool IsDead => CurrentHealth == 0;

        public static EnemySpawnData New(string guid, Vector2 spawnpoint, float currentHealth)
        {
            return new EnemySpawnData { EnemyGuid = guid, Spawnpoint = spawnpoint, CurrentHealth = currentHealth };
        }
    }
}