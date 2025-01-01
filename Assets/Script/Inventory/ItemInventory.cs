    using UnityEngine;


[CreateAssetMenu(fileName = "ItemInventory", menuName = "ScriptableObjects/Item")]
    
    public class ItemInventory : ScriptableObject
    {
        [Header("Item Stats")]
        [SerializeField] public int Attack = 0; // Giá trị Attack của item
        [SerializeField] public int Health = 0; // Giá trị Health của item
        [SerializeField] public int CurrentStart = 0; // sao của item
        public string Decription; // Mô tả item
        public ItemStats.ItemType Type;
        public ItemStats.ItemRarity Rarity;
        public bool isEquipped = false;
        public string itemName;    // Tên item
        public Sprite Icon; //chứa icon của item
        public GameObject itemPrefab; // chứa Item Stats
        public int currentQuantity = 1;   // Số lượng trong Túi

        public void IncreaseQuantity(int amount)
        {
            currentQuantity += amount;
        }     
        public void DecreaseQuantity(int amount)
        {
            currentQuantity = Mathf.Max(currentQuantity - amount, 0); // Không nhỏ hơn 0
        }  
        public void IncreaseStart(int amount)
        {
            CurrentStart= Mathf.Min(CurrentStart + amount, 5); // Không lớn hơn 5
        }
    }
