using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardGachaManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel; // Panel hiển thị phần thưởng
    [SerializeField] private GameObject slot; // Tham chiếu tới đối tượng cha chứa các object con
    [SerializeField] private GameObject vfx; // vfx itemitem
    [SerializeField] private Button revealButton; // Nút hiển thị từng object
    [SerializeField] private Button fastForwardButton; // Nút tua nhanh

    private Transform[] childObjects; // Mảng lưu các object con
    private int currentIndex = 0; // Chỉ số object con đang được xử lý
    private bool allObjectsRevealed = false; // Kiểm tra xem tất cả các object đã được hiện hay chưa

    public void SetActive()
    {
        // Lấy lại tất cả các object con mỗi khi SetActive được gọi
        childObjects = new Transform[slot.transform.childCount];
        for (int i = 0; i < slot.transform.childCount; i++)
        {
            childObjects[i] = slot.transform.GetChild(i);
            childObjects[i].gameObject.SetActive(false); // Ẩn tất cả các object con

            // Spawn VFX làm con của object con trong slot
            if (vfx != null)
            {
                GameObject spawnedVFX = Instantiate(vfx, childObjects[i].position, Quaternion.identity);
                spawnedVFX.transform.SetParent(childObjects[i]); // Đặt VFX làm con của object con
                spawnedVFX.transform.localPosition = Vector3.zero; // Đảm bảo VFX ở đúng tâm của object con
            }
        }

        // Đảm bảo nút được bật khi bắt đầu
        revealButton.gameObject.SetActive(true);
        fastForwardButton.gameObject.SetActive(true);

        // Reset các trạng thái
        currentIndex = 0; // Reset lại chỉ số
        allObjectsRevealed = false; // Đảm bảo trạng thái ban đầu
    }

    // Hàm hiện từng object con lần lượt
    public void RevealNextObject()
    {
        if (currentIndex < childObjects.Length)
        {
            AudioManager.instance.PlaySFX("OpenChest"); // Phát âm thanh mở hộp
            childObjects[currentIndex].gameObject.SetActive(true); // Hiện object con hiện tại
            currentIndex++; // Tăng chỉ số

            // Nếu đã hiện hết các object con
            if (currentIndex >= childObjects.Length)
            {
                revealButton.gameObject.SetActive(false); // Ẩn nút khi hiện hết
                fastForwardButton.gameObject.SetActive(false); // Ẩn nút khi hiện hết
                allObjectsRevealed = true; // Đánh dấu là đã hiển thị tất cả các object
            }
        }
    }

    // Hàm hiện tất cả object con ngay lập tức
    public void FastForward()
    {
        foreach (Transform child in childObjects)
        {
            AudioManager.instance.PlaySFX("OpenChest");
            child.gameObject.SetActive(true); // Hiện tất cả các object con
        }

        // Ẩn cả hai nút sau khi tua nhanh
        revealButton.gameObject.SetActive(false);
        fastForwardButton.gameObject.SetActive(false);
        allObjectsRevealed = true; // Đánh dấu là đã hiển thị tất cả các object
    }

    private void ClosePanel()
    {
        rewardPanel.SetActive(false); // Ẩn panel sau khi đóng
    }

    // Coroutine để đóng panel sau 5 giây
    private IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Đợi 5 giây trước khi đóng panel
        ClosePanel();
        ClearRewards();
    }

    private void Update()
    {
        // Kiểm tra xem người dùng có nhấn bất kỳ phím nào không, chỉ khi tất cả object đã được hiển thị
        if (allObjectsRevealed && Input.anyKeyDown)
        {
            ClosePanel(); // Ẩn panel khi nhấn bất kỳ phím nào sau khi hiện hết object
            ClearRewards();
        }
    }

    private void ClearRewards()
    {
        // Xóa các object con
        foreach (Transform child in slot.transform)
        {
            Destroy(child.gameObject);
        }
        
        // Sau khi xóa, cập nhật lại danh sách các object con
        SetActive(); // Đảm bảo sau khi ClearRewards được gọi, các object con mới được lấy lại
    }
}
