using System;
using System.Collections.Generic;
using UnityEngine;

namespace Escapist.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        // Singleton pattern accessor for clear cross-subsystem state hooks
        public static InventoryManager Instance { get; private set; }

        [Header("Storage Adjustments")]
        [SerializeField] private int totalInventorySlots = 16;

        // Internal structural storage container mapping
        private List<InventoryInstance> internalSlots = new List<InventoryInstance>();

        // Pure C# Event hook decoupling UI redraw operations from backend mutations
        public static event Action OnInventoryUpdated;

        public int TotalInventorySlots => totalInventorySlots;
        public IReadOnlyList<InventoryInstance> InventorySlots => internalSlots;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Attempts to add an item to the collection based on stacking rules and capacity limits.
        /// </summary>
        public bool AddItem(ItemData item, int amount = 1)
        {
            if (item == null || amount <= 0) return false;

            // Step A: Attempt processing via stack merging operations if item permits stacking
            if (item.IsStackable)
            {
                foreach (var slot in internalSlots)
                {
                    if (slot.Data.ItemId == item.ItemId && slot.Quantity < item.MaxStackSize)
                    {
                        int spacesRemaining = item.MaxStackSize - slot.Quantity;
                        int amountToPush = Mathf.Min(spacesRemaining, amount);
                        
                        slot.AddQuantity(amountToPush);
                        amount -= amountToPush;

                        if (amount <= 0)
                        {
                            OnInventoryUpdated?.Invoke();
                            return true;
                        }
                    }
                }
            }

            // Step B: Attempt pushing residual capacities to completely new allocations
            while (amount > 0)
            {
                if (internalSlots.Count >= totalInventorySlots)
                {
                    Debug.LogWarning("Inventory capacity constraints reached. Item bounce occurred.");
                    OnInventoryUpdated?.Invoke();
                    return false; 
                }

                int sliceAmount = Mathf.Min(amount, item.MaxStackSize);
                internalSlots.Add(new InventoryInstance(item, sliceAmount));
                amount -= sliceAmount;
            }

            OnInventoryUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// Removes a specific quantity of an item tracking by asset ID across allocation spaces.
        /// </summary>
        public bool RemoveItem(string itemId, int amount = 1)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;

            // Verify availability thresholds first
            int totalFound = GetTotalItemCount(itemId);
            if (totalFound < amount) return false;

            // Iterate backwards through lists when executing systemic drop manipulations safely
            for (int i = internalSlots.Count - 1; i >= 0; i--)
            {
                if (internalSlots[i].Data.ItemId == itemId)
                {
                    if (internalSlots[i].Quantity > amount)
                    {
                        internalSlots[i].RemoveQuantity(amount);
                        amount = 0;
                    }
                    else
                    {
                        amount -= internalSlots[i].Quantity;
                        internalSlots.RemoveAt(i);
                    }

                    if (amount <= 0) break;
                }
            }

            OnInventoryUpdated?.Invoke();
            return true;
        }

        public int GetTotalItemCount(string itemId)
        {
            int tally = 0;
            foreach (var slot in internalSlots)
            {
                if (slot.Data.ItemId == itemId) tally += slot.Quantity;
            }
            return tally;
        }
    }
}