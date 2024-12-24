using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaUI : MonoBehaviour
{
    public Asset asset;

    public string X1;
    public string X10;
    public TextMeshProUGUI RollOnce;
    public TextMeshProUGUI RollTen;

    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    
    public void UpdateUI()
    {
        if (asset.Star < 1599 && asset.Star > 159)
        {
            RollOnce.text = $"<color=white>{X1} </color>";
            RollTen.text = $"<color=red>{X10} </color>";
        }
        else if (asset.Star > 1599 && asset.Star > 159) 
        {
            RollOnce.text = $"<color=white>{X1} </color>";
            RollTen.text = $"<color=white>{X10} </color>";
        }
        else
        {
            RollOnce.text = $"<color=red>{X1} </color>";
            RollTen.text = $"<color=red>{X10} </color>";
        }
    }
}
