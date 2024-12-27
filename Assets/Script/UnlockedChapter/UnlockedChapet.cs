using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedChapet : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private GameObject PanelLocked;
    [SerializeField] private Button ButtonInteract;
    [SerializeField] private StageSO[] stage;
    public ChapterSO chapterSO;


    public bool isUnlocked;
    public int StageToUnlocked;
    private void Start()
    {
        CheckUnlocked();
        isUnlocked = chapterSO.isUnlocked;
        UpdateUI();

    }
    private void OnValidate()
    {
        if(chapterSO != null)
        {
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        if(isUnlocked)
        {
            PanelLocked.SetActive(false);
            ButtonInteract.interactable = true;
        }
        else
        {
            PanelLocked.SetActive(true);
            ButtonInteract.interactable = false;
        }
    }
    private void CheckUnlocked()
    {
        for(int i = 0; i< stage.Length;i++)
        {
            if (stage[i].isUnlocked)
            {
                StageToUnlocked++;
            }
        }
        if(StageToUnlocked == stage.Length)
        {
            chapterSO.isUnlocked = true;
            isUnlocked = true;
        }
    }
}
