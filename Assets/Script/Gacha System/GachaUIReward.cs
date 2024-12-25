using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaUIReward : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rewardPanel; // Panel hiển thị phần thưởng
    public Transform rewardParent; //  container nơi các vật phẩm sẽ được spawn

    private Coroutine closePanelCoroutine; // Biến để lưu coroutine đóng panel

    private void Start()
    {
        // Đảm bảo panel ẩn khi bắt đầu
        rewardPanel.SetActive(false);
    }

    // Phương thức gọi từ GachaSystem khi nhận phần thưởng
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

            // Nếu bạn muốn gắn thêm thông tin vào rewardClone, có thể thực hiện ở đây
            // Ví dụ: rewardClone.GetComponent<SomeComponent>().SetData(reward);
        }

        // Bắt đầu coroutine để đóng panel sau 5 giây
        if (closePanelCoroutine != null)
        {
            StopCoroutine(closePanelCoroutine); // Dừng coroutine trước nếu đang chạy
        }
        closePanelCoroutine = StartCoroutine(ClosePanelAfterDelay());
    }

    // Phương thức để đóng panel khi người chơi nhấn bất kỳ nút nào
    private void ClosePanel()
    {
        ClearRewards();
        rewardPanel.SetActive(false); // Ẩn panel sau khi đóng
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
