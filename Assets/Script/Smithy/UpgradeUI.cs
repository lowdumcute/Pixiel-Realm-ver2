using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Item Stats")]
    [SerializeField] private TMP_Text Attack;
    [SerializeField] private TMP_Text NextAttack;
    [SerializeField] private TMP_Text Health;
    [SerializeField] private TMP_Text NextHealth;
    [SerializeField] private Transform StarParent;
    [SerializeField] public ItemInventory itemData;

    [Header("Upgrade Success Rate")]
    [SerializeField] private TMP_Text SuccessRateText; // Hiển thị tỷ lệ thành công
    [SerializeField] private TMP_Text fragmentCostText; // Hiển thị chi phí mảnh
    [SerializeField] public TMP_Text notification; // Hiển thị thông báo thành công/thất bại

    [Header("Star Sprites")]
    [SerializeField] private Sprite startFillSprite;
    [SerializeField] private Sprite startEmptySprite;

    // Tỷ lệ thành công dựa trên cấp độ hiện tại
    private float[] successRates = { 100f, 75f, 50f, 35f, 10f };

    public void Start()
    {
        Empty();
    }

    public void UpdateUI()
    {
        UpdateStats();
        UpdateSuccessRate();
        UpdateFragmentCost();
        SpawnStars();
    }

    // Cập nhật các chỉ số như Attack, NextAttack, Health, NextHealth
    public void UpdateStats()
    {
        Attack.text = $"{itemData.Attack}";
        NextAttack.text = $"{itemData.Attack * 1.5f}";
        Health.text = $"{itemData.Health}";
        NextHealth.text = $"{itemData.Health * 1.5f}";
    }

    // Hiển thị tỷ lệ thành công cho lần nâng cấp tiếp theo
    public void UpdateSuccessRate()
    {
        int currentStar = itemData.CurrentStar;

        if (currentStar >= successRates.Length)
        {
            SuccessRateText.text = "Đã đạt cấp tối đa!";
        }
        else
        {
            float successRate = successRates[currentStar];
            SuccessRateText.text = $"{successRate}%";
        }
    }

    // Hiển thị chi phí mảnh nâng cấp
    public void UpdateFragmentCost()
    {
        int currentStar = itemData.CurrentStar;

        // Kiểm tra nếu đạt cấp tối đa
        if (currentStar >= 5)
        {
            fragmentCostText.text = "<color=red>Đã đạt cấp tối đa!</color>";
            return;
        }

        // Tính toán chi phí và hiển thị
        int fragmentNeed = UpgradeManager.Instance.GetFragmentCost(itemData.Rarity, currentStar); // Lấy từ UpgradeManager
        int currentFragment = UpgradeManager.Instance.fragmentsAsset.fragment;

        string currentFragmentColor = currentFragment >= fragmentNeed ? "green" : "red";
        fragmentCostText.text = $"<color={currentFragmentColor}>{currentFragment}</color> / {fragmentNeed}";
    }

    public void DisplayNotification(bool isSuccess)
    {

        // Ẩn thông báo sau một khoảng thời gian (nếu cần)
        StartCoroutine(HideNotificationAfterDelay(2f));
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notification.text = "";
    }

    // Spawn các sao dựa trên CurrentStar của item
    public void SpawnStars()
    {
        foreach (Transform child in StarParent)
        {
            Destroy(child.gameObject);
        }

        int currentStars = itemData.CurrentStar;
        int maxStars = 5;

        for (int i = 0; i < maxStars; i++)
        {
            GameObject star = new GameObject(i < currentStars ? "FilledStar" : "EmptyStar");
            star.transform.SetParent(StarParent, false);

            Image starImage = star.AddComponent<Image>();
            starImage.sprite = i < currentStars ? startFillSprite : startEmptySprite;
        }
    }

    public void Empty()
    {
        Attack.text = "0";
        NextAttack.text = "0";
        Health.text = "0";
        NextHealth.text = "0";
        SuccessRateText.text = "0%";
        fragmentCostText.text = "";
        notification.text = "";

        foreach (Transform child in StarParent)
        {
            Destroy(child.gameObject);
        }
    }
}
