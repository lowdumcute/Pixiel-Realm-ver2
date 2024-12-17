using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/PanelData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Item từ các Panel")]
    public ItemInventory weaponItem;
    public ItemInventory helmetItem;
    public ItemInventory armorItem;
    public ItemInventory shoeItem;
    public ItemInventory ringItem;
    public ItemInventory petItem;

    // Thêm item vào đúng ô dựa trên loại
    public void SetItem(ItemInventory newItem)
    {
        switch (newItem.Type)
        {
            case ItemStats.ItemType.Weapon:
                weaponItem = newItem;
                break;
            case ItemStats.ItemType.Helmet:
                helmetItem = newItem;
                break;
            case ItemStats.ItemType.Armor:
                armorItem = newItem;
                break;
            case ItemStats.ItemType.Shoe:
                shoeItem = newItem;
                break;
            case ItemStats.ItemType.Ring:
                ringItem = newItem;
                break;
            case ItemStats.ItemType.Pet:
                petItem = newItem;
                break;
            default:
                Debug.LogError("Unknown Item Type");
                break;
        }
    }

    // Xóa item khỏi đúng ô dựa trên loại
    public void RemoveItem(ItemStats.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemStats.ItemType.Weapon:
                weaponItem = null;
                break;
            case ItemStats.ItemType.Helmet:
                helmetItem = null;
                break;
            case ItemStats.ItemType.Armor:
                armorItem = null;
                break;
            case ItemStats.ItemType.Shoe:
                shoeItem = null;
                break;
            case ItemStats.ItemType.Ring:
                ringItem = null;
                break;
            case ItemStats.ItemType.Pet:
                petItem = null;
                break;
            default:
                Debug.LogError("Unknown Item Type");
                break;
        }
    }
}
