using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    [SerializeField] private ItemInventory itemInventory; // item add vào túi 
    [SerializeField] private Inventory inventory; // Tham chiếu tới Inventory ScriptableObject

    public void AddToInventory(int amount = 1)
    {
        if (itemInventory == null || inventory == null)
        {
            Debug.LogError("ItemInventory hoặc Inventory chưa được gán!");
            return;
        }

        // Gọi phương thức AddItem của Inventory để thêm item
        inventory.AddItem(itemInventory, amount);

        // Log thông tin để kiểm tra (có thể loại bỏ khi hoàn thành)
        Debug.Log($"Đã thêm {amount} x {itemInventory.itemName} vào túi!");
    }
}
