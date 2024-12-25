using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public enum ItemType { Weapon, Helmet, Armor, Shoe, Ring, Pet } // Loại Item
    public enum ItemRarity { Common, Rare, Legendary } //Loại Rarity

    [Header("Stat Item")]
    public ItemType Type;
    [SerializeField] public GameObject EquipButton;
    [SerializeField] public GameObject UnequipButton;
    [SerializeField] public int Attack = 0; // Giá trị Attack của item
    [SerializeField] public int Health = 0; // Giá trị Health của item
    [SerializeField] private Asset playerAsset; // Tham chiếu đến Asset người chơi
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private AssetDisplay assetDisplay;
    [SerializeField] private ItemInventory itemInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemData itemData; // Tham chiếu đến ItemData

    public bool isEquipped = false; // Trạng thái trang bị

    private void Start()
    {
        if (assetDisplay == null)
        {
            assetDisplay = FindObjectOfType<AssetDisplay>();
        }
        itemManager = FindObjectOfType<ItemManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void Equip()
    {
        if (isEquipped)
        {
            Debug.LogWarning("Item is already equipped!");
            return;
        }

        if (playerAsset != null && itemManager != null)
        {
            EquipButton.SetActive(false);
            UnequipButton.SetActive(true);
            itemInventory.isEquipped = true;

            // Di chuyển object sang panel tương ứng
            itemManager.EquipItem(this);

            // Trừ số lượng hiện trong túi
            itemInventory.DecreaseQuantity(1);

            // Thêm item vào ItemData dựa trên loại item
            itemData.SetItem(itemInventory);

            // Thêm stat cho nhân vật
            if (Attack != 0) playerAsset.AddStatPlayer(Stats.StatsType.Attack, Attack);
            if (Health != 0) playerAsset.AddStatPlayer(Stats.StatsType.Health, Health);

            // Cập nhật lại UI
            assetDisplay.UpdateDisplay();
            inventoryUI.UpdateUI();

            isEquipped = true;
            Debug.Log($"Equipped {gameObject.name}. Added Attack: {Attack}, Health: {Health}.");
        }
        else
        {
            Debug.LogError("Player Asset hoặc Item Manager chưa được gán!");
        }
    }

    public void Unequip()
    {
        if (!isEquipped)
        {
            Debug.LogWarning("Item is not equipped!");
            return;
        }

        if (playerAsset != null && itemManager != null)
        {
            UnequipButton.SetActive(false);
            EquipButton.SetActive(true);
            itemInventory.isEquipped = false;

            // Cộng số lượng hiện trong túi
            itemInventory.IncreaseQuantity(1);

            // Di chuyển object trở lại inventory panel
            itemManager.UnequipItem(Type);

            // Xóa item từ ItemData
            itemData.RemoveItem(Type);

            // Trừ stat của nhân vật
            if (Attack != 0) playerAsset.MinusStatPlayer(Stats.StatsType.Attack, Attack);
            if (Health != 0) playerAsset.MinusStatPlayer(Stats.StatsType.Health, Health);

            isEquipped = false;
            // cập nhập UI
            assetDisplay.UpdateDisplay();
            inventoryUI.UpdateUI();

            Debug.Log($"Unequipped {gameObject.name}. Removed Attack: {Attack}, Health: {Health}.");
        }
        else
        {
            Debug.LogError("Player Asset hoặc Item Manager chưa được gán!");
        }
    }
}
