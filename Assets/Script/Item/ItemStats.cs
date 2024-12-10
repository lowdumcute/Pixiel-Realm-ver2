using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public enum ItemType { Weapon, Helmet, Armor, Shoe, Ring, Pet } // Loại Item
    [Header("Stat Item")]
    public ItemType Type;
    [SerializeField] public int Attack = 0; // Giá trị Attack của item
    [SerializeField] public int Health = 0; // Giá trị Health của item
    [SerializeField] private Asset playerAsset; // Tham chiếu đến Asset người chơi
    [SerializeField] private AssetDisplay assetDisplay;

    private bool isEquipped = false; // Trạng thái trang bị

    private void Start()
    {
        if (assetDisplay == null)
        {
            assetDisplay = FindObjectOfType<AssetDisplay>();
        }
    }

    public void Equip()
    {
        if (isEquipped)
        {
            Debug.LogWarning("Item is already equipped!");
            return;
        }

        if (playerAsset != null)
        {
            // Gọi phương thức EquipItem trong Asset để trang bị item
            playerAsset.EquipItem(this);  // Truyền đúng item vào (this là đối tượng ItemStats hiện tại)
            // Thêm stat cho nhân vật
            if (Attack != 0) playerAsset.AddStatPlayer(Stats.StatsType.Attack, Attack);
            if (Health != 0) playerAsset.AddStatPlayer(Stats.StatsType.Health, Health);

            // Cập nhật lại UI
            assetDisplay.UpdateDisplay();

            isEquipped = true;
            Debug.Log($"Equipped {gameObject.name}. Added Attack: {Attack}, Health: {Health}.");
        }
        else
        {
            Debug.LogError("Player Asset is not assigned!");
        }
    }

    /// Tháo trang bị item, trừ giá trị của item khỏi stats nhân vật.
    public void Unequip()
    {
        if (!isEquipped)
        {
            Debug.LogWarning("Item is not equipped!");
            return;
        }

        if (playerAsset != null)
        {
            // Trừ stat của nhân vật khi tháo item
            if (Attack != 0) playerAsset.MinusStatPlayer(Stats.StatsType.Attack, Attack);
            if (Health != 0) playerAsset.MinusStatPlayer(Stats.StatsType.Health, Health);

            // Xóa item khỏi Asset
            playerAsset.EquipItem(null); // Đặt item về null để tháo trang bị

            isEquipped = false;
            Debug.Log($"Unequipped {gameObject.name}. Removed Attack: {Attack}, Health: {Health}.");
        }
        else
        {
            Debug.LogError("Player Asset is not assigned!");
        }
    }
}
