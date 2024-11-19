using System.Collections;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private MainHouseController mainHouseController;
    [SerializeField] private CastleHealth CastleHealth;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text playtext;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int[] enemiesPerWave;
    public float spawnInterval = 0.5f;
    public GameObject button;
    public int[] coinsPerWave;

    private int currentWave = 0;
    private int enemiesSpawned;
    private int enemiesDefeated;

    void Start()
    {
        button.SetActive(true); // Hiển thị nút khi bắt đầu
        UpdateWaveText();
        updateplaytext();
        
    }
    private void updateplaytext()
    {
        // Đảm bảo currentWave không vượt quá giới hạn của mảng coinsPerWave
        if (currentWave < coinsPerWave.Length)
        {
            playtext.text = $"Play: +{coinsPerWave[currentWave]}";
        }
        else
        {
            playtext.text = "Play: +0"; // Hiển thị +0 nếu đã hết wave
        }
    }
    private void UpdateWaveText()
    {
        int totalWaves = enemiesPerWave.Length;
        waveText.text = $"Wave: {currentWave + 1}/{totalWaves}";
    }

    public void SpawnCurrentLevel()
    {
        MainHouseController.EnterCombat();
        CastleHealth.RestoreHealth();
        gamePlayManager.DisableUpgradeLevel(); // Vô hiệu hóa nâng cấp
        Building.EnterCombat(); // Kích hoạt chế độ chiến đấu

        if (currentWave < enemiesPerWave.Length)
        {
            Miner[] miners = FindObjectsOfType<Miner>();
            foreach (Miner miner in miners)
            {
                miner.inGame();
            }

            button.SetActive(false); // Ẩn nút sau khi bắt đầu wave
            enemiesSpawned = 0;
            enemiesDefeated = 0;
            StartCoroutine(SpawnEnemies(enemiesPerWave[currentWave]));
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    IEnumerator SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        enemiesSpawned++;
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        // Khi tiêu diệt hết quái trong wave
        if (enemiesDefeated >= enemiesPerWave[currentWave])
        {
            CastleHealth.OnEnemyDefeated();
            MainHouseController.ExitCombat();
            Building.ExitCombat(); // Thoát chế độ chiến đấu
            gamePlayManager.AddCoins(coinsPerWave[currentWave]);

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

            currentWave++;
            UpdateWaveText();
            button.SetActive(true);
            gamePlayManager.EnableUpgradeLevel();
        }
    }
}
