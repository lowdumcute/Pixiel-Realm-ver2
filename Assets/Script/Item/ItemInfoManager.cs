using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemInfoManager : MonoBehaviour
{
    [SerializeField] private GameObject itemInfoPrefab; // Prefab item info
    [SerializeField] private ItemInfoReceiver itemInfoReceiver; // Object chứa script ItemInfoReceiver
    [SerializeField] private RaritySprites raritySprites; // Tham chiếu đến ScriptableObject RaritySprites

    private GameObject currentItemInfo; // Lưu tham chiếu đến item info hiện tại
    [SerializeField] private ItemInventory currentItemInventory; // Tham chiếu tới ItemInventory

    private void Start()
    {
        // Tìm đối tượng có ItemInfoReceiver nếu chưa gán
        if (itemInfoReceiver == null)
        {
            itemInfoReceiver = FindObjectOfType<ItemInfoReceiver>();
        }

        if (itemInfoReceiver == null)
        {
            Debug.LogError("Không tìm thấy ItemInfoReceiver trong scene!");
        }
    }

    public void SpawnItemInfo()
    {
        if (itemInfoPrefab == null)
        {
            Debug.LogError("ItemInfoPrefab chưa được gán!");
            return;
        }

        // Xóa item info trước đó nếu tồn tại
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo);
        }

        // Spawn prefab làm con của ItemInfoReceiver
        currentItemInfo = Instantiate(itemInfoPrefab, itemInfoReceiver.transform);

        // Thay đổi sprite dựa trên rarity
        Image rarityImage = currentItemInfo.transform.GetChild(0).GetComponent<Image>(); // Object con đầu tiên từ trên xuống
        if (rarityImage != null)
        {
            UpdateRaritySprite(rarityImage, currentItemInventory.Rarity);
        }

        // Lấy tên item từ prefab và gán nó cho TextMeshPro
        TextMeshProUGUI itemNameText = currentItemInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>(); // Object con thứ 2 từ trên xuống
        if (itemNameText != null)
        {
            itemNameText.text = currentItemInventory.itemName; // Hiển thị tên item
            UpdateItemColorByRarity(itemNameText, currentItemInventory.Rarity); // Thay đổi màu sắc tên item dựa trên rarity
        }

        // Lấy mô tả item từ prefab và gán nó cho TextMeshPro
        TextMeshProUGUI Decription = currentItemInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>(); // Object con thứ 3 từ trên xuống
        if (Decription != null)
        {
            // Mô tả item
            string descriptionText = currentItemInventory.Decription; 

            // Chỉ số tấn công, hiển thị màu đỏ cho số
            string attackText = currentItemInventory.Attack > 0 
                ? $"<color=white>Tấn Công: </color><color=red>{currentItemInventory.Attack}</color>\n"
                : "";

            // Chỉ số máu, hiển thị màu xanh cho số
            string healthText = currentItemInventory.Health > 0 
                ? $"<color=white>Máu: </color><color=green>{currentItemInventory.Health}</color>\n"
                : "";

            // Gộp tất cả thông tin
            Decription.text = $"{descriptionText}\n{attackText}{healthText}";
        }

        // Hiển thị số sao
        Transform starsParent = currentItemInfo.transform.GetChild(3); // Object con thứ 4 từ trên xuống để chứa các sao
        if (starsParent != null)
        {
            SpawnStars(starsParent, currentItemInventory.CurrentStart); // Hiển thị số sao
        }
        TextMeshProUGUI Rarity = currentItemInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>(); // Object con thứ 5 từ trên xuống
        if (Rarity != null)
        {
            // Lấy màu sắc từ rarity
            Color rarityColor = GetColorByRarity(currentItemInventory.Rarity);

            // Gán text với định dạng màu sắc riêng cho rarity type
            Rarity.text = $"Rarity: <color=#{ColorUtility.ToHtmlStringRGB(rarityColor)}>{currentItemInventory.Rarity}</color>";
        }

    }

    private void SpawnStars(Transform starsParent, int currentStars)
    {
        // Xóa các sao cũ
        foreach (Transform child in starsParent)
        {
            Destroy(child.gameObject);
        }

        // Kiểm tra xem RaritySprites đã được gán hay chưa
        if (raritySprites == null)
        {
            Debug.LogError("RaritySprites chưa được gán trong Inspector!");
            return;
        }

        // Tạo mới các sao
        for (int i = 0; i < 5; i++)
        {
            // Tạo sao mới
            GameObject star = new GameObject($"Star_{i + 1}");
            star.transform.SetParent(starsParent);
            star.transform.localScale = Vector3.one;

            // Thêm thành phần hình ảnh
            Image starImage = star.AddComponent<Image>();
            starImage.sprite = i < currentStars ? raritySprites.startFillSprite : raritySprites.startEmptySprite;

            // Đặt vị trí (nếu cần tùy chỉnh khoảng cách giữa các sao)
            RectTransform rectTransform = star.GetComponent<RectTransform>();
        }
    }


    private void UpdateItemColorByRarity(TextMeshProUGUI itemNameText, ItemStats.ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemStats.ItemRarity.Common:
                itemNameText.color = Color.gray; // Màu bạc cho Common
                break;
            case ItemStats.ItemRarity.Rare:
                itemNameText.color = new Color(0.5f, 0.0f, 0.5f); // Màu tím đậm cho Rare
                break;
            case ItemStats.ItemRarity.Legendary:
                itemNameText.color = Color.yellow; // Màu vàng cho Legendary
                break;
            default:
                itemNameText.color = Color.white; // Mặc định màu trắng nếu không phải các rarity trên
                break;
        }
    }
    private Color GetColorByRarity(ItemStats.ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemStats.ItemRarity.Common:
                return Color.gray; // Màu bạc cho Common
            case ItemStats.ItemRarity.Rare:
                return new Color(0.5f, 0.0f, 0.5f); // Màu tím đậm cho Rare
            case ItemStats.ItemRarity.Legendary:
                return Color.yellow; // Màu vàng cho Legendary
            default:
                return Color.white; // Mặc định màu trắng
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

    private void Update()
    {
        // Kiểm tra xem người dùng có nhấn bất kỳ nút nào không
        if (Input.anyKeyDown)
        {
            ClearItemInfo();
        }
    }

    public void ClearItemInfo()
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo);
            currentItemInfo = null;
        }
    }
}
