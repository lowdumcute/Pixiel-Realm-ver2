using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    public enum ItemType { Gold, Star } // Các loại Item
    public ItemType Type;

    [SerializeField] private TMP_Text quantityText; // Text hiển thị số lượng
    [SerializeField] private Asset asset;          // Tham chiếu tới ScriptableObject Asset
    public int quantity; // Số lượng hiện tại
    void Start()
    {
        UpdateText();
    }

    public void UpdateQuantity(int amount)
    {
        quantity += amount; // Cập nhật số lượng
        UpdateText();

        if (asset != null)
        {
            asset.AddToElement(Type, amount); // Cập nhật tài nguyên trong Asset
        }
        else
        {
            Debug.LogError("Asset reference is missing!");
        }
    }
    public void Add()
    {
        asset.AddToElement(Type, quantity); // Cập nhật tài nguyên trong Asset
    }

    private void UpdateText()
    {
        if (quantityText != null)
        {
            // Gọi hàm rút gọn số lượng và gán vào text
            quantityText.text = ShortenNumber(quantity);
        }
        else
        {
            Debug.LogWarning("Quantity Text is not assigned!");
        }
    }

    private string ShortenNumber(int number)
    {
        if (number >= 1_000_000)
            return (number / 1_000_000f).ToString("0.#") + "M"; // Rút gọn thành 'M'
        else if (number >= 1_000)
            return (number / 1_000f).ToString("0.#") + "K"; // Rút gọn thành 'K'
        else
            return number.ToString(); // Số nhỏ hơn 1.000 giữ nguyên
    }
}
