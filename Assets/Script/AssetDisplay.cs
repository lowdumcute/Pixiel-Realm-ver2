using TMPro;
using UnityEngine;

public class AssetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText; // Tham chiếu đến UI hiển thị Gold
    [SerializeField] private TMP_Text starText; // Tham chiếu đến UI hiển thị Star

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay(); // Cập nhật UI khi script được kích hoạt
    }

    public void UpdateDisplay()
    {
        if (Asset.instance != null)
        {
            if (goldText != null)
                goldText.text = $"{Asset.instance.GetGold()}";
            else
                Debug.LogError("GoldText is not assigned!");

            if (starText != null)
                starText.text = $"{Asset.instance.GetStar()}";
            else
                Debug.LogError("StarText is not assigned!");
        }
    }
}