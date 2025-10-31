using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class InventoryDisplayer : MonoBehaviour, IEnable, IInitializable
    {
        [Header("Input Events")]
        [SerializeField] private InputEvent<float> _openInventoryInput;
        [SerializeField] private InputEvent<float> _closeInventoryInput;
        [SerializeField] private GameEvent _enableMenusMapEvent;
        [SerializeField] private GameEvent _enableLevelMapEvent;

        [Header("Inventory Events")]
        [SerializeField] private ItemEvent _collectItemEvent;

        [Header("Relevant Objects")]
        [SerializeField] private GameObject _fullPanel;
        [SerializeField] private GameObject _collectorpanel;

        [Header("Inventory")]
        [SerializeField] private InventorySO _inventory;

        // dictionary containing all panels
        Dictionary<string, InventoryPanel> _panelDictionary = new();

        public void Initialize()
        {
            foreach (var panel in _collectorpanel.GetComponentsInChildren<InventoryPanel>())
            {
                var text = panel.NameText;
                _panelDictionary[text.text] = panel;
                panel.SetAmount(0); //once saving and loading gets implemented, this must be modified.
            }
        }

        public void Enable()
        {
            _openInventoryInput.AddEvent(OnPressOpenInventory);
            _closeInventoryInput.AddEvent(OnPressCloseInventory);
        }

        public void Disable()
        {
            _openInventoryInput.RemoveEvent(OnPressOpenInventory);
            _closeInventoryInput.RemoveEvent(OnPressCloseInventory);
        }

        private void OnPressOpenInventory(float inputData)
        {
            if (inputData <= 0) return;

            _fullPanel.SetActive(true);
            _enableMenusMapEvent.Raise();

            //update all panel entries.
            foreach (string itemName in _inventory.contents.Keys)
            {
                _panelDictionary[itemName].SetAmount(_inventory.contents[itemName]);
            }
        }

        private void OnPressCloseInventory(float inputData)
        {
            if (inputData <= 0) return;

            _fullPanel.SetActive(false);
            _enableLevelMapEvent.Raise();
        }
    }
}