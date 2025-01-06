using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [Header("Prefab Lists")]
    [SerializeField] private List<GameObject> commonItemPrefabs;
    [SerializeField] private List<GameObject> rareItemPrefabs;
    [SerializeField] private List<GameObject> legendaryItemPrefabs;

    [Header("Player Inventory")]
    public InventoryUI inventoryUI;
    public Inventory playerInventory;
    public Asset asset;

    [Header("Gacha Settings")]
    public TextMeshProUGUI StarText;
    public AssetDisplay assetDisplay;
    public GachaUI gachaUI;
    public GachaUIReward gachaUIReward;

    [Header("% Drop Rate")]
    public float Common = 0.90f;
    public float Rare = 0.10f;
    public float Legendary = 0.025f;

    public int pityCount = 0;
    public int consecutiveNonRare = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        StarText.text = asset.Star.ToString();
        assetDisplay.UpdateDisplay();
        inventoryUI.UpdateUI();
    }

    public void GachaOnce()
    {
        if (asset.Star < 160)
        {
            Debug.Log("Không đủ tài nguyên để quay!");
            return;
        }

        asset.Star -= 160;
        UpdateUI();

        float roll = Random.Range(0f, 1f);
        AdjustPity(ref roll);

        pityCount++;
        if (pityCount > 90)
        {
            pityCount = 0;
        }

        if (consecutiveNonRare >= 10)
        {
            roll = Rare + Legendary; // Chắc chắn nhận Rare nếu consecutiveNonRare >= 10
        }

        GameObject reward = GetRewardPrefab(roll);
        if (reward != null)
        {
            SpawnAndAddReward(reward);

            // Cập nhật pity và consecutiveNonRare dựa trên loại vật phẩm
            if (legendaryItemPrefabs.Contains(reward))
            {
                pityCount = 0;
                consecutiveNonRare = 0;
            }
            else if (rareItemPrefabs.Contains(reward))
            {
                consecutiveNonRare = 0;
            }
            else
            {
                consecutiveNonRare++;
            }
        }
    }

    public void GachaTenTimes()
    {
        if (asset.Star < 1600)
        {
            Debug.Log("Không đủ tài nguyên để quay 10 lần!");
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            GachaOnce();
        }
    }

    private void AdjustPity(ref float roll)
    {
        if (pityCount >= 89)
        {
            roll = 0f; // Chắc chắn nhận Legendary
        }
    }

    private GameObject GetRewardPrefab(float roll)
    {
        if (consecutiveNonRare >= 10)
        {
            return rareItemPrefabs[Random.Range(0, rareItemPrefabs.Count)];
        }

        if (pityCount >= 89)
        {
            return legendaryItemPrefabs[Random.Range(0, legendaryItemPrefabs.Count)];
        }

        if (roll <= Legendary)
        {
            return legendaryItemPrefabs[Random.Range(0, legendaryItemPrefabs.Count)];
        }
        else if (roll <= Rare + Legendary)
        {
            return rareItemPrefabs[Random.Range(0, rareItemPrefabs.Count)];
        }
        else
        {
            return commonItemPrefabs[Random.Range(0, commonItemPrefabs.Count)];
        }
    }

    private void SpawnAndAddReward(GameObject rewardPrefab)
    {
        // Tạo một clone của rewardPrefab trong rewardParent
        GameObject reward = Instantiate(rewardPrefab, gachaUIReward.rewardParent);
        gachaUIReward.rewardPanel.SetActive(true);

        // Kiểm tra và xử lý hiệu ứng Legendary ngay sau khi tạo clone
        AddItem addItemScript = reward.GetComponent<AddItem>();
        if (addItemScript != null && addItemScript.itemInventory != null)
        {
            if (addItemScript.itemInventory.Rarity == ItemStats.ItemRarity.Legendary)
            {
                Debug.Log("Trúng vật phẩm Legendary!");

                // Spawn hiệu ứng Legendary và làm nó là con của reward
                if (gachaUIReward.legendaryEffectPrefab != null)
                {
                    GameObject legendaryEffect = Instantiate(gachaUIReward.legendaryEffectPrefab, reward.transform);
                    legendaryEffect.transform.localPosition = Vector3.zero; // Đặt vị trí trung tâm
                    legendaryEffect.transform.SetSiblingIndex(0);
                }
            }

            // Thêm vật phẩm vào Inventory
            addItemScript.AddToInventory(1);
        }
        else
        {
            Debug.LogError("Prefab không có script AddItem hoặc ItemInventory!");
        }

        Debug.Log($"Trúng thưởng: {reward.name}");
    }

}
