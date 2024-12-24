using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ItemStats;

public class GachaSystem : MonoBehaviour
{
    [Header("List Gacha Item")]
    public List<ItemInventory> availableItems;  // Danh sách các item có thể trúng trong gacha


    // Cài đặt giá trị pity (số lần rút trước khi xác suất gacha tăng)
    public int pityCount = 0;


    // UI của inventory
    [Header("Inventory Of Player")]
    public InventoryUI inventoryUI;
    public Inventory playerInventory;  // Tham chiếu đến Inventory của người chơi
    [Header("Tài nguyên của người chơi")]
    public Asset asset;// Tham chiếu đến Tài nguyên của người chơi của người chơi
    public TextMeshProUGUI StarText;
    public AssetDisplay assetDisplay;
    //public GameObject Panel;

    // Tỉ lệ rơi vật phẩm của Item
    [Header("% ăn rate")]
    public float Common = 0.90f;    // 90% cho item hiếm 
    public float Rare = 0.10f;      // 10% cho item hiếm 
    public float Legendary = 0.025f; // 2,5% cho item huyền thoại

    // Danh sách cho từng loại item
    private List<ItemInventory> commonItems = new List<ItemInventory>();
    private List<ItemInventory> rareItems = new List<ItemInventory>();
    private List<ItemInventory> legendaryItems = new List<ItemInventory>();

    // Thống kê số lần quay không trúng Rare
    public int consecutiveNonRare = 0;

    public void Start()
    {
        // Phân loại các item vào 3 danh sách theo rarity
        foreach (var item in availableItems)
        {
            switch (item.Rarity)
            {
                case ItemRarity.Common:
                    commonItems.Add(item);
                    break;
                case ItemRarity.Rare:
                    rareItems.Add(item);
                    break;
                case ItemRarity.Legendary:
                    legendaryItems.Add(item);
                    break;
            }
        }
        TextUpdate();
        assetDisplay.UpdateDisplay();

    }
    private void TextUpdate()
    {
        StarText.text = asset.Star.ToString();
    }
    public void GachaOnce()
    {
        // Xác suất ngẫu nhiên với hệ thống pity
        if (asset.Star < 159)
        {
            Debug.Log("Khong du tai nguyen de roll");
            //Panel.SetActive(true);
            return;
        }
        asset.Star -= 160;
        TextUpdate();
        assetDisplay.UpdateDisplay();
        float roll = Random.Range(0f, 1f);

        // Kiểm tra pity và điều chỉnh xác suất nếu cần
        AdjustPity(ref roll);

        // Kiểm tra trường hợp 9 lần liên tiếp không trúng Rare
        if (consecutiveNonRare >= 9)
        {
            Debug.Log("Lần này chắc chắn là Rare!");
            roll = 0.9f; // Reset để luôn trúng Rare
        }

        // Lấy vật phẩm từ danh sách dựa trên xác suất
        ItemInventory selectedItem = GetItemBasedOnRarity(roll);

        // Đảm bảo rằng chắc chắn nhận được vật phẩm
        if (selectedItem != null)
        {

            playerInventory.AddItem(selectedItem, 1);

            // Nếu trúng Legendary, reset pity và tỷ lệ Legendary về giá trị ban đầu
            if (selectedItem.Rarity == ItemRarity.Legendary)
            {
                Debug.Log("<color=yellow>Trúng item: " + selectedItem.itemName + "</color>");
                Debug.Log("<color=yellow>Rarity: " + selectedItem.Rarity + "</color>");
                pityCount = 0; // Reset lại số lần quay không trúng Legendary
                Legendary = 0.025f; // Đưa tỷ lệ Legendary về giá trị ban đầu
            }
            else
            {
                pityCount++;
                // Nếu trúng Rare, reset đếm liên tiếp không trúng Rare
                if (selectedItem.Rarity == ItemRarity.Rare)
                {
                    Debug.Log("<color=purple>Trúng item: " + selectedItem.itemName + "</color>");
                    Debug.Log("<color=purple>Rarity: " + selectedItem.Rarity + "</color>");

                    consecutiveNonRare = 0;

                }
                else
                {
                    Debug.Log("Trúng item: " + selectedItem.itemName);
                    Debug.Log("Rarity: " + selectedItem.Rarity);
                    consecutiveNonRare++;
                }
            }
        }
        else
        {
            Debug.Log("Không trúng item nào.");
        }
        inventoryUI.UpdateUI();
    }

    private void AdjustPity(ref float roll)
    {
        // Kiểm tra nếu đã quay đủ 89 lần không trúng Legendary
        if (pityCount >= 89)
        {
            // Đảm bảo rằng lần này chắc chắn trúng Legendary
            roll = 0; // Đặt roll về 0 để chắc chắn trúng Legendary
            Debug.Log("Đã đạt mốc pity 89 lần, lần này chắc chắn Legendary!");
        }

    }

    public void GachaTenTimes()
    {
        if (asset.Star < 1599)
        {
            Debug.Log("Khong du tai nguyen de roll");
            //Panel.SetActive(true);
            return;
        }

        Debug.Log("Thực hiện gacha 10 lần...");
        for (int i = 0; i < 10; i++)
        {
            GachaOnce();
        }
    }
    private ItemInventory GetItemBasedOnRarity(float roll)
    {
        // Nếu đã quay đủ 9 lần không ra Rare, chắc chắn chọn Rare
        if (consecutiveNonRare >= 9)
        {
            // Đảm bảo trúng Rare
            return rareItems[Random.Range(0, rareItems.Count)];
        }
        // Nếu đã đạt đủ pity (89 lần không ra Legendary), chắc chắn chọn Legendary
        if (pityCount >= 89)
        {
            return legendaryItems[Random.Range(0, legendaryItems.Count)];
        }

        if (pityCount > 50)
        {

            Legendary += 0.01f;  // Tăng tỷ lệ Legendary thêm 2%
            Debug.Log("Tỉ lệ Legendary tăng lên: " + Legendary);
        }
        if (roll <= Legendary)
        {
            return legendaryItems[Random.Range(0, legendaryItems.Count)];
        }
        else if (roll <= Rare + Legendary)
        {
            return rareItems[Random.Range(0, rareItems.Count)];
        }
        else
        {
            return commonItems[Random.Range(0, commonItems.Count)];
        }
    }
}
