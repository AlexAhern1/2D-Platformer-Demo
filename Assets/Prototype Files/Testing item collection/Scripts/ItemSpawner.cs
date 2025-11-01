using UnityEngine;

namespace Game
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private string _itemName;

        [SerializeField] private SpawnableContext[] _spawnables;
        [SerializeField] private Transform _spawnpoint;

        [Header("Physics Configuration")]
        [SerializeField] private float _minSpawnVelocity;
        [SerializeField] private float _maxSpawnVelocity;
        [SerializeField] private float _spawnAngleDegrees;
        [SerializeField] private float _spreadRangeDegrees;
        [SerializeField] private float _minAngularVelocity;
        [SerializeField] private float _maxAngularVelocity;

        public void Spawn()
        {
            for (int i = 0; i < _spawnables.Length; i++)
            {
                for (int _ = 0; _ < _spawnables[i].Amount; _++)
                {
                    var spawnable = Instantiate(_spawnables[i].Spawnable);
                    spawnable.transform.position = _spawnpoint.position;

                    spawnable.Spawn();



                    if (spawnable.TryGetComponent(out Rigidbody2D rb2d)) ApplySpawnPhysics(rb2d);
                }
            }
        }

        private void ApplySpawnPhysics(Rigidbody2D rb2d)
        {
            // get a random initial spawn velocity
            float initVelocity = Random.Range(_minSpawnVelocity, _maxSpawnVelocity);

            // get a random spawn direction (degrees)
            float initDirectionRadians = Random.Range(_spawnAngleDegrees - _spreadRangeDegrees, _spawnAngleDegrees + _spreadRangeDegrees) * Mathf.Deg2Rad;

            // apply velocity
            rb2d.velocity = new Vector2(initVelocity * Mathf.Cos(initDirectionRadians), initVelocity * Mathf.Sin(initDirectionRadians));

            // get a random torque
            float torque = Random.Range(_minAngularVelocity, _maxAngularVelocity);

            rb2d.AddTorque(torque, ForceMode2D.Impulse);
        }
    }
}