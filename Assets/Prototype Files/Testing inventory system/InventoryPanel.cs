
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _amountText;

        private int _actualAmount;

        public TMP_Text NameText => _nameText;

        public TMP_Text AmountText => _amountText;

        public void SetAmount(int amount)
        {
            _actualAmount = amount;
            _amountText.text = $"{_actualAmount}";
        }
    }
}