using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Gold, Star } // Các loại Item
    public ItemType Type;

    [SerializeField] private TMP_Text quantityText; // Text hiển thị số lượng
    [SerializeField] private Asset asset;          // Tham chiếu tới ScriptableObject Asset
    public int quantity; // Số lượng hiện tại

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

    private void UpdateText()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity.ToString(); // Cập nhật UI
        }
        else
        {
            Debug.LogWarning("Quantity Text is not assigned!");
        }
    }
}
