using UnityEngine;

namespace Escapist.Inventory
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Escapist/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity Details")]
        [SerializeField] private string itemId;
        [SerializeField] private string displayName;
        [TextArea(2, 4)] [SerializeField] private string description;

        [Header("System Rules")]
        [SerializeField] private Sprite icon;
        [SerializeField] private bool isStackable;
        [SerializeField] private int maxStackSize = 64;

        // Public getters exposing structural profiles safely to foreign assemblies
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool IsStackable => isStackable;
        public int MaxStackSize => isStackable ? maxStackSize : 1;
    }
}
