using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [Header("References")]
    public QuestManager questManager;      // Tham chiếu tới QuestManager
    public GameObject questPrefab;         // Prefab của UI Quest
    public Transform questListParent;      // Parent để chứa các UI Quest

    private List<GameObject> activeQuestUIs = new List<GameObject>();

    void Start()
    {
        PopulateQuests(); // Tạo UI cho tất cả nhiệm vụ khi bắt đầu
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }
    }

    void Update()
    {
        // Cập nhật tiến trình nhiệm vụ liên tục
        UpdateAllQuestUIs();
    }

    // Tạo UI Quest dựa trên danh sách nhiệm vụ
    public void PopulateQuests()
    {
        // Xóa UI cũ
        foreach (GameObject questUI in activeQuestUIs)
        {
            Destroy(questUI);
        }
        activeQuestUIs.Clear();

        // Tạo mới UI cho tất cả nhiệm vụ trong danh sách
        foreach (Quest quest in questManager.questList)
        {
            GameObject newQuestUI = Instantiate(questPrefab, questListParent);
            UpdateQuestUI(newQuestUI, quest);  // Cập nhật thông tin UI cho nhiệm vụ mới
            activeQuestUIs.Add(newQuestUI);
        }
    }

    // Cập nhật toàn bộ UI nhiệm vụ
    private void UpdateAllQuestUIs()
    {
        for (int i = 0; i < activeQuestUIs.Count; i++)
        {
            UpdateQuestUI(activeQuestUIs[i], questManager.questList[i]);
        }
    }

    // Cập nhật UI của từng nhiệm vụ với dữ liệu từ Quest
    public void UpdateQuestUI(GameObject questUI, Quest quest)
    {
        TextMeshProUGUI titleText = questUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = questUI.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        Slider progressSlider = questUI.transform.Find("ProgressSlider").GetComponent<Slider>();

        int currentProgress = quest.currentAmount; // Lấy tiến độ hiện tại của nhiệm vụ
        titleText.text = quest.questName; // Cập nhật tên nhiệm vụ
        amountText.text = $"{currentProgress}/{quest.targetAmount}"; // Cập nhật tiến độ
        progressSlider.maxValue = quest.targetAmount; // Cập nhật giá trị tối đa của slider
        progressSlider.value = currentProgress; // Cập nhật giá trị hiện tại của slider
    }
}
