using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;           // Text hiển thị coin
    [SerializeField] private MainHouseController mainHouseController; // Tham chiếu tới MainHouseController
    [SerializeField] private int currentCoins = 2; // Số tiền hiện tại

    private void Start()
    {
       UpdateCoinDisplay();
    }

    // Cập nhật số coin
    public void UpdateCoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay();
    }

    // Cập nhật UI hiển thị coi

    // Hàm để tạm thời vô hiệu hóa UpgradeLevel
    public void DisableUpgradeLevel()
    {
        mainHouseController.canUpgrade = false; // Vô hiệu hóa UpgradeLevel
    }

    // Hàm để mở lại UpgradeLevel
    public void EnableUpgradeLevel()
    {
        mainHouseController.canUpgrade= true;  // Mở lại UpgradeLevel
    }
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay(); // Cập nhật giao diện sau khi thay đổi số tiền
    }
    public void subtractionCoins(int amount)
    {
        currentCoins -= amount;
        UpdateCoinDisplay(); // Cập nhật giao diện sau khi thay đổi số tiền
    }
    private void UpdateCoinDisplay()
    {
        coinText.text = currentCoins.ToString();
    }
}
