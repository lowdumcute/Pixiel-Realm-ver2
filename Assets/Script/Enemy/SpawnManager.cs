using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Để làm việc với Image
using TMPro;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class WaveConfig
{
    public List<EnemyConfig> enemyConfigs; // Danh sách các loại enemy và số lượng
    public int coinsReward; // Phần thưởng khi hoàn thành wave
}

[System.Serializable]
public class EnemyConfig
{
    public GameObject enemyPrefab; // Prefab của enemy
    public int amount; // Số lượng spawn
}

public class SpawnManager : MonoBehaviour
{
    [Header("Tham chiếu")]
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private MainHouseController mainHouseController;
    [SerializeField] private CastleHealth CastleHealth;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text playtext;
    [SerializeField] private Light2D dayLight; // Light 2D để điều chỉnh ngày đêm

    [Header("Enemy Configurations")]
    [SerializeField] private List<GameObject> enemyPrefabs; // Danh sách các prefab enemy
    [SerializeField] private List<WaveConfig> waveConfigs; // Danh sách config cho từng wave

    [Header("Spawn Configurations")]
    [SerializeField] private Transform[] spawnPoints; // Điểm spawn
    [SerializeField] private float spawnInterval = 0.5f; // Khoảng cách giữa các lần spawn
    [SerializeField] private GameObject button;
    [SerializeField] private float fadeDuration = 1.0f; // Thời gian điều chỉnh ánh sáng ngày đêm

    private int currentWave = 0;
    private int enemiesSpawned;
    private int enemiesDefeated;

    void Start()
    {
        button.SetActive(true);
        UpdateWaveText();
        UpdatePlayText();

        // Đặt cường độ ánh sáng ban đầu
        if (dayLight != null)
        {
            dayLight.intensity = 1.0f; // Ban ngày
        }
    }

    private void UpdatePlayText()
    {
        if (currentWave < waveConfigs.Count)
            playtext.text = $"Play: +{waveConfigs[currentWave].coinsReward}";
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave: {currentWave + 1}/{waveConfigs.Count}";
    }

    public void SpawnCurrentLevel()
    {
        MainHouseController.EnterCombat();
        AudioManager.instance.PlayMusic("FightGame");
        CastleHealth.RestoreHealth();
        gamePlayManager.DisableUpgradeLevel();
        Building.EnterCombat();

        if (currentWave < waveConfigs.Count)
        {
            // Chuyển sang ban đêm
            if (dayLight != null)
            {
                StartCoroutine(AdjustLightIntensity(1.0f, 0.1f));
            }

            Miner[] miners = FindObjectsOfType<Miner>();
            foreach (Miner miner in miners)
            {
                miner.inGame();
            }

            button.SetActive(false);
            enemiesSpawned = 0;
            enemiesDefeated = 0;

            // Bắt đầu spawn cho wave hiện tại
            StartCoroutine(SpawnEnemiesForWave(waveConfigs[currentWave]));
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private IEnumerator SpawnEnemiesForWave(WaveConfig waveConfig)
    {
        foreach (var enemyData in waveConfig.enemyConfigs)
        {
            for (int i = 0; i < enemyData.amount; i++)
            {
                SpawnEnemy(enemyData.enemyPrefab);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        enemiesSpawned++;
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        if (enemiesDefeated >= enemiesSpawned)
        {
            AudioManager.instance.PlayMusic("IdleGame");
            CastleHealth.OnEnemyDefeated();
            MainHouseController.ExitCombat();
            Building.ExitCombat();
            gamePlayManager.AddCoins(waveConfigs[currentWave].coinsReward);

            Miner[] miners = FindObjectsOfType<Miner>();
            foreach (Miner miner in miners)
            {
                miner.AddCoin();
            }

            Barracks[] barracks = FindObjectsOfType<Barracks>();
            foreach (Barracks barracksInstance in barracks)
            {
                barracksInstance.ResetUnitsPositions();
            }

            TowerHealth[] towers = FindObjectsOfType<TowerHealth>();
            foreach (TowerHealth towerInstance in towers)
            {
                towerInstance.RestoreHealth();
            }

            // Chuyển về ban ngày
            if (dayLight != null)
            {
                StartCoroutine(AdjustLightIntensity(0.1f, 1.0f));
            }

            currentWave++;
            if (currentWave < waveConfigs.Count)
            {
                UpdateWaveText();
                button.SetActive(true);
                gamePlayManager.EnableUpgradeLevel();
                UpdatePlayText();
            }
            else
            {
                gamePlayManager.Win();
            }
        }
    }

    private IEnumerator AdjustLightIntensity(float fromIntensity, float toIntensity)
    {
        if (dayLight != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                dayLight.intensity = Mathf.Lerp(fromIntensity, toIntensity, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            dayLight.intensity = toIntensity; // Đảm bảo cường độ ánh sáng được đặt chính xác
        }
    }
}
