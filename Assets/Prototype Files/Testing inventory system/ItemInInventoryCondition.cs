using System;

namespace Game
{
    [Serializable]
    public class ItemInInventoryCondition : ICondition
    {
        public string ItemName;
        public int ItemAmount;
        public InventorySO Inventory;
        public bool CheckIfTrue;

        public bool Evaluate() => Inventory.ItemIsInInventory(ItemName, ItemAmount) == CheckIfTrue;
    }
}