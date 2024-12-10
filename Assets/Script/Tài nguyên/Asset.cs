using UnityEngine;
using System;

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

    public float energyRechargeTime = 60f; // 1 phút cho 1 năng lượng
    private float timeSinceLastEnergyReplenish = 0f;

    public static event Action OnEnergyReplenished; // Sự kiện khi năng lượng được hồi

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

    public void UpdateEnergy(float deltaTime)
    {
        if (currentEnergy < maxEnergy)
        {
            timeSinceLastEnergyReplenish += deltaTime;

            // Kiểm tra thời gian đã đủ để hồi 1 năng lượng
            if (timeSinceLastEnergyReplenish >= energyRechargeTime)
            {
                currentEnergy++;
                timeSinceLastEnergyReplenish = 0f; // Reset thời gian

                // Gửi thông báo đã hồi năng lượng
                OnEnergyReplenished?.Invoke();
            }
        }
    }

    public string GetEnergyRechargeTime()
    {
        float timeRemaining = energyRechargeTime - timeSinceLastEnergyReplenish;
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        return $"{minutes:D2}:{seconds:D2}"; // Format phút:giây
    }
}
