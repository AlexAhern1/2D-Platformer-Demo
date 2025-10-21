using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectableTriggerHandler : MonoBehaviour
    {
        [SerializeField] private Tag _permittedTag;
        [SerializeField] private CollectableObject _collectable;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_permittedTag))
            {
                _collectable.Collect();
            }
        }
    }
}