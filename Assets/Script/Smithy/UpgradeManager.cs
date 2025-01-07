using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // Singleton Instance
    public static UpgradeManager Instance { get; private set; }

    [Header("Tham Chiếu")]
    public Asset fragmentsAsset; // Chứa số lượng fragment
    public AssetDisplay assetDisplay; // UI fragment
    public UpgradeUI upgradeUI; // UI của item

    [Header("Tham chiếu đến Upgrade Panel")]
    public Transform upgradePanel;
    [Header("Prefab")]
    [SerializeField] private GameObject VFXfailed;
    [SerializeField] private GameObject VFXSuccess;

    [Header("Công thức tiêu hao mảnh")]
    private Dictionary<ItemStats.ItemRarity, int[]> fragmentCosts = new Dictionary<ItemStats.ItemRarity, int[]>
    {
        { ItemStats.ItemRarity.Legendary, new int[] { 10, 20, 30, 40, 60 } },
        { ItemStats.ItemRarity.Rare, new int[] { 3, 6, 9, 12, 15 } },
        { ItemStats.ItemRarity.Common, new int[] { 1, 2, 3, 4, 5 } }
    };

    [Header("Tỷ lệ thành công nâng cấp")]
    private float[] upgradeSuccessRates = { 1f, 0.75f, 0.5f, 0.35f, 0.1f };

    private void Awake()
    {
        // Đảm bảo chỉ có một Instance duy nhất
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetFragmentCost(ItemStats.ItemRarity rarity, int currentStar)
    {
        if (fragmentCosts.ContainsKey(rarity) && currentStar < fragmentCosts[rarity].Length)
        {
            return fragmentCosts[rarity][currentStar];
        }

        Debug.LogError($"Không tìm thấy dữ liệu chi phí nâng cấp cho rarity {rarity} với currentStar {currentStar}.");
        return 0;
    }

    public void UpgradeItem()
    {
        if (upgradePanel.childCount == 0)
        {
            Debug.Log("Không có item nào trong Upgrade Panel.");
            return;
        }

        // Lấy item hiện tại từ Upgrade Panel
        ItemHandler itemHandler = upgradePanel.GetChild(0).GetComponent<ItemHandler>();
        if (itemHandler == null || itemHandler.itemData == null)
        {
            Debug.LogError("Không thể tìm thấy ItemHandler hoặc dữ liệu item.");
            return;
        }

        ItemInventory item = itemHandler.itemData;

        // Kiểm tra rarity và currentStar
        if (!fragmentCosts.ContainsKey(item.Rarity))
        {
            Debug.LogError($"Không hỗ trợ nâng cấp cho rarity: {item.Rarity}");
            return;
        }

        if (item.CurrentStar >= 5)
        {
            Debug.Log("Item đã đạt cấp độ tối đa.");
            return;
        }

        // Lấy chi phí nâng cấp và tỷ lệ thành công
        int currentStar = item.CurrentStar;
        int fragmentCost = fragmentCosts[item.Rarity][currentStar];
        float successRate = upgradeSuccessRates[currentStar];

        // Kiểm tra xem có đủ fragment không
        if (fragmentsAsset.fragment < fragmentCost)
        {
            Debug.Log("Không đủ mảnh để nâng cấp.");
            return;
        }

        // Tiến hành trừ nguyên liệu
        fragmentsAsset.DecreaseFragment(fragmentCost); // Giảm số lượng fragment

        // Xác định thành công hay thất bại
        if (Random.value <= successRate) // Thành công
        {
            item.Attack = Mathf.RoundToInt(item.Attack * 1.5f); // Tăng Attack
            item.Health = Mathf.RoundToInt(item.Health * 1.5f); // Tăng Health
            item.CurrentStar += 1; // Tăng sao

            upgradeUI.notification.text = "<color=green>Nâng cấp thành công!</color>";
            upgradeUI.DisplayNotification(true);

            Debug.Log($"Nâng cấp thành công! Item {item.itemName} đã được nâng lên cấp {item.CurrentStar}.");

            // Spawn VFX thành công
            SpawnVFX(VFXSuccess);
        }
        else // Thất bại
        {
            Debug.Log($"Nâng cấp thất bại. Item {item.itemName} vẫn giữ nguyên cấp {item.CurrentStar}.");
            upgradeUI.notification.text = "<color=red>Nâng cấp thất bại!</color>";
            upgradeUI.DisplayNotification(true);

            // Spawn VFX thất bại
            SpawnVFX(VFXfailed);
        }

        // Cập nhật UI nếu cần
        upgradeUI.UpdateUI();
        assetDisplay.UpdateDisplay();
    }
    private void SpawnVFX(GameObject vfxPrefab)
    {
        if (vfxPrefab == null || upgradePanel == null || upgradePanel.childCount == 0) return;

        // Lấy object con đầu tiên của upgradePanel
        Transform firstChild = upgradePanel.GetChild(0);

        // Tạo VFX tại vị trí của object con đầu tiên
        GameObject vfx = Instantiate(vfxPrefab, firstChild.position, Quaternion.identity);

        // Đặt VFX làm con của object con đầu tiên
        vfx.transform.SetParent(firstChild);

        // Hủy VFX sau 2 giây
        Destroy(vfx, 2f);
    }
}
