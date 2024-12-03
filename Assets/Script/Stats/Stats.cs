using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public enum StatsType { Attack, Health } 
    public StatsType Type;

    [SerializeField] private TMP_Text quantityText; 
    [SerializeField] private Asset asset;          
    private int quantity;      // Giá trị hiện tại
    private int nextQuantity;  // Giá trị kế tiếp

    private void Start()
    {
        UpdateText(); // Khởi tạo text hiển thị
    }

    public void UpdateQuantity(int amount)
    {
        if (asset != null)
        {
            asset.AddStatPlayer(Type, amount); 
        }
        else
        {
            Debug.LogError("Asset reference is missing!");
        }

        quantity += amount; 
        UpdateText();
    }

    public void PreviewNextQuantity(int additionalAmount)
    {
        // Hiển thị số lượng kế tiếp khi nâng cấp
        nextQuantity = quantity + additionalAmount; 
        UpdateText();
    }

    private void UpdateText()
    {
        if (quantityText != null)
        {
            // Hiển thị kiểu "Current -> Next"
            quantityText.text = $"{quantity} -> {nextQuantity}"; 
        }
        else
        {
            Debug.LogWarning("Quantity Text is not assigned!");
        }
    }
}
