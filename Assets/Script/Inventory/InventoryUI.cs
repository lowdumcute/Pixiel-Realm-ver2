using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory; // Tham chiếu tới Inventory ScriptableObject

    private void Start()
    {
        UpdateUI(); // Hiển thị inventory khi bắt đầu
    }

    public void UpdateUI()
    {
        // Xóa các object con cũ
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Tạo mới UI từ danh sách item trong inventory
        foreach (ItemInventory item in inventory.items)
        {
            if (item.currentQuantity > 0) // Kiểm tra nếu số lượng lớn hơn 0
            {
                if (item.itemPrefab != null)
                {
                    // Tạo object dựa trên currentQuantity
                    for (int i = 0; i < item.currentQuantity; i++)
                    {
                        GameObject spawnedItem = Instantiate(item.itemPrefab, transform);
                        spawnedItem.name = $"{item.itemName}_{i + 1}"; // Đặt tên để dễ phân biệt
                    }
                }
                else
                {
                    Debug.LogWarning($"Item {item.itemName} không có itemPrefab được gắn!");
                }
            }
        }
    }
}
