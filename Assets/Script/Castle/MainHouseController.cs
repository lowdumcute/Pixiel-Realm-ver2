using UnityEngine;

public class MainHouseController : MonoBehaviour
{
    [System.Serializable]
    public class LevelObjects
    {
        public GameObject[] objects; // Các object của mỗi level
    }

    public LevelObjects[] levels; // Mảng chứa các cấp độ và các object liên quan
    private int currentLevel = 0; // Cấp độ hiện tại
    public bool canUpgrade = true; 

    private void Start()
    {
        // Khởi tạo để chỉ hiển thị các đối tượng của level đầu tiên
        UpdateLevelVisibility();
    }

    private void Update()
    {
        // Kiểm tra khi người chơi nhấn phím Space để nâng cấp cấp độ
        if (Input.GetKeyDown(KeyCode.Space) && canUpgrade)
        {
            UpgradeLevel(true); // Gọi hàm nâng cấp level khi nhấn Space
        }
    }

    // Hàm để tăng cấp độ và hiển thị các đối tượng của cấp độ mới
    public void UpgradeLevel(bool enable)
    {
        if (currentLevel < levels.Length - 1)
        {
            canUpgrade = enable;
            currentLevel++;
            UpdateLevelVisibility();
            Debug.Log("Đã nâng cấp lên cấp độ: " + currentLevel);
        }
        else
        {
            Debug.Log("Đã đạt cấp độ tối đa.");
        }
    }

    // Cập nhật hiển thị cho các object theo cấp độ hiện tại
    private void UpdateLevelVisibility()
    {
        // Hiển thị các object của các cấp độ thấp hơn
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
}
