using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Để làm việc với Image
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

    [SerializeField] private Image dayNightImage; // Tham chiếu đến Image
    [SerializeField] private float fadeDuration = 1.0f; // Thời gian hiệu ứng tăng giảm alpha

    private int currentWave = 0;
    private int enemiesSpawned;
    private int enemiesDefeated;

    void Start()
    {
        button.SetActive(true); // Hiển thị nút khi bắt đầu
        UpdateWaveText();
        updateplaytext();
        
        // Đặt alpha ban đầu của dayNightImage về 0 (ban ngày)
        if (dayNightImage != null)
        {
            SetImageAlpha(0);
        }
    }

    private void updateplaytext()
    {
        playtext.text = $"Play: +{coinsPerWave[currentWave]}";
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
            // Tăng dần alpha (chuyển sang ban đêm)
            if (dayNightImage != null)
            {
                StartCoroutine(FadeImageAlpha(0, 120));
            }

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
        if (enemiesDefeated >= enemiesPerWave[currentWave])
        {
            CastleHealth.OnEnemyDefeated();
            MainHouseController.ExitCombat();
            Building.ExitCombat();
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

            // Giảm dần alpha (chuyển về ban ngày)
            if (dayNightImage != null)
            {
                StartCoroutine(FadeImageAlpha(120, 0));
            }

            currentWave++;
            if (currentWave < enemiesPerWave.Length)
            {
                UpdateWaveText();
                button.SetActive(true);
                gamePlayManager.EnableUpgradeLevel();
                updateplaytext();
            }
            else
            {
                // Nếu là wave cuối, gọi phương thức Win
                gamePlayManager.Win();
            }
        }
    }

    // Coroutine để thay đổi alpha dần
    private IEnumerator FadeImageAlpha(float fromAlpha, float toAlpha)
    {
        if (dayNightImage != null)
        {
            float elapsedTime = 0f;
            Color color = dayNightImage.color;
            float startAlpha = fromAlpha / 255f;
            float endAlpha = toAlpha / 255f;

            while (elapsedTime < fadeDuration)
            {
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                dayNightImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Đảm bảo giá trị cuối cùng được đặt chính xác
            color.a = endAlpha;
            dayNightImage.color = color;
        }
    }

    // Hàm tiện ích để thiết lập alpha ngay lập tức (nếu cần)
    private void SetImageAlpha(float alpha)
    {
        if (dayNightImage != null)
        {
            Color color = dayNightImage.color;
            color.a = alpha / 255f; // Chuyển alpha sang giá trị 0-1
            dayNightImage.color = color;
        }
    }
    // Hàm public để lấy số lượng quái đã tiêu diệt
    public int GetEnemiesDefeated()
    {
        return enemiesDefeated;
    }
}
