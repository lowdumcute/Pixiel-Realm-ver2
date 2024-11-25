using System.Collections;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField] public int coin = 2; // Số coin nhận được
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private GameObject buttonUpgrade;
    public float detectionRadius = 5f; // Bán kính phát hiện người chơi
    private static bool isInCombat = false; // Trạng thái chiến đấu
    private bool isPlaying = true; // Kiểm tra trạng thái in-game của Miner
    private Transform player;
    private MainHouseController mainHouseController; // Tham chiếu đến nhà chính

    private void Start()
    {
        gamePlayManager = FindObjectOfType<GamePlayManager>();
        mainHouseController = FindObjectOfType<MainHouseController>();
        buttonUpgrade.SetActive(false);

        // Tìm đối tượng có tag "Player" khi khởi động
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player không được tìm thấy trong cảnh!");
        }
    }

    public void Upgrade()
    {
        if (mainHouseController != null && mainHouseController.canUpgrade)
        {
            coin += 2;
            mainHouseController.UpgradeLevel(false); // Nâng cấp nhà chính và vô hiệu hóa khả năng nâng cấp
        }
    }

    public void AddCoin()
    {
        gamePlayManager.AddCoins(coin);
    }

    void Update()
    {
        // Ẩn nút nếu đang trong chiến đấu
        if (isInCombat)
        {
            buttonUpgrade.SetActive(false);
            return;
        }

        if (isPlaying && player != null)
        {
            // Kiểm tra khoảng cách giữa Miner và Player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius && mainHouseController != null)
            {
                // Hiển thị nút nâng cấp nếu chưa đạt cấp tối đa
                buttonUpgrade.SetActive(mainHouseController.canUpgrade);
            }
            else
            {
                buttonUpgrade.SetActive(false);
            }
        }
        else
        {
            buttonUpgrade.SetActive(false);
        }
    }

    public void inGame()
    {
        isPlaying = false;
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
