using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestUIManager : MonoBehaviour
{
    [Header("References")]
    public static QuestUIManager Instance;
    public ScrollRect scrollRect;  // Tham chiếu tới ScrollRect
    public QuestManager questManager;      // Tham chiếu tới QuestManager
    public GameObject questPrefab;         // Prefab của UI Quest
    public Transform questListParent;      // Parent để chứa các UI Quest

    public List<GameObject> activeQuestUIs = new List<GameObject>();

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
        UpdateAllQuestUIs();
    }

    // Tạo UI Quest dựa trên danh sách nhiệm vụ
    public void PopulateQuests()
    {
        // Xóa UI cũ
        foreach (GameObject questUI in activeQuestUIs)
        {
            if (questUI != null)
            {
                Destroy(questUI);
            }
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
    public void UpdateAllQuestUIs()
    {
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }
        for (int i = 0; i < activeQuestUIs.Count; i++)
        {
            if (activeQuestUIs[i] != null)
            {
                UpdateQuestUI(activeQuestUIs[i], questManager.questList[i]);
            }
        }
    }

    // Cập nhật UI của từng nhiệm vụ với dữ liệu từ Quest
    public void UpdateQuestUI(GameObject questUI, Quest quest)
    {
        if (questUI == null) return; // Kiểm tra questUI có null hay không

        TextMeshProUGUI titleText = questUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = questUI.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        Slider progressSlider = questUI.transform.Find("ProgressSlider").GetComponent<Slider>();
        Button buttonClaimReward = questUI.transform.Find("ClaimRewardButton").GetComponent<Button>();
        CanvasGroup canvasGroup = questUI.GetComponent<CanvasGroup>(); // CanvasGroup để điều khiển độ mờ

        buttonClaimReward.onClick.RemoveAllListeners(); // Xóa tất cả các sự kiện trước khi thêm mới

        // Nếu nhiệm vụ đã hoàn thành, kích hoạt nút Claim Reward, nếu không thì vô hiệu hóa nút
        if (quest.isCompleted)
        {
            amountText.gameObject.SetActive(false);
            buttonClaimReward.gameObject.SetActive(true); // Kích hoạt nút
            buttonClaimReward.onClick.AddListener(() => {
                questManager.ClaimQuestReward(quest.questIndex); // Gán sự kiện nhận thưởng
                // Làm mờ nhiệm vụ sau khi nhận phần thưởng
                FadeOutQuest(questUI, canvasGroup); 
                MoveQuestToBottom(questUI); // Di chuyển nhiệm vụ xuống dưới cùng
            });
        }
        else
        {
            buttonClaimReward.gameObject.SetActive(false); // Vô hiệu hóa nút
        }

        titleText.text = quest.questName; // Cập nhật tên nhiệm vụ
        amountText.text = $"{quest.currentAmount}/{quest.targetAmount}"; // Cập nhật tiến độ
        progressSlider.maxValue = quest.targetAmount; // Cập nhật giá trị tối đa của slider
        progressSlider.value = quest.currentAmount; // Cập nhật giá trị hiện tại của slider
    }

    // Phương thức làm mờ nhiệm vụ (fade out)
    private void FadeOutQuest(GameObject questUI, CanvasGroup canvasGroup)
    {
        if (canvasGroup == null)
            canvasGroup = questUI.AddComponent<CanvasGroup>(); // Nếu chưa có, thêm CanvasGroup để điều khiển độ mờ

        // Dùng tween hoặc animate để làm mờ dần
        StartCoroutine(FadeOut(canvasGroup)); 
    }

    // Coroutine để làm mờ nhiệm vụ
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float fadeDuration = 1f; // Thời gian fade
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha; // Đảm bảo hoàn thành quá trình fade
    }

    // Phương thức để di chuyển nhiệm vụ xuống dưới cùng
    private void MoveQuestToBottom(GameObject questUI)
    {
        questUI.transform.SetAsLastSibling(); // Đưa nhiệm vụ xuống dưới cùng của danh sách
    }

    // Phương thức chỉ cập nhật amountText và progressSlider
    public void UpdateQuestProgressUI(GameObject questUI, int currentAmount, int targetAmount)
    {
        if (questUI == null) return; // Kiểm tra questUI có null hay không

        TextMeshProUGUI amountText = questUI.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        Slider progressSlider = questUI.transform.Find("ProgressSlider").GetComponent<Slider>();

        amountText.text = $"{currentAmount}/{targetAmount}"; // Cập nhật tiến độ
        progressSlider.maxValue = targetAmount; // Cập nhật giá trị tối đa của slider
        progressSlider.value = currentAmount; // Cập nhật giá trị hiện tại của slider
    }
}
