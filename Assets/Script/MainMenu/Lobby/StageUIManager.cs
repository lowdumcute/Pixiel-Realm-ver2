using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Để truy cập các component UI như Button

public class StageUIManager : MonoBehaviour
{
    [SerializeField] public StageSO currentStageSO; // Biến tham chiếu đến StageSO
    public GameObject imageLocked;       // Tham chiếu đến hình ảnh khóa
    public Button button;                // Tham chiếu đến component Button


    private void OnEnable()
    {
        // Lắng nghe sự kiện thay đổi dữ liệu của StageSO
        if (currentStageSO != null)
        {
            currentStageSO.OnDataChange += UpdateUI;
        }
        UpdateUI(); // Cập nhật giao diện lần đầu
    }

    private void OnDisable()
    {
        // Gỡ bỏ lắng nghe sự kiện khi script bị vô hiệu hóa
        if (currentStageSO != null)
        {
            currentStageSO.OnDataChange -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (currentStageSO == null) return;

        // Kiểm tra trạng thái isUnlocked để cập nhật UI
        if (currentStageSO.isUnlocked)
        {
            imageLocked.SetActive(false);
            if (button != null)
                button.interactable = true; // Kích hoạt nút
        }
        else
        {
            imageLocked.SetActive(true);
            if (button != null)
                button.interactable = false; // Vô hiệu hóa nút
        }
    }
}
