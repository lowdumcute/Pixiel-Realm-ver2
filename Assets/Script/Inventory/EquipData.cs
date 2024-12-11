using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/PanelData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Danh sách item từ các Panel")]
    public List<ItemInventory> weaponItems = new List<ItemInventory>();
    public List<ItemInventory> helmetItems = new List<ItemInventory>();
    public List<ItemInventory> armorItems = new List<ItemInventory>();
    public List<ItemInventory> shoeItems = new List<ItemInventory>();
    public List<ItemInventory> ringItems = new List<ItemInventory>();
    public List<ItemInventory> petItems = new List<ItemInventory>();

    // Thêm item vào đúng danh sách dựa trên loại
    public void AddItem(ItemInventory newItem, int amount = 1)
    {
        switch (newItem.Type)
        {
            case ItemStats.ItemType.Weapon:
                AddToList(weaponItems, newItem, amount);
                break;
            case ItemStats.ItemType.Helmet:
                AddToList(helmetItems, newItem, amount);
                break;
            case ItemStats.ItemType.Armor:
                AddToList(armorItems, newItem, amount);
                break;
            case ItemStats.ItemType.Shoe:
                AddToList(shoeItems, newItem, amount);
                break;
            case ItemStats.ItemType.Ring:
                AddToList(ringItems, newItem, amount);
                break;
            case ItemStats.ItemType.Pet:
                AddToList(petItems, newItem, amount);
                break;
            default:
                Debug.LogError("Unknown Item Type");
                break;
        }
    }

    // Xóa item khỏi đúng danh sách dựa trên loại
    public void RemoveItem(ItemInventory itemToRemove, int amount = 1)
    {
        switch (itemToRemove.Type)
        {
            case ItemStats.ItemType.Weapon:
                RemoveFromList(weaponItems, itemToRemove, amount);
                break;
            case ItemStats.ItemType.Helmet:
                RemoveFromList(helmetItems, itemToRemove, amount);
                break;
            case ItemStats.ItemType.Armor:
                RemoveFromList(armorItems, itemToRemove, amount);
                break;
            case ItemStats.ItemType.Shoe:
                RemoveFromList(shoeItems, itemToRemove, amount);
                break;
            case ItemStats.ItemType.Ring:
                RemoveFromList(ringItems, itemToRemove, amount);
                break;
            case ItemStats.ItemType.Pet:
                RemoveFromList(petItems, itemToRemove, amount);
                break;
            default:
                Debug.LogError("Unknown Item Type");
                break;
        }
    }

    // Thêm item vào danh sách
    private void AddToList(List<ItemInventory> itemList, ItemInventory item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            itemList.Add(item);
        }
    }

    // Xóa item khỏi danh sách
    private void RemoveFromList(List<ItemInventory> itemList, ItemInventory item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (itemList.Contains(item))
            {
                itemList.Remove(item);
            }
        }
    }
}
