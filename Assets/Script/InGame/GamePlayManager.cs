using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private MainHouseController mainHouseController;
    [SerializeField] private int currentCoins = 2;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private GameObject winPanel; // Tham chiếu tới Win Panel

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = coinText.rectTransform.localPosition;
        UpdateCoinDisplay();
        winPanel.SetActive(false); // Ẩn Win Panel khi bắt đầu game
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
    }
}
