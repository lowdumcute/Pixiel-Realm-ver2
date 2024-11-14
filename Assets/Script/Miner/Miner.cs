using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    [SerializeField] public int coin = 2;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private GameObject buttonUpgrade;
    public float detectionRadius = 5f; // Bán kính phát hiện người chơi
    private bool isPlaying = true;
    private Transform player; // Không cần gán trong Inspector

    private void Start()
    {
        gamePlayManager = FindObjectOfType<GamePlayManager>();
        buttonUpgrade.SetActive(false);

        // Tìm đối tượng có tag "Player" khi khởi động
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player không được tìm thấy trong cảnh!");
        }
    }

    public void Upgrade()
    {
        coin += 2;
    }

    public void AddCoin()
    {
        gamePlayManager.AddCoins(coin);
    }

    void Update()
    {
        if (isPlaying && player != null)
        {
            // Kiểm tra khoảng cách giữa Miner và Player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRadius)
            {
                buttonUpgrade.SetActive(true);
            }
            else
            {
                buttonUpgrade.SetActive(false);
            }
        }
        else
        {
            buttonUpgrade.SetActive(false);
        }
    }

    public void inGame()
    {
        isPlaying = false;
    }
}
