using UnityEngine;

namespace Game
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private ProjectilePool _projectilePool;

        private void Awake()
        {
            _projectilePool.ResetPool();
            _projectilePool.SetManager(this);
        }
    }
}