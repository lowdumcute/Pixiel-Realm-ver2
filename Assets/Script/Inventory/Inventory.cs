using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemInventory> items = new List<ItemInventory>();

    // Tham chiếu đến Asset để lưu mảnh
    public Asset playerAsset;

    // Thêm item vào inventory
    public void AddItem(ItemInventory newItem, int amount = 1)
    {
        ItemInventory existingItem = items.Find(item => item.itemName == newItem.itemName);

        if (existingItem != null)
        {
            // Nếu item đã tồn tại, tăng số lượng
            existingItem.IncreaseQuantity(amount);
            

            // Kiểm tra nếu số lượng > 1 và chuyển thành mảnh
            if (existingItem.currentQuantity > 1 && newItem.isEquipped == false)
            {
                ConvertToFragmentsUnEquip(existingItem);
            }
            else if (existingItem.isEquipped == true)
            {
                ConvertToFragmentsisEquipped(existingItem);
            }
        }
        else
        {
            // Tạo bản sao để lưu trong inventory và thêm vào danh sách
            ItemInventory newItemInstance = Instantiate(newItem);
            newItemInstance.currentQuantity = amount;
            items.Add(newItemInstance);

            // Kiểm tra nếu số lượng > 1 và chuyển thành mảnh
            if (existingItem.currentQuantity > 1 && newItem.isEquipped == false)
            {
                ConvertToFragmentsUnEquip(newItemInstance);
            }
            else if (existingItem.isEquipped == true)
            {
                ConvertToFragmentsisEquipped(existingItem);
            }
        }
    }

    // Xóa item khỏi inventory
    public void RemoveItem(ItemInventory itemToRemove, int amount = 1)
    {
        ItemInventory existingItem = items.Find(item => item.itemName == itemToRemove.itemName);

        if (existingItem != null)
        {
            existingItem.DecreaseQuantity(amount);

        }
    }

    // Kiểm tra xem inventory có chứa item hay không
    public bool ContainsItem(ItemInventory itemToCheck)
    {
        return items.Exists(item => item.itemName == itemToCheck.itemName);
    }

    // Chuyển đổi item thành mảnh dựa trên rarity
    private void ConvertToFragmentsisEquipped(ItemInventory item)
    {
            int fragmentCount = CalculateFragmentCount(item.Rarity, item.currentQuantity);
            AddFragmentsToAsset(item.Type, fragmentCount);
            item.CheckEquip();
    }     
        
    private void ConvertToFragmentsUnEquip(ItemInventory item)
    {
        int fragmentCount = CalculateFragmentCount(item.Rarity, item.currentQuantity-1);
        AddFragmentsToAsset(item.Type, fragmentCount);
        item.CheckEquip();
    }

    private int CalculateFragmentCount(ItemStats.ItemRarity rarity, int quantity)
    {
        int baseFragment = rarity switch
        {
            ItemStats.ItemRarity.Common => 1,
            ItemStats.ItemRarity.Rare => 2,
            ItemStats.ItemRarity.Legendary => 3,
            _ => 1
        };
        return quantity * baseFragment;
    }

    // Tạo mảnh từ item gốc
    private void AddFragmentsToAsset(ItemStats.ItemType type, int fragmentCount)
    {
        switch (type)
        {
            case ItemStats.ItemType.Weapon:
                playerAsset.fragment += fragmentCount;
                break;
            case ItemStats.ItemType.Helmet:
                playerAsset.fragment += fragmentCount;
                break;
            case ItemStats.ItemType.Armor:
                playerAsset.fragment += fragmentCount;
                break;
            case ItemStats.ItemType.Shoe:
                playerAsset.fragment += fragmentCount;
                break;
            case ItemStats.ItemType.Ring:
                playerAsset.fragment += fragmentCount;
                break;
            case ItemStats.ItemType.Pet:
                playerAsset.fragment += fragmentCount;
                break;
            default:
                Debug.LogWarning("Unknown item type: " + type);
                break;
        }
    }

    // Xóa toàn bộ inventory (cập nhật lại currentQuantity và totalQuantity của tất cả item)
    public void ClearInventory()
    {
        foreach (ItemInventory item in items)
        {
            item.currentQuantity = 0;  // Đặt lại số lượng hiện tại về 0
            
        }
    }
}
