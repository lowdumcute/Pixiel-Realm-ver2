using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUIReward : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rewardPanel; // Panel hiển thị phần thưởng
    public GameObject chestPanel; // Panel hiển thị chest
    public Transform rewardParent; //  container nơi các vật phẩm sẽ được spawn

    private Coroutine closePanelCoroutine; // Biến để lưu coroutine đóng panel
    public GameObject legendaryEffectPrefab;

    private void Start()
    {
        // Đảm bảo panel ẩn khi bắt đầu
        rewardPanel.SetActive(false);
    }

    // Phương thức gọi từ GachaSystem khi nhận phần thưởng
    public void ShowReward(List<GameObject> rewards)
    {
        rewardPanel.SetActive(true); // Hiển thị panel khi có phần thưởng

        // Xóa các vật phẩm cũ (nếu có)
        ClearRewards();

        // Spawn các phần thưởng vào panel từ danh sách rewards
        foreach (var reward in rewards)
        {
            // Spawn vật phẩm vào đúng vị trí của rewardParent và làm rewardParent là parent
            GameObject rewardClone = Instantiate(reward, rewardParent.position, Quaternion.identity, rewardParent);

        }

  
    }

    // Phương thức để xóa các phần thưởng clone
    private void ClearRewards()
    {
        foreach (Transform child in rewardParent)
        {
            Destroy(child.gameObject);
        }
    }

}
