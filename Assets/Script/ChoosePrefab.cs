using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class IndependentSpawner : MonoBehaviour
{
    public GameObject panel;                // UI Panel cho đối tượng này
    public GameObject prefabToSpawn;        // Prefab cụ thể cho đối tượng này
    public float detectionRadius = 5f;      // Bán kính phát hiện người chơi
    public Transform player;                // Tham chiếu đến người chơi
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] int coin = 2;

    private bool hasSpawned = false;        // Biến riêng biệt để kiểm tra việc spawn của đối tượng này

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
        // Nếu đã spawn prefab, ẩn panel và thoát sớm
        if (hasSpawned)
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

    // Hàm gọi khi nhấn nút Spawn trong panel
    public void OnSpawnButtonClicked()
    {
        // Chỉ thực hiện nếu prefab chưa được spawn
        if (prefabToSpawn != null && !hasSpawned)
        {
            gamePlayManager.subtractionCoins(coin);
            // Spawn prefab tại vị trí của đối tượng hiện tại
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            
            // Đánh dấu là đã spawn và ẩn panel
            hasSpawned = true;
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    // Phương thức để thay đổi prefab muốn spawn
    public void SetPrefabToSpawn(GameObject newPrefab)
    {
        prefabToSpawn = newPrefab;
    }
}
