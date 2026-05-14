using System.Collections;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField] public int coin = 2; // Số coin nhận được

    [SerializeField] private GameObject buttonUpgrade;
    [SerializeField] private GameObject Light;
    public float detectionRadius = 5f; // Bán kính phát hiện người chơi
    private static bool isInCombat = false; // Trạng thái chiến đấu
    private bool isPlaying = true; // Kiểm tra trạng thái in-game của Miner
    private Transform player;

    private void Start()
    {
        buttonUpgrade.SetActive(false);
        Light.SetActive (false);

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
        if (MainHouseController.instance != null && MainHouseController.instance.canUpgrade)
        {
            coin += 2;
            MainHouseController.instance.UpgradeLevel(false); // Nâng cấp nhà chính và vô hiệu hóa khả năng nâng cấp
        }
    }

    public void AddCoin()
    {
        GamePlayManager.instance.AddCoins(coin);
        Light.SetActive (false);
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

            if (distanceToPlayer <= detectionRadius && MainHouseController.instance != null)
            {
                // Hiển thị nút nâng cấp nếu chưa đạt cấp tối đa
                buttonUpgrade.SetActive(MainHouseController.instance.canUpgrade);
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
        Light.SetActive (true);
        
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
