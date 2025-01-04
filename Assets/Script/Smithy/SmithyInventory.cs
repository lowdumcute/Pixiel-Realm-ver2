using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmithyInventory : MonoBehaviour
{
    [Header("Prefab để hiển thị item")]
    public GameObject itemPrefab;

    [Header("Danh sách Inventory cần lấy dữ liệu")]
    public Inventory inventory;

    [Header("Vị trí hiển thị các item")]
    public Transform itemParent;
    public Transform upgradePanel;

    [Header("Danh sách Rarity")]
    [SerializeField] private RaritySprites raritySprites; // Tham chiếu đến ScriptableObject RaritySprites

    private void Start()
    {
        if (inventory != null)
        {
            RefreshInventoryUI();
        }
    }
    public void Arrange()
    {
        SortItemsByRarity(itemParent);
    }

    // Làm mới UI của SmithyInventory
    public void RefreshInventoryUI()
{
    // Xóa các object con hiện tại
    foreach (Transform child in itemParent)
    {
        Destroy(child.gameObject);
    }
    foreach (Transform child in upgradePanel)
    {
        Destroy(child.gameObject);
    }

    Arrange();

    // Duyệt qua tất cả các item trong Inventory sau khi sắp xếp
    foreach (ItemInventory item in inventory.items)
    {
        // Kiểm tra điều kiện item có quantity = 1 hoặc isEquip == true
        if (item.currentQuantity == 1 || item.isEquipped)
        {
            // Tạo một object mới từ prefab
            GameObject newItem = Instantiate(itemPrefab, itemParent);
            newItem.name = item.itemName; // Đặt tên cho object con

            // Gán Sprite Icon vào object con
            Image rarityImage = newItem.transform.GetComponent<Image>(); // Object cha 
            if (rarityImage != null)
            {
                UpdateRaritySprite(rarityImage, item.Rarity);
            }

            // Gán thông tin 
            ItemHandler itemHandler = newItem.GetComponent<ItemHandler>();
            if (itemHandler != null)
            {
                itemHandler.inventoryPanel = itemParent;
                itemHandler.upgradePanel = upgradePanel;
                itemHandler.itemData = item;
                itemHandler.isUpgrade = false;
            }

            Image icon = newItem.transform.GetChild(0).GetComponent<Image>(); // Object con đầu tiên từ trên xuống
            if (icon != null)
            {
                icon.sprite = item.Icon; // Thay đổi icon từ ItemInventory
            }
        }
    }
}
    private void SortItemsByRarity(Transform panel)
{
    // Lấy tất cả các item con trong panel
    List<ItemHandler> itemHandlers = new List<ItemHandler>();

    foreach (Transform child in panel)
    {
        ItemHandler itemHandler = child.GetComponent<ItemHandler>();
        if (itemHandler != null)
        {
            itemHandlers.Add(itemHandler); // Thêm các item vào danh sách
        }
    }

    // Sắp xếp danh sách item theo rarity: Legendary -> Rare -> Common
    itemHandlers.Sort((item1, item2) => item2.itemData.Rarity.CompareTo(item1.itemData.Rarity));

    // Đặt lại vị trí các item theo thứ tự đã sắp xếp
    for (int i = 0; i < itemHandlers.Count; i++)
    {
        itemHandlers[i].transform.SetSiblingIndex(i); // Đặt lại vị trí của item trong panel
    }
}

    // Cập nhật hình ảnh dựa trên rarity
    private void UpdateRaritySprite(Image rarityImage, ItemStats.ItemRarity rarity)
    {
        if (raritySprites == null)
        {
            Debug.LogError("RaritySprites chưa được gán!");
            return;
        }

        switch (rarity)
        {
            case ItemStats.ItemRarity.Common:
                rarityImage.sprite = raritySprites.commonSprite;
                break;
            case ItemStats.ItemRarity.Rare:
                rarityImage.sprite = raritySprites.rareSprite;
                break;
            case ItemStats.ItemRarity.Legendary:
                rarityImage.sprite = raritySprites.legendSprite;
                break;
            default:
                rarityImage.sprite = null; // Xóa sprite nếu không có rarity phù hợp
                break;
        }
    }
}
