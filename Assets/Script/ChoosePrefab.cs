using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject panel;                // UI Panel cho đối tượng này
    public GameObject prefabToSpawn;        // Prefab cụ thể cho đối tượng này
    public float detectionRadius = 5f;      // Bán kính phát hiện người chơi
    public Transform player;                // Tham chiếu đến người chơi
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private int coin = 2;

    private bool hasSpawned = false;        // Biến kiểm tra việc spawn của đối tượng này
    private static bool isInCombat = false; // Biến toàn cục để kiểm soát trạng thái chiến đấu

    private void Start()
    {
        // Đảm bảo panel ban đầu ẩn đi
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        // Ẩn panel nếu đã spawn hoặc đang trong chế độ chiến đấu
        if (hasSpawned || isInCombat)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
            return;
        }

        // Kiểm tra khoảng cách với người chơi
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            // Hiển thị panel nếu người chơi trong phạm vi, ngược lại thì ẩn đi
            if (panel != null)
            {
                panel.SetActive(distance <= detectionRadius);
            }
        }
    }

    public void OnSpawnButtonClicked()
    {
        // Kiểm tra nếu đủ tiền mới thực hiện
        if (gamePlayManager.CanAfford(coin))
        {
            // Chỉ thực hiện nếu prefab chưa được spawn
            if (prefabToSpawn != null && !hasSpawned)
            {
                gamePlayManager.subtractionCoins(coin);
                Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                hasSpawned = true;

                // Ẩn panel sau khi spawn
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
        }
        else
        {
            // Rung text nếu không đủ tiền
            gamePlayManager.ShakeCoinText();
        }
    }

    public static void EnterCombat()
    {
        isInCombat = true;
    }

    public static void ExitCombat()
    {
        isInCombat = false;
    }
}
