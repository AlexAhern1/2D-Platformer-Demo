using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// stores projectiles as inactive gameobjects so that other classes can use them if needed
    /// </summary>
    [CreateAssetMenu(fileName = "Projectile Pool", menuName = "SO/Managers/Object Pools/Projectile")]
    public class ProjectilePool : ScriptableObject
    {
        //key - projectile PREFAB, value - projectile INSTANCES
        private readonly Dictionary<Projectile, Stack<Projectile>> _inactiveProjectiles = new();
        private readonly Dictionary<Projectile, List<Projectile>> _activeProjectiles = new();

        private ProjectileManager _manager;

        public bool HasManager => _manager != null;

        public Projectile GetProjectile(Projectile prefab)
        {
            //check if the projectile prefab is part of the _inactiveProjectiles dictionary
            if (!_inactiveProjectiles.ContainsKey(prefab)) return null;

            //check if the stack is empty
            else if (_inactiveProjectiles[prefab].Count == 0)
            {
                Projectile newProjectileInstance = GameObject.Instantiate(prefab, _manager.transform);
                newProjectileInstance.gameObject.SetActive(false);
                _activeProjectiles[prefab].Add(newProjectileInstance);
                return newProjectileInstance;
            }

            //retreive the projectle prefab from the inactive dictionary
            Projectile retreivedProjectile = _inactiveProjectiles[prefab].Pop();
            _activeProjectiles[prefab].Add(retreivedProjectile);

            return retreivedProjectile;
        }

        public void SetProjectileAsInactive(Projectile prefab, Projectile instance)
        {
            if (!_activeProjectiles.ContainsKey(prefab)) return;
            _activeProjectiles[prefab].Remove(instance);
            _inactiveProjectiles[prefab].Push(instance);
        }

        public void RegisterProjectilePrefab(Projectile prefab)
        {
            if (_inactiveProjectiles.ContainsKey(prefab)) return;
            _inactiveProjectiles[prefab] = new Stack<Projectile>();
            _activeProjectiles[prefab] = new List<Projectile>();
        }

        public void UnregisterProjectilePrefab(Projectile prefab)
        {
            if (!_inactiveProjectiles.ContainsKey(prefab)) return;
            _inactiveProjectiles.Remove(prefab);
            _activeProjectiles.Remove(prefab);
        }

        public void ResetPool()
        {
            _inactiveProjectiles.Clear();
            _activeProjectiles.Clear();
        }

        public void SetManager(ProjectileManager manager)
        {
            _manager = manager;
        }
    }
}