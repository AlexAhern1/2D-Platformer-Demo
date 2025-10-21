using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawnable : Spawnable
    {
        [SerializeField] private bool _hasSpawner = false;
        [SerializeField] private EnemyController _controller;

        private void Awake()
        {
            _controller.Initialize();
        }

        private void OnEnable()
        {
            _controller.Enable();
        }

        private void OnDisable()
        {
            _controller.Disable();
        }

        public override void Spawn()
        {
            gameObject.SetActive(true);
        }

        public override void Despawn()
        {
            gameObject.SetActive(false);
            if (_hasSpawner)
            {
                //do spawner logic.
            }
        }
    }
}