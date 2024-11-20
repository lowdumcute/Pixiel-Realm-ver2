using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Element : MonoBehaviour
{
    public static Element insntance;
    [SerializeField]  private TMP_Text goldText;
    [SerializeField]  private TMP_Text starText;

    public static float Gold =0;
    public static float Star=0;
    public void UpdateElement()
    {
        goldText.text = $"Gold: {Gold} ";
        starText.text = $"Star: {Star} ";
    }
}
