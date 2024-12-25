    using UnityEngine;

    
    [CreateAssetMenu(fileName = "ItemInventory", menuName = "ScriptableObjects/Item")]
    
    public class ItemInventory : ScriptableObject
    {
        public ItemStats.ItemType Type;
    public ItemStats.ItemRarity Rarity;
        public string itemName;    // Tên item
        public GameObject itemPrefab; // chứa Item Stats
        public int currentQuantity = 1;   // Số lượng trong Túi
        public int totalQuantity = 0; // tổng số lượng 

        public void IncreaseQuantity(int amount)
        {
            currentQuantity += amount;
        }
        public void IncreaseQuantityTotal(int amount)
        {
            totalQuantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            currentQuantity = Mathf.Max(currentQuantity - amount, 0); // Không nhỏ hơn 0
        }
        public void DecreaseQuantityTotal(int amount)
        {
            totalQuantity = Mathf.Max(totalQuantity - amount, 0); // Không nhỏ hơn 0
        }
    }
