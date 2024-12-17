using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operation : MonoBehaviour
{
    public Button OpenButtons; 
    public Button CloseButtons;
    public GameObject ScrollView;
    public GameObject Panel;
    private void Start()
    {
        OpenButtons.onClick.AddListener(OpenPanel);
        CloseButtons.onClick.AddListener(ClosePanel);
    }
    public void OpenPanel()
    {
        StartCoroutine(StartOpen());
    }
    public void ClosePanel()
    {
        StartCoroutine(StartClose());
    }
    IEnumerator StartOpen()
    {
        
        yield return new WaitForSeconds(0.4f);
        ScrollView.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ScrollView.GetComponent<Animator>().SetTrigger("Open");
        
        yield return new WaitForSeconds(0.2f);
        Panel.GetComponent<Animator>().SetTrigger("Open");
    }
    IEnumerator StartClose()
    {
        Panel.GetComponent<Animator>().SetTrigger("Close");            
        yield return new WaitForSeconds(0.15f);
        ScrollView.GetComponent<CanvasGroup>().blocksRaycasts = true;
        ScrollView.GetComponent<Animator>().SetTrigger("Close");
        
    }
}
