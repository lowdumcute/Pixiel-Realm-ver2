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
            if (item.itemPrefab != null)
            {
                // Tạo một bản sao của itemPrefab
                GameObject spawnedItem = Instantiate(item.itemPrefab, transform);

                // Tạo object con chứa hình ảnh itemIcon
                GameObject iconObject = new GameObject("ItemIcon");
                iconObject.transform.SetParent(spawnedItem.transform);

                // Đặt vị trí và scale mặc định
                iconObject.transform.localPosition = Vector3.zero;
                iconObject.transform.localScale = Vector3.one; // Đặt scale mặc định là (1, 1, 1)

                // Gắn Image component và gán sprite
                Image icon = iconObject.AddComponent<Image>();
                icon.sprite = item.itemIcon;

                // Đảm bảo iconObject được đặt ở cuối danh sách con của spawnedItem
                iconObject.transform.SetAsFirstSibling();
            }
            else
            {
                Debug.LogWarning($"Item {item.itemName} không có itemPrefab được gắn!");
            }
        }
    }
}
