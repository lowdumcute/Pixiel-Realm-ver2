using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Gold, Star } // Các loại Item
    public ItemType Type;
    [SerializeField] private TMP_Text quantityText; // Text hiển thị số lượng
    public int quantity; // Số lượng hiện tại

    public void UpdateQuantity(int amount)
    {
        quantity += amount; // Cập nhật số lượng
        UpdateText();

            Asset.instance.AddToElement(Type, amount);
    }

    private void UpdateText()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity.ToString(); // Cập nhật UI hiển thị số lượng
        }
        else
        {
            Debug.LogWarning("Đéo tìm thấy text");
        }
    }
}
