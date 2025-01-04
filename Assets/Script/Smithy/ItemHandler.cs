using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
{
    [Header("Thông tin item")]
    public ItemInventory itemData; // Dữ liệu item này

    [Header("Panel cho Inventory và Upgrade")]
    public Transform inventoryPanel;
    public Transform upgradePanel;

    [Header("Tham chiếu đến")]
    public SmithyInventory smithyInventory;
    public UpgradeUI upgradeUI;

    [Header("Trạng thái Upgrade")]
    public bool isUpgrade = false; // Trạng thái isUpgrade của item này

    private void Start()
    {
        smithyInventory = FindObjectOfType<SmithyInventory>();
        upgradeUI = FindObjectOfType<UpgradeUI>();
    }

    // Thay đổi trạng thái isUpgrade
    public void ToggleUpgradeState()
    {
        // Kiểm tra xem upgradePanel có item nào chưa
        if (upgradePanel.childCount > 0)
        {
            // Nếu có item trong upgradePanel, thay đổi isUpgrade của item hiện tại về false
            ItemHandler existingItemHandler = upgradePanel.GetChild(0).GetComponent<ItemHandler>();
            if (existingItemHandler != null)
            {
                existingItemHandler.isUpgrade = false; // Set trạng thái isUpgrade về false cho item cũ
                existingItemHandler.transform.SetParent(inventoryPanel, false); // Di chuyển item cũ về Inventory
                existingItemHandler.Arrange(); // Cập nhật lại
            }
        }

        // Đổi trạng thái isUpgrade của item này
        isUpgrade = !isUpgrade;

        // Di chuyển item giữa các panel
        Transform currentParent = transform.parent;
        Transform targetPanel = isUpgrade ? upgradePanel : inventoryPanel;
        upgradeUI.itemData = itemData;
        upgradeUI.UpdateUI();
        Arrange();

        if (currentParent != targetPanel)
        {
            transform.SetParent(targetPanel, false);
        }

        Debug.Log($"Item {itemData.itemName} chuyển sang trạng thái isUpgrade = {isUpgrade}");
    }

    public void Arrange()
    {
        smithyInventory.Arrange();
    }
}
