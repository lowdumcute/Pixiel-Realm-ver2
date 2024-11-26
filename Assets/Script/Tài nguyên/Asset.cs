using UnityEngine;
public class Asset : MonoBehaviour
{
    public static Asset instance;

    public static float Gold = 0;
    public static float Star = 0;
    public int currentEnergy;
    public int maxEnergy = 30;

    private void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Giữ đối tượng giữa các cảnh
        }
        else
        {
            Destroy(gameObject); // Hủy đối tượng nếu đã có instance
        }
    }

    private void Start()
    {
        // Đặt năng lượng hiện tại bằng năng lượng tối đa khi bắt đầu
        currentEnergy = maxEnergy;
    }

    public void AddToElement(Item.ItemType type, int amount)
    {
        switch (type)
        {
            case Item.ItemType.Gold:
                Gold += amount;
                break;
            case Item.ItemType.Star:
                Star += amount;
                break;
        }
    }

    public void UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough energy!");
        }
    }

    public void ReplenishEnergy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy; // Đảm bảo không vượt quá giới hạn
        }
    }

    public float GetGold()
    {
        return Gold;
    }

    public float GetStar()
    {
        return Star;
    }

    public int GetEnergy()
    {
        return currentEnergy;
    }
}
