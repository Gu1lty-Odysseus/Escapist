using System;

namespace Escapist.Inventory
{
    [Serializable]
    public class InventoryInstance
    {
        public ItemData Data { get; private set; }
        public int Quantity { get; private set; }

        public InventoryInstance(ItemData data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }

        public void AddQuantity(int amount) => Quantity += amount;
        public void RemoveQuantity(int amount) => Quantity = Math.Max(0, Quantity - amount);
    }
}