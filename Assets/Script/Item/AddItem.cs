using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemInventory itemInventory; // item add vào túi 
    [SerializeField] private Inventory inventory; // Tham chiếu tới Inventory ScriptableObject

    [Header("Component thông tin của vật phẩm")]
    [SerializeField] private GameObject PanelInformation;// panel
    [SerializeField]
    private Image Icon;
    [SerializeField]
    private Image CellRarityIcon;
    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Rarity;
    [SerializeField] private TextMeshProUGUI Decription;
    [Header("Panel rarity")]
    [SerializeField]
    private Sprite Common;
    
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
            CellRarityIcon.sprite = Common;
            CellRarityIcon.color = Color.white;
            Rarity.text = "Rarity: " + $"<color=grey>{itemInventory.Rarity}</color>";
            CheckInfor();
        }
        if (itemInventory.Rarity == ItemStats.ItemRarity.Rare)
        {
            CellRarityIcon.sprite = Common;
            CellRarityIcon.color = new Color(183f / 255f, 46f / 255f, 255f / 255f); // RGB value for purple (chuẩn hóa)
            ;
            Rarity.text = "Rarity: " + $"<color=purple>{itemInventory.Rarity}</color>";
            CheckInfor();
        }
        if (itemInventory.Rarity == ItemStats.ItemRarity.Legendary)
        {
            CellRarityIcon.sprite = Common;
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
        Decription.text = itemInventory.name;

        // Tính toán vị trí panel
        RectTransform canvasRect = PanelInformation.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform panelRect = PanelInformation.GetComponent<RectTransform>();

        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null, // Camera (null nếu Canvas là Screen Space - Overlay)
            out mousePosition
        );

        // Offset panel ngay dưới con chuột
        Vector2 offset = new Vector2(800f, -400f);
        Vector2 targetPosition = mousePosition + offset;

        // Đảm bảo vị trí không vượt quá giới hạn
        targetPosition.x = Mathf.Clamp(targetPosition.x, -1000, 1000);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -1000, 1000);

        // Đặt vị trí
        panelRect.anchoredPosition = targetPosition;




    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PanelInformation.SetActive(false);
    }
}
