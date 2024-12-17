using System.Collections.Generic;
using UnityEngine;

// Enum để định nghĩa các loại nhiệm vụ
public enum QuestType
{
    KillEnemies,  // Nhiệm vụ tiêu diệt quái
    StayOnline    // Nhiệm vụ online
}

// Class nhiệm vụ
[System.Serializable]
public class Quest
{
    public string questName;       // Tên nhiệm vụ
    public QuestType questType;    // Loại nhiệm vụ
    public int targetAmount;       // Số lượng mục tiêu (đối với StayOnline là số phút)
    public bool isCompleted;       // Trạng thái hoàn thành
    public int currentAmount;      // Tiến trình hiện tại của nhiệm vụ

    public Quest(string name, QuestType type, int target)
    {
        questName = name;
        questType = type;
        targetAmount = target;
        isCompleted = false;
        currentAmount = 0;
    }
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> questList = new List<Quest>();

    private float elapsedTimeInSeconds = 0; // Biến lưu thời gian online của người chơi (tính theo giây)
    private int minutesElapsed = 0;         // Biến lưu thời gian đã qua tính theo phút

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PrintQuests();
    }

    void Update()
    {
        // Cập nhật thời gian trôi qua mỗi frame
        elapsedTimeInSeconds += Time.deltaTime;

        // Kiểm tra xem đã đủ 60 giây để chuyển sang phút
        if (elapsedTimeInSeconds >= 60)
        {
            minutesElapsed++; // Tăng số phút đã trôi qua
            elapsedTimeInSeconds = 0; // Reset lại thời gian tính theo giây

            // Cập nhật nhiệm vụ StayOnline
            UpdateStayOnlineProgress(minutesElapsed);
        }
    }

    public void PrintQuests()
    {
        foreach (Quest quest in questList)
        {
            Debug.Log($"Tên nhiệm vụ: {quest.questName}, Loại: {quest.questType}, Mục tiêu: {quest.targetAmount}, Hoàn thành: {quest.isCompleted}");
        }
    }

    public void CheckQuestCompletion()
    {
        foreach (Quest quest in questList)
        {
            if (!quest.isCompleted && quest.currentAmount >= quest.targetAmount)
            {
                quest.isCompleted = true;
                Debug.Log($"Nhiệm vụ '{quest.questName}' đã hoàn thành!");
            }
        }
    }

    // Hàm cập nhật tiến độ nhiệm vụ StayOnline theo phút
    public void UpdateStayOnlineProgress(int elapsedMinutes)
    {
        foreach (Quest quest in questList)
        {
            if (quest.questType == QuestType.StayOnline && !quest.isCompleted)
            {
                // Cập nhật tiến độ nhiệm vụ StayOnline (tính theo phút)
                quest.currentAmount = Mathf.Min(elapsedMinutes, quest.targetAmount);
                Debug.Log($"Tiến độ '{quest.questName}': {quest.currentAmount}/{quest.targetAmount} phút");

                // Kiểm tra nếu nhiệm vụ đã hoàn thành
                CheckQuestCompletion();
            }
        }
    }

    public void IncrementKillQuestProgress()
    {
        foreach (Quest quest in questList)
        {
            if (quest.questType == QuestType.KillEnemies && !quest.isCompleted)
            {
                quest.currentAmount++;
                Debug.Log($"Tiến độ '{quest.questName}': {quest.currentAmount}/{quest.targetAmount}");
                CheckQuestCompletion();
            }
        }
    }
}
