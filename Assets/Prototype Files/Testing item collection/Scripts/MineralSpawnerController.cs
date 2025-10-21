using UnityEngine;

namespace Game
{
    public class MineralSpawnerController : MonoBehaviour
    {
        [SerializeField] private ItemSpawner _spawner;
        [SerializeField] private BreakableObject _breakable;

        //will also handle the hitting / breaking fx (in the future)

        private void Awake()
        {
            //_breakable.Initialize();
        }

        private void OnEnable()
        {
            //_breakable.Enable();
            _breakable.BreakEvent += OnBroken;
        }

        private void OnDisable()
        {
            //_breakable.Disable();
            _breakable.BreakEvent -= OnBroken;
        }

        private void OnBroken(Damage dmg)
        {
            _spawner.Spawn();
            gameObject.SetActive(false);
        }
    }
}