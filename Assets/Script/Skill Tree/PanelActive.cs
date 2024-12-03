using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelActive : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Button openPanel;
    [SerializeField] private Button closePanel;
    void Start()
    {
        
        openPanel.onClick.AddListener(Open);
        closePanel.onClick.AddListener(Close);
        Panel.SetActive(false);
    }

    private void Open()
    {

        AudioManager.instance.PlaySFX("Pop");
        Panel.SetActive(true);
    }
    private void Close()
    {

        AudioManager.instance.PlaySFX("Pop");
        Panel.SetActive(false);
    }
    
}
