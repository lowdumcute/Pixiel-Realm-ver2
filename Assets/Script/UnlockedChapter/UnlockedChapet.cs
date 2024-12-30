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
    }

  
    private void CheckUnlocked()
    {
        StageToUnlocked = 0;
        foreach (StageSO n in stage)
        {
            if(n.isUnlocked == true)
            {
                StageToUnlocked++;
            }
        }
        if(StageToUnlocked == stage.Length)
        {
            chapterSO.isUnlocked = true;
            isUnlocked = true;
            PanelLocked.SetActive(false);
        }
        else
        {
            chapterSO.isUnlocked = false;
            isUnlocked = false;
            PanelLocked.SetActive(true);
        }
    }
}
