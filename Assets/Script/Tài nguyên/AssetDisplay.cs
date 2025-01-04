using TMPro;
using UnityEngine;

public class AssetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text starText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text timeEnergyText; // Hiển thị thời gian hồi năng lượng
    [SerializeField] private TMP_Text attacktext;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text fragment;
    private void Start()
    {
        // Lắng nghe sự kiện hồi năng lượng
        if (AssetManager.instance != null)
        {
            Asset assetData = AssetManager.instance.GetAssetData();
            assetData.RestoreEnergyOnLoad(); // Khôi phục năng lượng mỗi khi vào scene

            Asset.OnEnergyReplenished += UpdateDisplay;
        }

        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi không còn cần thiết
        if (AssetManager.instance != null)
        {
            Asset.OnEnergyReplenished -= UpdateDisplay;
        }
    }

    private void Update()
    {
        // Cập nhật thời gian hồi năng lượng mỗi frame
        if (AssetManager.instance != null)
        {
            Asset assetData = AssetManager.instance.GetAssetData();
            assetData.UpdateEnergy(Time.deltaTime); // Cập nhật năng lượng theo thời gian

            if (timeEnergyText != null)
                timeEnergyText.gameObject.SetActive(assetData.currentEnergy < assetData.maxEnergy);

            if (timeEnergyText != null && assetData.currentEnergy < assetData.maxEnergy)
                timeEnergyText.text = assetData.GetEnergyRechargeTime(); // Hiển thị thời gian hồi năng lượng
        }
    }

    public void UpdateDisplay()
    {
        if (AssetManager.instance != null)
        {
            Asset assetData = AssetManager.instance.GetAssetData();

            if (goldText != null)
                goldText.text = FormatNumber(assetData.Gold); // Rút gọn số vàng
            if (starText != null)
                starText.text = FormatNumber(assetData.Star); // Rút gọn số sao
            if (energyText != null)
                energyText.text = $"{assetData.currentEnergy} / {assetData.maxEnergy}";

            if (attacktext != null)
                attacktext.text = $"{assetData.Attack}";

            if (healthText != null)
                healthText.text = $"{assetData.Health}";
            fragment.text = $"{assetData.fragment}";
        }
        else
        {
            Debug.LogError("AssetManager instance is not available!");
        }
    }
    private string FormatNumber(long value)
    {
        if (value >= 1_000_000_000) // Tỷ (Billion)
            return $"{value / 1_000_000_000f:0.#}B";
        if (value >= 1_000_000) // Triệu (Million)
            return $"{value / 1_000_000f:0.#}M";
        if (value >= 1_000) // Nghìn (Thousand)
            return $"{value / 1_000f:0.#}K";

        return value.ToString("N0"); // Hiển thị số đầy đủ nếu nhỏ hơn 1,000
    }
}
