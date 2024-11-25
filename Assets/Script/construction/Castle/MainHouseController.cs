using UnityEngine;
using TMPro;

public class MainHouseController : MonoBehaviour
{
    [System.Serializable]
    public class LevelObjects
    {
        public GameObject[] objects; // Các object của mỗi level
    }

    [SerializeField] private GameObject UpgradePanel; // Panel nâng cấp
    [SerializeField] private GameObject LevelObj;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private int coin = 2;
    public LevelObjects[] levels; // Mảng chứa các cấp độ và các object liên quan
    public float detectionRadius = 5f; // Bán kính phát hiện người chơi
    private static bool isInCombat = false; // Trạng thái chiến đấu
    private Transform player; // Vị trí của người chơi
    private int currentLevel = 0; // Cấp độ hiện tại
    public bool canUpgrade = true; // Có thể nâng cấp hay không

    private void Start()
    {
        LevelText.text = "Level";
        // Tìm đối tượng có tag "Player"
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng Player trong cảnh!");
        }

        // Khởi tạo hiển thị nút nâng cấp và cập nhật các đối tượng cấp độ
        UpgradePanel.SetActive(false);
        LevelObj.SetActive(false);
        UpdateLevelVisibility();
        UpdateLevelText();
    }

    private void Update()
    {
        // Kiểm tra nếu đang trong trạng thái chiến đấu thì luôn ẩn nút nâng cấp
        if (isInCombat)
        {
            UpgradePanel.SetActive(false);
            LevelObj.SetActive(false);
            return;
        }

        // Kiểm tra khoảng cách giữa người chơi và nhà chính
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Hiển thị nút nâng cấp khi người chơi ở trong bán kính và nhà chính có thể nâng cấp
            if (distanceToPlayer <= detectionRadius && canUpgrade)
            {
                UpgradePanel.SetActive(true);
                LevelObj.SetActive(true);
            }
            else
            {
                UpgradePanel.SetActive(false);
                LevelObj.SetActive(false);
            }
        }
        else
        {
            UpgradePanel.SetActive(false);
            LevelObj.SetActive(false);
        }
    }

    public void UpgradeLevel(bool enable)
    {
        if (currentLevel < levels.Length - 1)
        {
            if (gamePlayManager.CanAfford(coin))
            {
                gamePlayManager.subtractionCoins(coin);
                currentLevel++;
                canUpgrade = enable && currentLevel < levels.Length - 1; // Kiểm tra cấp tối đa
                UpdateLevelVisibility();
                Debug.Log("Đã nâng cấp lên cấp độ: " + currentLevel);
                UpdateLevelText();
            }
            else
            {
                // Rung text nếu không đủ tiền
                gamePlayManager.ShakeCoinText();
            }
        }
        else
        {
            canUpgrade = false; // Không thể nâng cấp nữa
            Debug.Log("Đã đạt cấp độ tối đa.");
        }
    }
    private void UpdateLevelText()
    {
        LevelText.text = $"Level {currentLevel +1}";
    }
    private void UpdateLevelVisibility()
    {
        // Hiển thị các object của cấp độ hiện tại và thấp hơn
        for (int i = 0; i <= currentLevel; i++)
        {
            foreach (GameObject obj in levels[i].objects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }

        // Ẩn các object của cấp độ cao hơn
        for (int i = currentLevel + 1; i < levels.Length; i++)
        {
            foreach (GameObject obj in levels[i].objects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    // Bật trạng thái chiến đấu
    public static void EnterCombat()
    {
        isInCombat = true;
    }

    // Tắt trạng thái chiến đấu
    public static void ExitCombat()
    {
        isInCombat = false;
    }
}
