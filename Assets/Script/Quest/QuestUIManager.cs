using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [Header("References")]
    public static QuestUIManager Instance;
    public ScrollRect scrollRect;
    public QuestManager questManager;
    public GameObject questPrefab;
    public Transform rewardContainer;
    
    public Transform questListParent;


    public GameObject rewardEffectPrefab; // Prefab phần thưởng với hiệu ứng
    public List<GameObject> activeQuestUIs = new List<GameObject>();

    void Start()
    {
        PopulateQuests();
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }
    }

    void Update()
    {
        UpdateAllQuestUIs();
    }

    public void PopulateQuests()
    {
        foreach (GameObject questUI in activeQuestUIs)
        {
            if (questUI != null)
            {
                Destroy(questUI);
            }
        }
        activeQuestUIs.Clear();

        foreach (Quest quest in questManager.questList)
        {
            GameObject newQuestUI = Instantiate(questPrefab, questListParent);
            UpdateQuestUI(newQuestUI, quest);
            activeQuestUIs.Add(newQuestUI);
        }
    }

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

    public void UpdateQuestUI(GameObject questUI, Quest quest)
    {
        if (questUI == null) return;

        TextMeshProUGUI titleText = questUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = questUI.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        Slider progressSlider = questUI.transform.Find("ProgressSlider").GetComponent<Slider>();
        Button buttonClaimReward = questUI.transform.Find("ClaimRewardButton").GetComponent<Button>();
        CanvasGroup canvasGroup = questUI.GetComponent<CanvasGroup>();

        buttonClaimReward.onClick.RemoveAllListeners();

        if (quest.isCompleted)
        {
            buttonClaimReward.gameObject.SetActive(true);
            buttonClaimReward.onClick.AddListener(() => {
                questManager.ClaimQuestReward(quest.questIndex);

                // Cập nhật RewardText
                TextMeshProUGUI RewardText = questUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (RewardText != null)
                {
                    RewardText.text = $" +{quest.rewardAmount}";
                }

                // Spawn phần thưởng với hiệu ứng
                SpawnRewardEffect(buttonClaimReward.transform, quest.rewardAmount);

                // Làm mờ nhiệm vụ sau khi nhận phần thưởng
                FadeOutQuest(questUI, canvasGroup);
                MoveQuestToBottom(questUI); // Move to bottom after claiming
            });

            // Đẩy nhiệm vụ hoàn thành lên đầu
            MoveQuestToTop(questUI);
        }
        else
        {
            buttonClaimReward.gameObject.SetActive(false);
        }

        // Nếu nhiệm vụ đã được nhận phần thưởng, đẩy xuống dưới và làm mờ
        if (quest.isClaimed)
        {
            MoveQuestToBottom(questUI);
            FadeOutQuest(questUI, canvasGroup);
        }

        titleText.text = quest.questName;
        amountText.text = $"{quest.currentAmount}/{quest.targetAmount}";
        progressSlider.maxValue = quest.targetAmount;
        progressSlider.value = quest.currentAmount;
    }

    // Đẩy nhiệm vụ lên đầu danh sách
    private void MoveQuestToTop(GameObject questUI)
    {
        questUI.transform.SetAsFirstSibling(); // Đặt làm đầu tiên trong danh sách
    }

    // Đẩy nhiệm vụ xuống dưới cùng danh sách
    private void MoveQuestToBottom(GameObject questUI)
    {
        questUI.transform.SetAsLastSibling(); // Đặt làm cuối cùng trong danh sách
    }


    private void FadeOutQuest(GameObject questUI, CanvasGroup canvasGroup)
    {
        if (canvasGroup == null)
            canvasGroup = questUI.AddComponent<CanvasGroup>();

        StartCoroutine(FadeOut(canvasGroup));
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float fadeDuration = 1f;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    // Spawn phần thưởng với hiệu ứng bay lên
    private void SpawnRewardEffect(Transform buttonTransform, int rewardAmount)
    {
        // Tạo phần thưởng tại vị trí của buttonClaimReward
        GameObject rewardEffect = Instantiate(rewardEffectPrefab, rewardContainer);

        // Đặt vị trí của rewardEffect trùng với buttonClaimReward
        rewardEffect.transform.position = buttonTransform.position;

        // Cập nhật text hiển thị
        TextMeshPro rewardText = rewardEffect.transform.GetChild(0).GetComponent<TextMeshPro>();
        if (rewardText != null)
        {
            rewardText.text = $"+{rewardAmount}";
        }

        // Thực hiện hiệu ứng bay lên và mờ dần
        StartCoroutine(RewardFlyUpAndFade(rewardEffect));
    }

    private IEnumerator RewardFlyUpAndFade(GameObject rewardEffect)
    {
        float duration = 2.5f; // Tổng thời gian hiệu ứng
        float fadeDuration = 1.5f; // Thời gian mờ dần
        Vector3 startPosition = rewardEffect.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 10f; // Bay lên 30 đơn vị

        float elapsedTime = 0f;
        CanvasGroup canvasGroup = rewardEffect.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = rewardEffect.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < duration)
        {
            // Di chuyển phần thưởng bay lên
            rewardEffect.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            // Làm mờ phần thưởng
            if (elapsedTime >= duration - fadeDuration)
            {
                float fadeElapsed = elapsedTime - (duration - fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo phần thưởng bị xóa sau hiệu ứng
        Destroy(rewardEffect);
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
