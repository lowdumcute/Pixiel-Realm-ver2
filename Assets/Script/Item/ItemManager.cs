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
        UpdatePanel(weaponPanel, itemData.weaponItems, ref weapon);
        UpdatePanel(helmetPanel, itemData.helmetItems, ref helmet);
        UpdatePanel(armorPanel, itemData.armorItems, ref armor);
        UpdatePanel(shoePanel, itemData.shoeItems, ref shoe);
        UpdatePanel(ringPanel, itemData.ringItems, ref ring);
        UpdatePanel(petPanel, itemData.petItems, ref pet);
    }
    private void UpdatePanel(GameObject panel, List<ItemInventory> itemList, ref ItemStats itemStats)
    {
        foreach (var item in itemList)
        {
            if (item != null)
            {
                // Tạo một item từ danh sách và di chuyển vào đúng panel
                GameObject itemPrefab = Instantiate(item.itemPrefab, panel.transform);
                itemPrefab.transform.localPosition = Vector3.zero;

                // Tìm ItemStats trong prefab và set nó
                ItemStats stats = itemPrefab.GetComponent<ItemStats>();  // Lấy ItemStats từ prefab

                if (stats != null)
                {
                    // Cập nhật lại button và trạng thái
                    stats.EquipButton.SetActive(false); // Ẩn EquipButton
                    stats.UnequipButton.SetActive(true); // Hiện UnequipButton
                    stats.isEquipped = true;

                    // Gán ItemStats vào biến itemStats để quản lý
                    itemStats = stats;
                }
            }
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
            switch (item.Type)
            {
                case ItemStats.ItemType.Weapon:
                    if (weapon != null)
                    {
                        Debug.LogWarning("Weapon slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    weapon = item;
                    break;

                case ItemStats.ItemType.Helmet:
                    if (helmet != null)
                    {
                        Debug.LogWarning("Helmet slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    helmet = item;
                    break;

                case ItemStats.ItemType.Armor:
                    if (armor != null)
                    {
                        Debug.LogWarning("Armor slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    armor = item;
                    break;

                case ItemStats.ItemType.Shoe:
                    if (shoe != null)
                    {
                        Debug.LogWarning("Shoe slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    shoe = item;
                    break;

                case ItemStats.ItemType.Ring:
                    if (ring != null)
                    {
                        Debug.LogWarning("Ring slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    ring = item;
                    break;

                case ItemStats.ItemType.Pet:
                    if (pet != null)
                    {
                        Debug.LogWarning("Pet slot đã được trang bị. Tháo trước khi trang bị mới.");
                        return;
                    }
                    pet = item;
                    break;
            }

            Debug.Log($"{item.name} đã được di chuyển đến {targetPanel.name}");
        }
        else
        {
            Debug.LogError($"Không tìm thấy panel cho loại item: {item.Type}");
        }
    }

    public void UnequipItem(ItemStats.ItemType itemType)
    {
        ItemStats itemToUnequip = GetEquippedItem(itemType);
        inventoryUI.UpdateUI();

        if (itemToUnequip != null)
        {
            // Di chuyển object về inventory
            inventoryUI.UpdateUI();

            // Xóa object trong panel tương ứng
            GameObject targetPanel = GetTargetPanel(itemType);
            if (targetPanel != null)
            {
                foreach (Transform child in targetPanel.transform)
                {
                    if (child.gameObject == itemToUnequip.gameObject)
                    {
                        Destroy(child.gameObject);
                        Debug.Log($"{itemToUnequip.name} đã được xóa khỏi panel {targetPanel.name}.");
                        break;
                    }
                }
            }
        }
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
