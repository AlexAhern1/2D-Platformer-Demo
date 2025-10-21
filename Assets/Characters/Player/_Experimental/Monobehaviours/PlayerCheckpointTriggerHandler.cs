using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerCheckpointTriggerHandler : MonoBehaviour
    {
        [SerializeField] private Transform _checkpointTransform;
        [SerializeField] private Tag _playerTag;
        [SerializeField] private Vector2Reference _checkpointReference;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag)) UpdatePlayerCheckpoint();
        }

        private void UpdatePlayerCheckpoint()
        {
            _checkpointReference.Set(_checkpointTransform.position);
        }
    }
}