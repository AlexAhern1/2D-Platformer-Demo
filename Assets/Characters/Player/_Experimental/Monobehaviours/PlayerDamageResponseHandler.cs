using UnityEngine;

namespace Game.Player
{
    public class PlayerDamageResponseHandler : MonoBehaviour
    {
        [Header("Player Monobehaviours")]
        [SerializeField] private PlayerCheckpointRespawnHandler _checkpointRespawnHandler;

        [Header("Scriptable Object Resources")]
        [SerializeField] private ResourceSO _playerHealthResource;

        public void ProcessDamageAppliedToPlayer(Damage damageData, DamageTaken damageTakenData)
        {
            // possible responses:
            // 1) take hazard damage
            if (damageData.Weapon == WeaponType.Hazard)
            {
                ProcessHazardDamage(damageTakenData.TotalDamage);
            }
            // 2) take normal damage
            else
            {
                ProcessNormalDamage();
            }
        }

        private void ProcessHazardDamage(float damageTaken)
        {
            _playerHealthResource.Add(-damageTaken);
            bool playerIsAlive = _playerHealthResource.Current > 0;

            if (playerIsAlive) _checkpointRespawnHandler.HandleHazardRespawning();
            else  _checkpointRespawnHandler.HandleDeathRespawning();
        }

        private void ProcessNormalDamage()
        {
            Logger.Log("normal damage.");
        }
    }
}