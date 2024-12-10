    using UnityEngine;

    [CreateAssetMenu(fileName = "ItemInventory", menuName = "ScriptableObjects/Item")]
    public class ItemInventory : ScriptableObject
    {
        public string itemName;    // Tên item
        public Sprite itemIcon;    // Hình ảnh biểu tượng item
        public GameObject itemPrefab; // chứa Item Stats
        public int quantity = 1;   // Số lượng item

        public void IncreaseQuantity(int amount)
        {
            quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            quantity = Mathf.Max(quantity - amount, 0); // Không nhỏ hơn 0
        }
    }
