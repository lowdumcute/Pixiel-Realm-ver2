using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public ItemInventory itemInventory; // item add vào túi 
    [SerializeField] private Inventory inventory; // Tham chiếu tới Inventory ScriptableObject

    [Header("Component thông tin của vật phẩm")]
    [SerializeField] private GameObject PanelInformation;// panel
    [SerializeField] private Image Icon;
    [SerializeField]
    private Image CellRarityIcon;
    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Rarity;
    [SerializeField] private TextMeshProUGUI Decription;
    [SerializeField] private TextMeshProUGUI type;
    [Header("Panel rarity")]
    [SerializeField]
    private RaritySprites raritySprites;
    void Start()
    {
        SpawnItemInfo();
    }
    public void SpawnItemInfo()
    {
        // Thay đổi sprite dựa trên rarity
        Image rarityImage = gameObject.transform.GetChild(0).GetComponent<Image>(); // Object con đầu tiên từ trên xuống
        if (rarityImage != null)
        {
            UpdateRaritySprite(rarityImage, itemInventory.Rarity);
        }
    }

    private void UpdateRaritySprite(Image rarityImage, ItemStats.ItemRarity rarity)
    {
        if (raritySprites == null)
        {
            Debug.LogError("RaritySprites chưa được gán!");
            return;
        }

        switch (rarity)
        {
            case ItemStats.ItemRarity.Common:
                rarityImage.sprite = raritySprites.commonSprite;
                break;
            case ItemStats.ItemRarity.Rare:
                rarityImage.sprite = raritySprites.rareSprite;
                break;
            case ItemStats.ItemRarity.Legendary:
                rarityImage.sprite = raritySprites.legendSprite;
                break;
            default:
                rarityImage.sprite = null; // Xóa sprite nếu không có rarity phù hợp
                break;
        }
    }

    public void AddToInventory(int amount = 1)
    {
        if (itemInventory == null || inventory == null)
        {
            Debug.LogError("ItemInventory hoặc Inventory chưa được gán!");
            return;
        }

        // Gọi phương thức AddItem của Inventory để thêm item
        inventory.AddItem(itemInventory, amount);

        // Log thông tin để kiểm tra (có thể loại bỏ khi hoàn thành)
        Debug.Log($"Đã thêm {amount} x {itemInventory.itemName} vào túi!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInventory.Rarity== ItemStats.ItemRarity.Common)
        {
            CellRarityIcon.sprite = raritySprites.commonSprite;
            CellRarityIcon.color = Color.white;
            Rarity.text = "Rarity: " + $"<color=grey>{itemInventory.Rarity}</color>";
            CheckInfor();
        }
        if (itemInventory.Rarity == ItemStats.ItemRarity.Rare)
        {
            CellRarityIcon.sprite = raritySprites.rareSprite;
            CellRarityIcon.color = new Color(183f / 255f, 46f / 255f, 255f / 255f); // RGB value for purple (chuẩn hóa)
            ;
            Rarity.text = "Rarity: " + $"<color=purple>{itemInventory.Rarity}</color>";
            CheckInfor();
        }
        if (itemInventory.Rarity == ItemStats.ItemRarity.Legendary)
        {
            CellRarityIcon.sprite = raritySprites.legendSprite;
            CellRarityIcon.color = Color.yellow;
            Rarity.text = "Rarity: " + $"<color=yellow>{itemInventory.Rarity}</color>";
            CheckInfor();
        }       
    }

    
    private void CheckInfor()
    {
        PanelInformation.SetActive(true);
        Name.text = itemInventory.itemName;
        Icon.sprite = itemInventory.Icon;
        Decription.text = itemInventory.Decription;
        type.text = $"Type: <color=white>{itemInventory.Type}</color>";

        // Tính toán vị trí panel
        RectTransform canvasRect = PanelInformation.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform panelRect = PanelInformation.GetComponent<RectTransform>();

        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            Camera.main, // Đảm bảo Camera chính được sử dụng nếu cần
            out mousePosition
        );

        // Offset panel ngay dưới con chuột
        Vector2 offset = new Vector2(1000f, -500f);
        Vector2 targetPosition = mousePosition + offset;

        // Đảm bảo vị trí không vượt quá giới hạn
        targetPosition.x = Mathf.Clamp(targetPosition.x, -canvasRect.rect.width / 2, canvasRect.rect.width / 2);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -canvasRect.rect.height / 2, canvasRect.rect.height / 2);

        // Đặt vị trí
        panelRect.anchoredPosition = targetPosition;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PanelInformation.SetActive(false);
    }
}
