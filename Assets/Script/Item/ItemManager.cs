using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Quản lý Item")]
    [SerializeField] private ItemStats weapon;
    [SerializeField] private ItemStats helmet;
    [SerializeField] private ItemStats armor;
    [SerializeField] private ItemStats shoe;
    [SerializeField] private ItemStats ring;
    [SerializeField] private ItemStats pet;

    [Header("Panel UI")]
    [SerializeField] public GameObject weaponPanel;
    [SerializeField] public GameObject helmetPanel;
    [SerializeField] public GameObject armorPanel;
    [SerializeField] public GameObject shoePanel;
    [SerializeField] public GameObject ringPanel;
    [SerializeField] public GameObject petPanel;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemData itemData;
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Cập nhật UI cho từng loại item
        UpdatePanel(weaponPanel, itemData.weaponItem, ref weapon);
        UpdatePanel(helmetPanel, itemData.helmetItem, ref helmet);
        UpdatePanel(armorPanel, itemData.armorItem, ref armor);
        UpdatePanel(shoePanel, itemData.shoeItem, ref shoe);
        UpdatePanel(ringPanel, itemData.ringItem, ref ring);
        UpdatePanel(petPanel, itemData.petItem, ref pet);
    }

    // Phương thức cập nhật từng Panel dựa trên item hiện tại
    private void UpdatePanel(GameObject panel, ItemInventory itemInventory, ref ItemStats currentItemSlot)
    {
        // Kiểm tra nếu itemInventory không null
        if (itemInventory != null)
        {
            // Kích hoạt panel nếu có item
            panel.SetActive(true);

            // Nếu UI slot chưa có, tạo một object mới từ prefab của item
            if (currentItemSlot == null)
            {
                GameObject itemObject = Instantiate(itemInventory.itemPrefab, panel.transform);
                currentItemSlot = itemObject.GetComponent<ItemStats>();

                // Đặt trạng thái cho item
                if (currentItemSlot != null)
                {
                    currentItemSlot.EquipButton.SetActive(false); // Ẩn nút trang bị
                    currentItemSlot.UnequipButton.SetActive(true); // Hiện nút gỡ bỏ
                    currentItemSlot.isEquipped = true;
                }
            }
        }
        else
        {
            // Tắt panel và hủy đối tượng nếu không có item
            if (currentItemSlot != null)
            {
                Destroy(currentItemSlot.gameObject);
                currentItemSlot = null;
            }
            panel.SetActive(false);
        }
    }

    public void EquipItem(ItemStats item)
    {
        GameObject targetPanel = GetTargetPanel(item.Type);
        if (targetPanel != null)
        {
            // Di chuyển object sang panel tương ứng
            item.transform.SetParent(targetPanel.transform);
            item.transform.localPosition = Vector3.zero;

            // Lưu item vào `ItemData`
            itemData.SetItem(new ItemInventory { Type = item.Type, itemPrefab = item.gameObject });

            // Gán tham chiếu vào biến quản lý
            switch (item.Type)
            {
                case ItemStats.ItemType.Weapon: weapon = item; break;
                case ItemStats.ItemType.Helmet: helmet = item; break;
                case ItemStats.ItemType.Armor: armor = item; break;
                case ItemStats.ItemType.Shoe: shoe = item; break;
                case ItemStats.ItemType.Ring: ring = item; break;
                case ItemStats.ItemType.Pet: pet = item; break;
            }

            Debug.Log($"{item.name} đã được trang bị.");
        }
    }

    public void UnequipItem(ItemStats.ItemType itemType)
    {
        ItemStats itemToUnequip = GetEquippedItem(itemType);

        if (itemToUnequip != null)
        {
            // Xóa item khỏi panel
            GameObject targetPanel = GetTargetPanel(itemType);
            if (targetPanel != null)
            {
                Destroy(itemToUnequip.gameObject);
            }

            // Xóa dữ liệu trong `ItemData`
            itemData.RemoveItem(itemType);

            // Xóa tham chiếu trong ItemManager
            switch (itemType)
            {
                case ItemStats.ItemType.Weapon: weapon = null; break;
                case ItemStats.ItemType.Helmet: helmet = null; break;
                case ItemStats.ItemType.Armor: armor = null; break;
                case ItemStats.ItemType.Shoe: shoe = null; break;
                case ItemStats.ItemType.Ring: ring = null; break;
                case ItemStats.ItemType.Pet: pet = null; break;
            }

            Debug.Log($"Item {itemType} đã được tháo.");
        }
        inventoryUI.UpdateUI();
    }

    private GameObject GetTargetPanel(ItemStats.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemStats.ItemType.Weapon: return weaponPanel;
            case ItemStats.ItemType.Helmet: return helmetPanel;
            case ItemStats.ItemType.Armor: return armorPanel;
            case ItemStats.ItemType.Shoe: return shoePanel;
            case ItemStats.ItemType.Ring: return ringPanel;
            case ItemStats.ItemType.Pet: return petPanel;
            default: return null;
        }
    }

    private ItemStats GetEquippedItem(ItemStats.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemStats.ItemType.Weapon: return weapon;
            case ItemStats.ItemType.Helmet: return helmet;
            case ItemStats.ItemType.Armor: return armor;
            case ItemStats.ItemType.Shoe: return shoe;
            case ItemStats.ItemType.Ring: return ring;
            case ItemStats.ItemType.Pet: return pet;
            default: return null;
        }
    }
}
