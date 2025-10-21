namespace Game.World
{
    public class ArenaTrap : DoorTrap
    {
        private ITrapComponent _enemyArenaSpawner;

        private void Awake()
        {
            _enemyArenaSpawner = GetComponent<ITrapComponent>();
        }

        private void OnEnable()
        {
            _enemyArenaSpawner.ReleaseEvent += OnRelease;
        }

        private void OnDisable()
        {
            _enemyArenaSpawner.ReleaseEvent -= OnRelease;
        }

        protected override void OnPlayerTriggerTrap()
        {
            LockDoors();
            _enemyArenaSpawner.Entrap();
        }

        private void OnRelease()
        {
            UnlockDoors();
        }
    }
}