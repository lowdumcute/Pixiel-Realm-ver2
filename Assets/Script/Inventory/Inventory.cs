using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemInventory> items = new List<ItemInventory>();

    // Thêm item vào inventory
    public void AddItem(ItemInventory newItem, int amount = 1)
    {
        ItemInventory existingItem = items.Find(item => item.itemName == newItem.itemName);

        if (existingItem != null)
        {
            // Nếu item đã tồn tại, tăng số lượng
            existingItem.IncreaseQuantity(amount);
        }
        else
        {
            // Tạo bản sao để lưu trong inventory và thêm vào danh sách
            ItemInventory newItemInstance = Instantiate(newItem);
            newItemInstance.currentQuantity = amount;
            items.Add(newItemInstance);
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
        }
    }

    // Kiểm tra xem inventory có chứa item hay không
    public bool ContainsItem(ItemInventory itemToCheck)
    {
        return items.Exists(item => item.itemName == itemToCheck.itemName);
    }

    // Xóa toàn bộ inventory
    public void ClearInventory()
    {
        items.Clear();
    }
}
