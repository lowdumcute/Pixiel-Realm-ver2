using TMPro;
using UnityEngine;

public class AssetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text starText;
    [SerializeField] private TMP_Text energyText;

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (AssetManager.instance != null)
        {
            Asset assetData = AssetManager.instance.GetAssetData();

            if (goldText != null)
                goldText.text = $"{assetData.Gold}";

            if (starText != null)
                starText.text = $"{assetData.Star}";

            if (energyText != null)
                energyText.text = $"{assetData.currentEnergy} / {assetData.maxEnergy}";
        }
        else
        {
            Debug.LogError("AssetManager instance is not available!");
        }
    }
}
