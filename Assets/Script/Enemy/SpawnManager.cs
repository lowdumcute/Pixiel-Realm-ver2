using System.Collections;
using UnityEngine;
using TMPro; // Thêm thư viện TMP

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GamePlayManager gamePlayManager; // Tham chiếu đến GamePlayManager
    [SerializeField] private CastleHealth CastleHealth;
    [SerializeField] private TMP_Text waveText; // TMP_Text để hiển thị thông tin wave
    public GameObject enemyPrefab; // Prefab của quái
    public Transform[] spawnPoints; // Các điểm spawn quái
    public int[] enemiesPerWave; // Số lượng quái theo mỗi Wave
    public float spawnInterval = 1f; // Khoảng thời gian giữa các lần spawn
    public GameObject button; // Nút spawn quái
    public int[] coinsPerWave; // Số tiền nhận được cho mỗi Wave

    private int currentWave = 0; // Wave hiện tại
    private int enemiesSpawned; // Số quái đã spawn trong level hiện tại
    private int enemiesDefeated; // Số quái đã bị tiêu diệt

    void Start()
    {
        button.SetActive(true); // Đảm bảo nút hiện khi bắt đầu
        UpdateWaveText(); // Hiển thị số wave ban đầu
    }

    // Phương thức để cập nhật hiển thị số wave
    private void UpdateWaveText()
    {
        int totalWaves = enemiesPerWave.Length;
        waveText.text = $"Wave: {currentWave + 1}/{totalWaves}";
    }

    // Phương thức để bắt đầu spawn quái cho level hiện tại khi nhấn nút
    public void SpawnCurrentLevel()
    {
        CastleHealth.RestoreHealth();
        gamePlayManager.DisableUpgradeLevel(); // Vô hiệu hóa nâng cấp
        if (currentWave < enemiesPerWave.Length)
        {
            Miner[] miners = FindObjectsOfType<Miner>();
            foreach (Miner miner in miners)
            {
                miner.inGame();
            }
            button.SetActive(false); // Ẩn nút sau khi nhấn
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

    // Phương thức này được gọi khi một quái bị tiêu diệt
    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        CastleHealth.OnEnemyDefeated();

        // Khi tiêu diệt hết quái trong level hiện tại
        if (enemiesDefeated >= enemiesPerWave[currentWave])
        {
            // Cộng tiền theo cấp độ hiện tại
            gamePlayManager.AddCoins(coinsPerWave[currentWave]);

            // Gọi AddCoin cho tất cả các Miner trong cảnh
            Miner[] miners = FindObjectsOfType<Miner>();
            foreach (Miner miner in miners)
            {
                miner.AddCoin();
            }

            // Gọi ResetUnitsPositions cho tất cả các Barracks trong cảnh
            Barracks[] barracks = FindObjectsOfType<Barracks>();
            foreach (Barracks barracksInstance in barracks)
            {
                barracksInstance.ResetUnitsPositions();
            }
            // Tìm tất cả các đối tượng TowerHealth
            TowerHealth[] towers = FindObjectsOfType<TowerHealth>();

            // Duyệt qua tất cả và khôi phục máu
            foreach (TowerHealth towerInstance in towers)
            {
                towerInstance.RestoreHealth();
            }

            // Tăng cấp độ và hiển thị lại nút để spawn cho level tiếp theo
            currentWave++;
            UpdateWaveText(); // Cập nhật hiển thị số wave
            button.SetActive(true);

            // Mở lại khả năng nâng cấp
            gamePlayManager.EnableUpgradeLevel();
        }
    }
}
