using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUIReward : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rewardPanel; // Panel hiển thị phần thưởng
    public Transform rewardParent; // Container nơi các vật phẩm sẽ được spawn
    public GameObject chestPanel; // Panel của Chest (chứ không phải prefab)
    public GameObject legendaryEffectPrefab; // Prefab hiệu ứng Legendary

    private Coroutine closePanelCoroutine; // Biến để lưu coroutine đóng panel

    private void Start()
    {
        // Đảm bảo panel ẩn khi bắt đầu
        rewardPanel.SetActive(false);
        chestPanel.SetActive(false); // Ẩn chestPanel ngay từ đầu
    }

    // Phương thức gọi từ GachaSystem khi nhận phần thưởng
    public void ShowReward(List<GameObject> rewards)
    {
        // Hiển thị chestPanel (nếu chưa có)
        chestPanel.SetActive(true);

        // Bắt đầu coroutine để phóng to rewardPanel sau 0.5 giây
        StartCoroutine(AnimateRewardPanel());

        // Xóa các vật phẩm cũ (nếu có)
        ClearRewards();

        // Spawn các phần thưởng vào panel từ danh sách rewards
        foreach (var reward in rewards)
        {
            GameObject rewardClone = Instantiate(reward, rewardParent.position, Quaternion.identity, rewardParent);
        }

        // Bắt đầu coroutine để đóng panel sau 5 giây
        if (closePanelCoroutine != null)
        {
            StopCoroutine(closePanelCoroutine); // Dừng coroutine trước nếu đang chạy
        }
        closePanelCoroutine = StartCoroutine(ClosePanelAfterDelay());
    }

    // Coroutine để phóng to rewardPanel
    private IEnumerator AnimateRewardPanel()
    {
        yield return new WaitForSeconds(0.5f); // Đợi 0.5 giây trước khi bắt đầu phóng to

        Vector3 originalScale = rewardPanel.transform.localScale;
        rewardPanel.transform.localScale = Vector3.zero; // Bắt đầu với kích thước nhỏ

        float duration = 0.5f; // Thời gian phóng to
        float elapsed = 0f;

        // Phóng to rewardPanel từ kích thước nhỏ đến kích thước ban đầu
        while (elapsed < duration)
        {
            rewardPanel.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rewardPanel.transform.localScale = originalScale; // Đảm bảo nó có kích thước ban đầu khi hoàn tất
    }

    // Phương thức để đóng panel khi người chơi nhấn bất kỳ nút nào
    private void ClosePanel()
    {
        ClearRewards();
        chestPanel.SetActive(false); // Ẩn chestPanel sau khi đóng
        rewardPanel.SetActive(false); // Ẩn rewardPanel sau khi đóng
    }

    // Phương thức để xóa các phần thưởng clone
    private void ClearRewards()
    {
        foreach (Transform child in rewardParent)
        {
            Destroy(child.gameObject);
        }
    }

    // Coroutine để đóng panel sau 5 giây
    private IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Đợi 5 giây trước khi đóng panel
        ClosePanel();
    }

    private void Update()
    {
        // Kiểm tra xem người dùng có nhấn bất kỳ nút nào không
        if (Input.anyKeyDown)
        {
            ClosePanel();
        }
    }
}
