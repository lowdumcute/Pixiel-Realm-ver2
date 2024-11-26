using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TaiNguyen : MonoBehaviour
{
    //Gán Script để lấy thông số
    public TaiNguyen tainguyen;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI StartText;
    [SerializeField] private TextMeshProUGUI EnergyinText;


    private void Start()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        //gán chữ của tài nguyên
        coinText.text = $"{tainguyen.Gold}";
        StartText.text = $"{tainguyen.Star}";
        EnergyinText.text = $"{tainguyen.Energy}/{tainguyen.MaxEnergy}";
    }
}
