using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;           // Text hiển thị coin
    [SerializeField] private MainHouseController mainHouseController; // Tham chiếu tới MainHouseController
    [SerializeField] private int currentCoins = 2;        // Số tiền hiện tại
    [SerializeField] private float shakeDuration = 0.5f;  // Thời gian rung text
    [SerializeField] private float shakeMagnitude = 10f;  // Biên độ rung text

    private Vector3 originalPosition; // Vị trí gốc của text coin

    private void Start()
    {
        originalPosition = coinText.rectTransform.localPosition; // Lưu vị trí ban đầu
        UpdateCoinDisplay();
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
        return currentCoins >= amount; // Kiểm tra nếu đủ tiền
    }

    private void UpdateCoinDisplay()
    {
        coinText.text = currentCoins.ToString();
    }

    public void ShakeCoinText()
    {
        StopAllCoroutines(); // Dừng các coroutine rung hiện tại nếu có
        StartCoroutine(ShakeText());
    }

    private System.Collections.IEnumerator ShakeText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Tạo vị trí ngẫu nhiên trong giới hạn biên độ
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * shakeMagnitude;
            coinText.rectTransform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đưa text về vị trí ban đầu sau khi rung
        coinText.rectTransform.localPosition = originalPosition;
    }
     // Hàm để mở lại UpgradeLevel
    public void EnableUpgradeLevel()
    {
        mainHouseController.canUpgrade= true;  // Mở lại UpgradeLevel
    }
      // Hàm để tạm thời vô hiệu hóa UpgradeLevel
    public void DisableUpgradeLevel()
    {
        mainHouseController.canUpgrade = false; // Vô hiệu hóa UpgradeLevel
    }
}
