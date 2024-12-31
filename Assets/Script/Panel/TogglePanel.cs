using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TogglePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Panel cần hiển thị/ẩn

    private bool isPanelActive = false;

    private void Start()
    {
        // Đảm bảo panel được ẩn lúc đầu
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Kiểm tra khi nhấn chuột trái
        {
            // Kiểm tra nếu click chuột vào panel
            if (!IsPointerOverUIObject())
            {
                HidePanel();
            }
        }
    }

    public void TogglePanelVisibility()
    {
        if (panel == null) return;

        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);
        AudioManager.instance.PlaySFX("Button");
    }

    public void HidePanel()
    {
        if (panel == null) return;

        isPanelActive = false;
        panel.SetActive(false);
    }

    private bool IsPointerOverUIObject()
    {
        // Kiểm tra nếu con trỏ đang trên một đối tượng UI
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>(); // Sử dụng List thay vì mảng
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == panel)
            {
                return true;
            }
        }

        return false;
    }
}
