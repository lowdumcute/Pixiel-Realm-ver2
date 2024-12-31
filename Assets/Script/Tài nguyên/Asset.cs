using UnityEngine;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "TaiNguyen", menuName = "ScriptableObjects/Asset")]
public class Asset : ScriptableObject
{
    public float Gold = 0;
    public float Star = 0;
    public int currentEnergy;
    public int maxEnergy = 30;

    [Header("Stat Player")]
    public int Health = 100;
    public int Attack = 10;
    [Header("Stats Constuction")]
    public int HealthCastle = 400;
    public int AttackTower = 20;
    public int Coin =2;
    [Header("Fragment")]
    public int fragmentWeapon = 0;
    public int fragmentHelmet = 0;
    public int fragmentArmor = 0;
    public int fragmentShoe = 0;
    public int fragmentRing = 0;
    public int fragmentPet = 0;
    [Header("Energy")]
    public float energyRechargeTime = 60f; // 1 phút cho 1 năng lượng
    private float timeSinceLastEnergyReplenish = 0f;
    private string lastEnergySaveKey = "LastEnergySave";
    public static event Action OnEnergyReplenished; // Sự kiện khi năng lượng được hồi

    private const string LastPlayedTimeKey = "LastPlayedTime";

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
            case Stats.StatsType.HealthPlayer:
                Health += amount;
                break;
            case Stats.StatsType.AttackPlayer:
                Attack += amount;
                break;
        }
    }
    public void AddStatConstructer(Stats.StatsType type, int amount)
    {
        switch(type)
        {
            case Stats.StatsType.HealthCastle:
                HealthCastle += amount;
                break;
            case Stats.StatsType.AttackTower:
                AttackTower += amount;
                break;
            case Stats.StatsType.Coin:
                Coin += amount;
                break;

        }
    }
    public void MinusStatPlayer(Stats.StatsType type, int amount)
    {
        switch (type)
        {
            case Stats.StatsType.HealthPlayer:
                Health -= amount;
                break;
            case Stats.StatsType.AttackPlayer:
                Attack -= amount;
                break;
        }
    }

    public void UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            SaveLastPlayedTime();
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

    // Lưu thời gian khi người chơi thoát
    public void SaveLastPlayedTime()
    {
        PlayerPrefs.SetString(LastPlayedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    // Hồi năng lượng dựa trên thời gian offline
    public void RestoreEnergyFromOffline()
    {
        if (PlayerPrefs.HasKey(LastPlayedTimeKey))
        {
            // Lấy thời gian lưu trước đó
            string lastPlayedTimeString = PlayerPrefs.GetString(LastPlayedTimeKey);
            DateTime lastPlayedTime = DateTime.Parse(lastPlayedTimeString);

            // Tính thời gian trôi qua
            TimeSpan timeElapsed = DateTime.Now - lastPlayedTime;
            float secondsElapsed = (float)timeElapsed.TotalSeconds;

            // Tính số năng lượng hồi được
            int energyToReplenish = Mathf.FloorToInt(secondsElapsed / energyRechargeTime);

            if (energyToReplenish > 0)
            {
                ReplenishEnergy(energyToReplenish);

                // Reset thời gian chờ còn lại
                timeSinceLastEnergyReplenish = secondsElapsed % energyRechargeTime;
            }
        }
    }
    public void RestoreEnergyOnLoad()
    {
        if (PlayerPrefs.HasKey(lastEnergySaveKey))
        {
            DateTime lastPlayedTime = DateTime.Parse(PlayerPrefs.GetString(lastEnergySaveKey));
            TimeSpan timePassed = DateTime.Now - lastPlayedTime;

            // Tính toán số năng lượng được hồi
            int energyRecovered = Mathf.FloorToInt((float)timePassed.TotalSeconds / energyRechargeTime);
            if (energyRecovered > 0)
            {
                currentEnergy = Mathf.Min(currentEnergy + energyRecovered, maxEnergy);

                // Gọi sự kiện nếu có thay đổi năng lượng
                OnEnergyReplenished?.Invoke();

                // Lưu lại thời gian còn lại
                SaveLastPlayedTime();
            }
        }
    }
    public int GetStat(Stats.StatsType type)
    {
        switch (type)
        {
            case Stats.StatsType.HealthPlayer:
                return Health;
            case Stats.StatsType.AttackPlayer:
                return Attack;
            case Stats.StatsType.HealthCastle:
                return HealthCastle;
            case Stats.StatsType.AttackTower:
                return AttackTower;
            case Stats.StatsType.Coin:
                return Coin;
            default:
                Debug.LogWarning($"Stat type {type} not recognized!");
                return 0;
        }
    }
}
