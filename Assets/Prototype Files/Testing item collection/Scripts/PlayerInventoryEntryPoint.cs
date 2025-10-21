using UnityEngine;

namespace Game
{
    /// <summary>
    /// IMPORTANT - THIS WILL MOST LIKELY HAVE TO BE INITIALIZED BY THE GAME INITIATOR.
    /// </summary>
    public class PlayerInventoryEntryPoint : MonoBehaviour
    {
        [SerializeField] private ItemEvent _collectableEvent;
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private bool _resetOnEnterPlaymode;

        [Header("Manual removing")]
        [SerializeField] private string _removalName;
        [SerializeField] private int _removalAmount;

        [ContextMenu("Manual Remove")]
        public void ManualRemove()
        {
            _inventory.TryRemove(_removalName, _removalAmount);
        }

        private void Awake()
        {
            if (_resetOnEnterPlaymode)
            {
                _inventory.Clear();
            }
        }

        private void OnEnable()
        {
            _collectableEvent.AddEvent(OnReceiveCollectable);
        }

        private void OnDisable()
        {
            _collectableEvent.RemoveEvent(OnReceiveCollectable);
        }

        private void OnReceiveCollectable(ItemData data)
        {
            _inventory.Add(data);
        }
    }
}