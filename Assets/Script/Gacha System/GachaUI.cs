using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    
    

    public string RollX1Text;
    public string RollX10Text;
    public TextMeshProUGUI RollX1;
    public TextMeshProUGUI RollX10;

    private void Start()
    {
        
        RollX1.text = RollX1Text;
        RollX10.text = RollX10Text;
    }
}
