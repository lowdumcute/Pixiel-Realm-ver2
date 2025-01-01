using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemStats : MonoBehaviour
{
    public enum ItemType { Weapon, Helmet, Armor, Shoe, Ring, Pet } // Loại Item
    public enum ItemRarity { Common, Rare, Legendary } // Loại Rarity

    [Header("Stat Item")]
    public ItemType Type;
    [SerializeField] public GameObject EquipButton;
    [SerializeField] public GameObject UnequipButton;
    [SerializeField] private Asset playerAsset; // Tham chiếu đến Asset người chơi
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private AssetDisplay assetDisplay;
    [SerializeField] private ItemInventory itemInventory; // Tham chiếu tới itemInventory
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemData itemData; // Tham chiếu đến ItemData
    [SerializeField] private RaritySprites raritySprites; // Tham chiếu đến RaritySprites
    [SerializeField] private Image itemImage; // Tham chiếu đến Image của item 

    public bool isEquipped = false; // Trạng thái trang bị

    private void Start()
    {
        if (assetDisplay == null)
        {
            assetDisplay = FindObjectOfType<AssetDisplay>();
        }
        itemManager = FindObjectOfType<ItemManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        UpdateItemImageByRarity();
    }

    private void UpdateItemImageByRarity()
    {
        switch (itemInventory.Rarity)
        {
            case ItemStats.ItemRarity.Common:
                itemImage.sprite = raritySprites.commonSprite; // Sprite cho Common
                break;
            case ItemStats.ItemRarity.Rare:
                itemImage.sprite = raritySprites.rareSprite; // Sprite cho Rare
                break;
            case ItemStats.ItemRarity.Legendary:
                itemImage.sprite = raritySprites.legendSprite; // Sprite cho Legendary
                break;
            default:
                Debug.LogError("Không nhận diện được rarity cho item!");
                break;
        }
    }

    public void Equip()
    {
        if (isEquipped)
        {
            Debug.LogWarning("Item is already equipped!");

            // Gọi Notification() khi đã có item trang bị
            itemManager.Notification();

            // Không tiếp tục trang bị item mới, chỉ dừng lại ở đây
            return;
        }

        if (playerAsset != null && itemManager != null)
        {
            // Kiểm tra xem có item nào đã trang bị chưa
            ItemStats itemToUnequip = itemManager.GetEquippedItem(itemInventory.Type);

            // Nếu đã có trang bị cũ, yêu cầu tháo ra trước
            if (itemToUnequip != null)
            {
                Debug.LogWarning("Đã có item trang bị, yêu cầu tháo item cũ trước.");
                itemManager.Notification();
                return;
            }

            EquipButton.SetActive(false);
            UnequipButton.SetActive(true);
            itemInventory.isEquipped = true;

            // Di chuyển item sang panel tương ứng
            itemManager.EquipItem(this);

            // Trừ số lượng item trong túi
            itemInventory.DecreaseQuantity(1);

            // Thêm item vào ItemData
            itemData.SetItem(itemInventory);

            // Cập nhật stat cho nhân vật
            if (itemInventory.Attack > 0)
            {
                playerAsset.AddStatPlayer(Stats.StatsType.AttackPlayer, itemInventory.Attack);
            }

            if (itemInventory.Health > 0)
            {
                playerAsset.AddStatPlayer(Stats.StatsType.HealthPlayer, itemInventory.Health);
            }

            // Cập nhật UI
            assetDisplay.UpdateDisplay();
            inventoryUI.UpdateUI();

            isEquipped = true;
            Debug.Log($"Equipped {gameObject.name}. Added Attack: {itemInventory.Attack}, Health: {itemInventory.Health}.");
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
            if (itemInventory.Attack > 0)
            {
                playerAsset.MinusStatPlayer(Stats.StatsType.AttackPlayer, itemInventory.Attack);
            }

            if (itemInventory.Health > 0)
            {
                playerAsset.MinusStatPlayer(Stats.StatsType.HealthPlayer, itemInventory.Health);
            }

            isEquipped = false;

            // Cập nhật UI
            assetDisplay.UpdateDisplay();
            inventoryUI.UpdateUI();

            Debug.Log($"Unequipped {gameObject.name}. Removed Attack: {itemInventory.Attack}, Health: {itemInventory.Health}.");
        }
        else
        {
            Debug.LogError("Player Asset hoặc Item Manager chưa được gán!");
        }
    }
}
