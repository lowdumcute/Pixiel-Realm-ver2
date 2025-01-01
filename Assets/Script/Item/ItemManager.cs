using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;


public class ItemManager : MonoBehaviour
{
    [Header("Quản lý Item")]
    [SerializeField] private ItemStats weapon;
    [SerializeField] private ItemStats helmet;
    [SerializeField] private ItemStats armor;
    [SerializeField] private ItemStats shoe;
    [SerializeField] private ItemStats ring;
    [SerializeField] private ItemStats pet;

    [Header("Panel UI")]
    [SerializeField] public GameObject weaponPanel;
    [SerializeField] public GameObject helmetPanel;
    [SerializeField] public GameObject armorPanel;
    [SerializeField] public GameObject shoePanel;
    [SerializeField] public GameObject ringPanel;
    [SerializeField] public GameObject petPanel;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemData itemData;
    [SerializeField] private GameObject NotificationPanel;
    [SerializeField] private TMP_Text NotificationText;
    private void Start()
    {
        UpdateUI();
        NotificationPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        // Cập nhật UI cho từng loại item
        UpdatePanel(weaponPanel, itemData.weaponItem, ref weapon);
        UpdatePanel(helmetPanel, itemData.helmetItem, ref helmet);
        UpdatePanel(armorPanel, itemData.armorItem, ref armor);
        UpdatePanel(shoePanel, itemData.shoeItem, ref shoe);
        UpdatePanel(ringPanel, itemData.ringItem, ref ring);
        UpdatePanel(petPanel, itemData.petItem, ref pet);
    }

    // Phương thức cập nhật từng Panel dựa trên item hiện tại
    private void UpdatePanel(GameObject panel, ItemInventory itemInventory, ref ItemStats currentItemSlot)
    {
        // Kiểm tra nếu itemInventory không null
        if (itemInventory != null)
        {
            // Kích hoạt panel nếu có item
            panel.SetActive(true);

            // Nếu UI slot chưa có, tạo một object mới từ prefab của item
            if (currentItemSlot == null)
            {
                GameObject itemObject = Instantiate(itemInventory.itemPrefab, panel.transform);
                currentItemSlot = itemObject.GetComponent<ItemStats>();

                // Đặt trạng thái cho item
                if (currentItemSlot != null)
                {
                    currentItemSlot.EquipButton.SetActive(false); // Ẩn nút trang bị
                    currentItemSlot.UnequipButton.SetActive(true); // Hiện nút gỡ bỏ
                    currentItemSlot.isEquipped = true;
                }
            }
        }
    }

    public void EquipItem(ItemStats item)
    {
        // Kiểm tra nếu có item đã trang bị trong loại này, và gọi hàm Unequip từ item đó
        ItemStats itemToUnequip = GetEquippedItem(item.Type);
        if (itemToUnequip != null)
        {
            // Hiển thị thông báo yêu cầu tháo trang bị cũ trước khi trang bị item mới
            

            Debug.LogWarning("Item đã được trang bị, hãy tháo item cũ trước.");
            return;
        }

        // Lấy target panel cho item mới
        GameObject targetPanel = GetTargetPanel(item.Type);
        if (targetPanel != null)
        {
            // Di chuyển object sang panel tương ứng
            item.transform.SetParent(targetPanel.transform);
            item.transform.localPosition = Vector3.zero;

            // Lưu item vào `ItemData`
            itemData.SetItem(new ItemInventory { Type = item.Type, itemPrefab = item.gameObject });

            // Gán tham chiếu vào biến quản lý
            switch (item.Type)
            {
                case ItemStats.ItemType.Weapon: weapon = item; break;
                case ItemStats.ItemType.Helmet: helmet = item; break;
                case ItemStats.ItemType.Armor: armor = item; break;
                case ItemStats.ItemType.Shoe: shoe = item; break;
                case ItemStats.ItemType.Ring: ring = item; break;
                case ItemStats.ItemType.Pet: pet = item; break;
            }

            Debug.Log($"{item.name} đã được trang bị.");
        }
    }

    public void UnequipItem(ItemStats.ItemType itemType)
    {
        ItemStats itemToUnequip = GetEquippedItem(itemType);

        if (itemToUnequip != null)
        {
            // Xóa item khỏi panel
            GameObject targetPanel = GetTargetPanel(itemType);
            if (targetPanel != null)
            {
                Destroy(itemToUnequip.gameObject);
            }

            // Xóa dữ liệu trong `ItemData`
            itemData.RemoveItem(itemType);

            // Xóa tham chiếu trong ItemManager
            switch (itemType)
            {
                case ItemStats.ItemType.Weapon: weapon = null; break;
                case ItemStats.ItemType.Helmet: helmet = null; break;
                case ItemStats.ItemType.Armor: armor = null; break;
                case ItemStats.ItemType.Shoe: shoe = null; break;
                case ItemStats.ItemType.Ring: ring = null; break;
                case ItemStats.ItemType.Pet: pet = null; break;
            }

            Debug.Log($"Item {itemType} đã được tháo.");
        }
        inventoryUI.UpdateUI();
    }

    private GameObject GetTargetPanel(ItemStats.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemStats.ItemType.Weapon: return weaponPanel;
            case ItemStats.ItemType.Helmet: return helmetPanel;
            case ItemStats.ItemType.Armor: return armorPanel;
            case ItemStats.ItemType.Shoe: return shoePanel;
            case ItemStats.ItemType.Ring: return ringPanel;
            case ItemStats.ItemType.Pet: return petPanel;
            default: return null;
        }
    }

    public ItemStats GetEquippedItem(ItemStats.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemStats.ItemType.Weapon: return weapon;
            case ItemStats.ItemType.Helmet: return helmet;
            case ItemStats.ItemType.Armor: return armor;
            case ItemStats.ItemType.Shoe: return shoe;
            case ItemStats.ItemType.Ring: return ring;
            case ItemStats.ItemType.Pet: return pet;
            default: return null;
        }
    }
    public void Notification()
    { 
        // Hiển thị Notification Panel
        NotificationPanel.SetActive(true);
        
        // Cập nhật nội dung thông báo
        NotificationText.text = "Tháo Item cùng loại trước khi trang bị!";

        // Bắt đầu hiệu ứng rung
        StartCoroutine(RingEffect());

        // Mờ dần NotificationPanel sau khi rung xong
        StartCoroutine(FadeOutNotification());
    }

    private IEnumerator RingEffect()
    {
        Vector3 originalPosition = NotificationPanel.transform.localPosition;
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            float shakeAmount = Mathf.Sin(timer * Mathf.PI * 2f * 3f) * 10f; // Tạo hiệu ứng rung
            NotificationPanel.transform.localPosition = originalPosition + new Vector3(shakeAmount, 0f, 0f);
            timer += Time.deltaTime;
            yield return null; // Dùng IEnumerator chuẩn, không có tham số kiểu
        }

        // Đảm bảo rằng nó trở về vị trí ban đầu
        NotificationPanel.transform.localPosition = originalPosition;
    }

    private IEnumerator FadeOutNotification()
    {
        CanvasGroup canvasGroup = NotificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = NotificationPanel.AddComponent<CanvasGroup>(); // Thêm CanvasGroup nếu chưa có
        }

        float fadeDuration = 1.5f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null; // Dùng IEnumerator chuẩn, không có tham số kiểu
        }

        canvasGroup.alpha = 0f;
        NotificationPanel.SetActive(false); // Ẩn NotificationPanel sau khi mờ
    }
}
