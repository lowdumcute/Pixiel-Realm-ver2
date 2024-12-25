using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    [SerializeField] private StageSO stageSO;
    [SerializeField] private GameObject[] Panel;

    [SerializeField] private CanvasGroup Stage;

    [SerializeField] private TextMeshProUGUI NameStage;
    [SerializeField] private TextMeshProUGUI Decription;
    [SerializeField]
    private Button Click;

    [SerializeField] private Button NormalMode;
    [SerializeField] private Button ChallengeMode;

    public bool isUnlocked;
    private void Start()
    {
        UnlockthisStage();
    }
    private void OnEnable()
    {
        stageSO.OnDataChange += UnlockthisStage;
    }
    private void OnDisable()
    {
        stageSO.OnDataChange -= UnlockthisStage;
    }
    private void UnlockthisStage()
    {
        isUnlocked = stageSO.isUnlocked;
        UpdateUI();
    }
    private void OnValidate()
    {
        if (stageSO != null)
        {
            Debug.Log($"OnValidate called. isUnlocked: {stageSO.isUnlocked}");
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("StageSO is null in OnValidate.");
        }
    }
    
    
    private void UpdateUI()
    {     
        
            if (isUnlocked == false)
            {
                Stage.GetComponent<CanvasGroup>().alpha = 0f;
                Stage.GetComponent<CanvasGroup>().interactable = false;
                Stage.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                Stage.GetComponent<CanvasGroup>().alpha = 1f;
                Stage.GetComponent<CanvasGroup>().interactable = true;
                Stage.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        

    }


    public void OpenPanel(int index)
    {
        for(int i = 0; i < Panel.Length;i++)
        {
            if(i != index)
            {
                Panel[i].SetActive(false);
                
                
            }
            else
            {
                Panel[i].SetActive(true);
                Panel[i].GetComponent<Animator>().SetTrigger("Open");
            }
                
        }
        if (stageSO != null)
        {
            NameStage.text = stageSO.chapter;
            Decription.text = stageSO.Decription;
        }
    }
   public void LoadNormalStage(int index)
    {
        Panel[index].GetComponent<LoadingScreen>().namesence = stageSO.NameSceneNormalStage;
        Panel[index].GetComponent<LoadingScreen>().LoadingLevel();

    }
    public void LoadChallengeStage(int index)
    {
        Panel[index].GetComponent<LoadingScreen>().namesence = stageSO.NameSceneChallengeStage;
        Panel[index].GetComponent<LoadingScreen>().LoadingLevel();

    }
    // Update is called once per frame

}
