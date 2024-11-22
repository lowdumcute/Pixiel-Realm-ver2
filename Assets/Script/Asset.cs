using UnityEngine;

public class Asset : MonoBehaviour
{
    public static Asset instance;

    public static float Gold = 0;
    public static float Star = 0;

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

    public float GetGold()
    {
        return Gold;
    }

    public float GetStar()
    {
        return Star;
    }
}
