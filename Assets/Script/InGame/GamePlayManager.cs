using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class LevelReward
{
    public GameObject[] itemReward; // Các Item phần thưởng
    public int[] rewardQuantities; // Số lượng tương ứng của từng itemReward
}

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private MainHouseController mainHouseController;
    [SerializeField] private CastleHealth castleHealth; 
    [SerializeField] private int currentCoins = 2;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private GameObject winPanel; // Tham chiếu tới Win Panel
    [SerializeField] private GameObject losePanel; // Tham chiếu tới Lose Panel
    [SerializeField] private LevelReward[] levelRewards; // Danh sách phần thưởng

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = coinText.rectTransform.localPosition;
        UpdateCoinDisplay();
        winPanel.SetActive(false); // Ẩn Win Panel khi bắt đầu game
        losePanel.SetActive(false); // Ẩn Win Panel khi bắt đầu game
        AudioManager.instance.PlayMusic("IdleGame");
    }

    public void UpdateCoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay();
    }

    public void subtractionCoins(int amount)
    {
        currentCoins -= amount;
        UpdateCoinDisplay();
    }

    public bool CanAfford(int amount)
    {
        return currentCoins >= amount;
    }

    private void UpdateCoinDisplay()
    {
        coinText.text = currentCoins.ToString();
    }

    public void ShakeCoinText()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeText());
    }

    private System.Collections.IEnumerator ShakeText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * shakeMagnitude;
            coinText.rectTransform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coinText.rectTransform.localPosition = originalPosition;
    }

    public void EnableUpgradeLevel()
    {
        mainHouseController.canUpgrade = true;
    }

    public void DisableUpgradeLevel()
    {
        mainHouseController.canUpgrade = false;
    }



    // Phương thức Win
    public void Win()
    {
        Debug.Log("You Win!");
        winPanel.SetActive(true); // Hiển thị Win Panel
        Time.timeScale = 0; // Dừng thời gian trong game
        AudioManager.instance.PlaySFX("Victory");
        UpdateRewards();
    }

    private void UpdateRewards()
    {
        foreach (LevelReward reward in levelRewards)
        {
            for (int i = 0; i < reward.itemReward.Length; i++)
            {
                GameObject rewardObject = reward.itemReward[i];
                int quantity = reward.rewardQuantities[i];

                if (rewardObject != null)
                {
                    Item itemComponent = rewardObject.GetComponent<Item>();
                    if (itemComponent != null)
                    {
                        itemComponent.UpdateQuantity(quantity); // Cập nhật số lượng vào Item
                    }
                    else
                    {
                        Debug.LogWarning($"Item component is missing on {rewardObject.name}");
                    }
                }
            }
        }
    }
    public void ChangeSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        AudioManager.instance.PlayMusic("MainMenuTheme");
        Time.timeScale = 1; 
    }
    public void Lose()
    {
        Debug.Log("You Lose!");
        losePanel.SetActive(true); // Hiển thị Lose Panel
        Time.timeScale = 0; // Dừng thời gian trong game
    }
}
