using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_1 : MonoBehaviour
{
    public GameObject[] Panels; // Mảng chứa các panel
    public Button[] OpenButtons; // Mảng chứa các nút bấm mở panel
    public Button[] CloseButtons; // Mảng chứa các nút bấm đóng panel

    private Animator[] animators; // Mảng chứa Animator của các panel

    private void Start()
    {
        // Lấy Animator từ tất cả các panel
        animators = new Animator[Panels.Length];
        for (int i = 0; i < Panels.Length; i++)
        {
            animators[i] = Panels[i].GetComponent<Animator>();
        }

        // Gán sự kiện mở panel cho từng nút bấm
        for (int i = 0; i < OpenButtons.Length; i++)
        {
            int index = i; // Lưu giá trị `i` để tránh vấn đề delegate capture
            OpenButtons[i].onClick.AddListener(() => OpenPanel(index));
        }

        // Gán sự kiện đóng panel cho từng nút bấm
        for (int i = 0; i < CloseButtons.Length; i++)
        {
            int index = i;
            CloseButtons[i].onClick.AddListener(() => ClosePanel(index));
        }
    }

    private void OpenPanel(int panelIndex)
    {
        
        // Đóng tất cả panel trước
        foreach (Animator animator in animators)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.SetTrigger("Close");
            }
        }

        // Mở panel tương ứng
        if (panelIndex >= 0 && panelIndex < Panels.Length)
        {
            AudioManager.instance.PlaySFX("Button");
            Panels[panelIndex].SetActive(true);
            animators[panelIndex].SetTrigger("Open");
        }
    }

    private void ClosePanel(int panelIndex)
    {
        // Đóng panel tương ứng
        if (panelIndex >= 0 && panelIndex < Panels.Length)
        {
            animators[panelIndex].SetTrigger("Close");

            // Ẩn panel sau khi animation đóng hoàn tất
            StartCoroutine(DisablePanelAfterAnimation(Panels[panelIndex], animators[panelIndex]));
            AudioManager.instance.PlaySFX("Button");
        }
    }

    private IEnumerator DisablePanelAfterAnimation(GameObject panel, Animator animator)
    {
        // Đợi thời gian bằng với độ dài animation "Close"
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Tắt panel
        panel.SetActive(false);
    }
}
