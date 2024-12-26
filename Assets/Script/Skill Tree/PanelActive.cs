using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelActive : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    private void Awake()
    {
    }
    void Start()
    {
        Panel.SetActive(false);
    }

    public  void Open()
    {
            AudioManager.instance.PlaySFX("Button");
            Panel.SetActive(true);
    }
    public void Close()
    {
            AudioManager.instance.PlaySFX("Button");
            Panel.SetActive(false);
    }
    
}
