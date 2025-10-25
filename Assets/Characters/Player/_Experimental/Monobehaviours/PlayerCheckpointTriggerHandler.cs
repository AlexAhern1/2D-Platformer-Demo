using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerCheckpointTriggerHandler : MonoBehaviour
    {
        [Header("Checkpoint settings")]
        [SerializeField] private Transform _checkpointTransform;
        [SerializeField] private Tag _playerTag;
        [SerializeField] private Vector2Reference _checkpointReference;

        [Header("Restpoint settings")]
        [SerializeField] Vector2Reference _restpointReference;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag)) UpdatePlayerCheckpoint();
        }

        private void UpdatePlayerCheckpoint()
        {
            _checkpointReference.Set(_checkpointTransform.position);

            if (_restpointReference.Variable == null) return;
            _restpointReference.Set(_checkpointTransform.position);
        }
    }
}