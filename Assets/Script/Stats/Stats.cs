using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public enum StatsType { AttackPlayer, HealthPlayer, HealthCastle, AttackTower, Coin }
    public StatsType Type;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Asset asset; // Đối tượng Asset sẽ cung cấp chỉ số từ stats
    private int quantity;     // Giá trị hiện tại
    [SerializeField] public int AddQuantity;
    private int nextQuantity; // Giá trị kế tiếp

    private void Start()
    {
        // Lấy giá trị hiện tại từ Asset theo loại StatsType
        if (asset != null)
        {
            quantity = asset.GetStat(Type); // Lấy chỉ số từ asset
            nextQuantity = quantity + AddQuantity;
        }
        else
        {
            Debug.LogError("Asset reference is missing!");
        }

        UpdateText(); // Khởi tạo text hiển thị
    }

    public void UpdateQuantity(int amount)
    {
        if (asset != null)
        {
            // Cập nhật giá trị trong Asset
            asset.AddStatPlayer(Type, amount);
            asset.AddStatConstructer(Type, amount);

            // Lấy lại giá trị hiện tại sau khi cập nhật
            quantity = asset.GetStat(Type);
        }
        else
        {
            Debug.LogError("Asset reference is missing!");
        }

        UpdateText();
    }

    public void PreviewNextQuantity()
    {
        // Tính toán số lượng kế tiếp dựa trên giá trị hiện tại
        nextQuantity = quantity + AddQuantity;
        UpdateText();
    }

    private void UpdateText()
    {
        if (quantityText != null)
        {
            // Hiển thị kiểu "Hiện tại -> Kế tiếp"
            if (nextQuantity > quantity)
            {
                quantityText.text = $"{quantity} -> {nextQuantity}";
            }
            else
            {
                quantityText.text = $"{quantity}";
            }
        }
        else
        {
            Debug.LogWarning("Quantity Text is not assigned!");
        }
    }
}
