using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Panel UI")]
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject helmetPanel;
    [SerializeField] private GameObject armorPanel;
    [SerializeField] private GameObject shoePanel;
    [SerializeField] private GameObject ringPanel;
    [SerializeField] private GameObject petPanel;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Asset Display")]
    [SerializeField] private Asset playerAsset; // Tham chiếu đến Asset của người chơi

    private void Start()
    {
        if (playerAsset == null)
        {
            Debug.LogError("Player Asset is not assigned!");
        }
        
    }

    // Gọi khi trang bị item
    public void EquipItem(ItemStats item)
    {
        if (playerAsset != null)
        {
            playerAsset.EquipItem(item); // Gọi phương thức trang bị item từ Asset

            // Cập nhật hiển thị UI
            UpdateDisplay();
        }
    }

    // Cập nhật giao diện UI sau khi trang bị item
    public void UpdateDisplay()
    {
        if (playerAsset != null)
        {
            if (playerAsset.weapon != null)
            {
                weaponPanel.SetActive(true);
            }
            if (playerAsset.helmet != null)
            {
                helmetPanel.SetActive(true);
            }
            if (playerAsset.armor != null)
            {
                armorPanel.SetActive(true);
            }
            if (playerAsset.shoe != null)
            {
                shoePanel.SetActive(true);
            }
            if (playerAsset.ring != null)
            {
                ringPanel.SetActive(true);
            }
            if (playerAsset.pet != null)
            {
                petPanel.SetActive(true);
            }
        }
    }
}
