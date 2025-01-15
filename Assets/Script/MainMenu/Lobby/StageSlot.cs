using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    [SerializeField] private StageSO stageSO;
    [SerializeField] private GameObject[] Panel;

    [SerializeField] private TextMeshProUGUI NameStage;
    [SerializeField] private TextMeshProUGUI Decription;

    [SerializeField] private Button NormalMode;

    private bool isUnlocked; // Biến để lưu trạng thái mở khóa của stage

    private void Start()
    {
        // Ẩn tất cả các panel khi khởi động
        foreach (var panel in Panel)
        {
            panel.SetActive(false);
        }

        // Cập nhật trạng thái mở khóa của stage
        UnlockthisStage();
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện khi stageSO thay đổi
        stageSO.OnDataChange += UnlockthisStage;
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi không cần thiết
        stageSO.OnDataChange -= UnlockthisStage;
    }

    private void UnlockthisStage()
    {
        if (stageSO == null)
        {
            Debug.LogWarning("StageSO is null in UnlockthisStage.");
            return;
        }

        // Lấy trạng thái mở khóa từ StageSO
        isUnlocked = stageSO.isUnlocked;

        // Cập nhật giao diện
        UpdateUI();
    }

    private void UpdateUI()
    {

        if (stageSO != null)
        {
            NameStage.text = stageSO.chapter;
            Decription.text = stageSO.Decription;
        }
    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(i == index);
        }

        if (stageSO != null)
        {
            NameStage.text = stageSO.chapter;
            Decription.text = stageSO.Decription;
        }
    }

    public void LoadNormalStage(int index)
    {
        if (index >= 0 && index < Panel.Length)
        {
            var loadingScreen = Panel[index].GetComponent<LoadingScreen>();
            if (loadingScreen != null)
            {
                loadingScreen.namesence = stageSO.NameSceneNormalStage;
                loadingScreen.LoadingLevel();
            }
        }
    }

    public void LoadChallengeStage(int index)
    {
        if (index >= 0 && index < Panel.Length)
        {
            var loadingScreen = Panel[index].GetComponent<LoadingScreen>();
            if (loadingScreen != null)
            {
                loadingScreen.namesence = stageSO.NameSceneChallengeStage;
                loadingScreen.LoadingLevel();
            }
        }
    }
}
