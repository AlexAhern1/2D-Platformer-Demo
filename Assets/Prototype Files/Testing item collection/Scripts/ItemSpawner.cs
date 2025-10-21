using UnityEngine;

namespace Game
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private string _itemName;

        [SerializeField] private Spawnable[] _spawnables;
        [SerializeField] private Transform _spawnpoint;
        
        public void Spawn()
        {
            for (int i = 0; i < _spawnables.Length; i++)
            {
                var spawnable = Instantiate(_spawnables[i]);
                spawnable.transform.position = _spawnpoint.position;

                spawnable.Spawn();
            }
        }
    }
}