using UnityEngine;

[CreateAssetMenu(fileName = "TaiNguyen", menuName = "ScriptableObjects/Asset")]
public class Asset : ScriptableObject
{
    public float Gold = 0;
    public float Star = 0;
    public int currentEnergy;
    public int maxEnergy = 30;
    [Header ("Stat PLayer")]
    public float Health = 100;
    public int Attack = 10;



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
    public void AddStatPlayer(Stats.StatsType type, int amount)
    {
        switch (type)
        {
            case Stats.StatsType.Health:
                Health += amount;
                break;
            case Stats.StatsType.Attack:
                Attack += amount;
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
            currentEnergy = maxEnergy; // Không vượt quá giới hạn
        }
    }
}
