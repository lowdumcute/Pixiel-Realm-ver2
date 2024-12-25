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
            existingItem.IncreaseQuantityTotal(amount);

            // Kiểm tra nếu số lượng > 1 và chuyển thành mảnh
            if (existingItem.currentQuantity > 1)
            {
                ConvertToFragments(existingItem);
            }
        }
        else
        {
            // Tạo bản sao để lưu trong inventory và thêm vào danh sách
            ItemInventory newItemInstance = Instantiate(newItem);
            newItemInstance.currentQuantity = amount;
            items.Add(newItemInstance);

            // Kiểm tra nếu số lượng > 1 và chuyển thành mảnh
            if (newItemInstance.currentQuantity > 1)
            {
                ConvertToFragments(newItemInstance);
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

            // Xóa item nếu số lượng bằng 0
            if (existingItem.currentQuantity <= 0)
            {
                items.Remove(existingItem);
            }

            // Nếu item bị giảm số lượng còn lại > 1, kiểm tra lại mảnh
            if (existingItem.currentQuantity > 1)
            {
                ConvertToFragments(existingItem);
            }
        }
    }

    // Kiểm tra xem inventory có chứa item hay không
    public bool ContainsItem(ItemInventory itemToCheck)
    {
        return items.Exists(item => item.itemName == itemToCheck.itemName);
    }

    // Chuyển đổi item thành mảnh dựa trên rarity
    private void ConvertToFragments(ItemInventory item)
    {
        int fragmentCount = 0;

        switch (item.Rarity)
        {
            case ItemStats.ItemRarity.Common:
                fragmentCount = 1; // Chuyển thành 1 mảnh
                break;
            case ItemStats.ItemRarity.Rare:
                fragmentCount = 2; // Chuyển thành 2 mảnh
                break;
            case ItemStats.ItemRarity.Legendary:
                fragmentCount = 3; // Chuyển thành 3 mảnh
                break;
            default:
                fragmentCount = 1; // Mặc định chuyển thành 1 mảnh
                break;
        }

        // Nếu item có số lượng > 1, tạo các mảnh
        if (item.currentQuantity > 1)
        {
            // Lưu số mảnh vào Asset
            AddFragmentsToAsset(item.Type, fragmentCount);

            // Đặt lại số lượng item sau khi chuyển thành mảnh
            item.currentQuantity = 0; // Số lượng item gốc sẽ trở thành 0
        }
    }

    // Tạo mảnh từ item gốc
    private void AddFragmentsToAsset(ItemStats.ItemType type, int fragmentCount)
    {
        switch (type)
        {
            case ItemStats.ItemType.Weapon:
                playerAsset.fragmentWeapon += fragmentCount;
                break;
            case ItemStats.ItemType.Helmet:
                playerAsset.fragmentHelmet += fragmentCount;
                break;
            case ItemStats.ItemType.Armor:
                playerAsset.fragmentArmor += fragmentCount;
                break;
            case ItemStats.ItemType.Shoe:
                playerAsset.fragmentShoe += fragmentCount;
                break;
            case ItemStats.ItemType.Ring:
                playerAsset.fragmentRing += fragmentCount;
                break;
            case ItemStats.ItemType.Pet:
                playerAsset.fragmentPet += fragmentCount;
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
            item.totalQuantity = 0;    // Đặt lại tổng số lượng về 0
        }
    }
}
