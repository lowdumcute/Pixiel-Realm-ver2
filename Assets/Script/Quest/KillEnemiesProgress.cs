using UnityEngine;

public class KillEnemiesProgress : MonoBehaviour
{
    void Start()
    {
        Debug.Log("KillEnemiesProgress Script đã khởi động.");
    }

    // Hàm này sẽ được gọi mỗi khi quái bị tiêu diệt
    public void OnEnemyKilled()
    {
        QuestManager.Instance.IncrementKillQuestProgress();
    }
}
