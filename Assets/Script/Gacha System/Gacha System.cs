using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemStats;

public class GachaSystem : MonoBehaviour
{
    public List<ItemInventory> availableItems;  // Danh sách các item có thể trúng trong gacha
    public Inventory playerInventory;  // Tham chiếu đến Inventory của người chơi

    // Cài đặt giá trị pity (số lần rút trước khi xác suất gacha tăng)
    public int pityCount = 0;
    public int pityThreshold = 89; // Mốc pity khi rút không trúng legendary
    public float pityBonusRate = 1.05f; // Tăng xác suất trúng legendary sau pity
    //UI của inventory
    public InventoryUI inventoryUI;
    public void GachaOnce()
    {
        // Xác suất ngẫu nhiên với hệ thống pity
        float roll = Random.Range(0f, 1f);

        // Kiểm tra xem có đang ở trong chế độ pity không
        if (pityCount >= pityThreshold)
        {
            // Tăng xác suất cho Legendary sau khi đạt mốc pity
            roll = Mathf.Min(roll * pityBonusRate, 1.0f);
            Debug.Log("Đã đạt pity, tăng xác suất trúng Legendary!");
        }
        // Lấy vật phẩm từ danh sách theo xác suất và mốc pity
        ItemInventory selectedItem = GetItemBasedOnRarity(roll);
        // Đảm bảo rằng chắc chắn nhận được vật phẩm
        if (selectedItem != null)
        {
            Debug.Log("Trúng item: " + selectedItem.itemName);
            // Thêm item vào Inventory
            playerInventory.AddItem(selectedItem, 1);
            inventoryUI.UpdateUI();
            // Nếu trúng Legendary, reset pity
            if (selectedItem.Rarity == ItemRarity.Legendary)
            {
                pityCount = 0;
            }
            else
            {
                pityCount++;
            }
        }
        else
        {
            Debug.Log("Không trúng item nào.");
        }
    }
    public void GachaTenTimes()
    {
        Debug.Log("Thực hiện gacha 10 lần...");
        for (int i = 0; i < 10; i++)
        {
            GachaOnce();
        }
    }

    private ItemInventory GetItemBasedOnRarity(float roll)
    {
        List<ItemInventory> availableItemsByRarity = new List<ItemInventory>();

        // Lọc vật phẩm theo xác suất
        foreach (var item in availableItems)
        {
            if (roll <= GetRarityProbability(item.Rarity))
            {
                availableItemsByRarity.Add(item);
            }
        }

        // Nếu không có vật phẩm phù hợp, chọn bất kỳ vật phẩm nào từ danh sách
        if (availableItemsByRarity.Count > 0)
        {
            return availableItemsByRarity[Random.Range(0, availableItemsByRarity.Count)];
        }
        return null;
    }
    private float GetRarityProbability(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return 0.80f; // 90% cho item thông thường        
            case ItemRarity.Rare: return 0.20f; // 10% cho item hiếm 
            case ItemRarity.Legendary: return 0.025f; // 1% cho item huyền thoại
            default: return 0.0f;
        }
    }
}
