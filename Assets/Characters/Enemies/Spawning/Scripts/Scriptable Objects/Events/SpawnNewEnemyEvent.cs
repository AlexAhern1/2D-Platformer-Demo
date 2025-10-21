using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "Spawn New Enemy Event", menuName = "SO/Game Events/Spawn Enemy")]
    public class SpawnNewEnemyEvent : GameEvent<EnemySpawnableOLD> { }
}