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

    public StageSO StageSO;


    public bool isUnlocked;
    public int StageToUnlocked;
    private void Start()
    {
        CheckUnlocked();
    }

  
    private void CheckUnlocked()
    {
       
        if(StageSO.isUnlocked == true)
        {
            isUnlocked = true;
            PanelLocked.SetActive(false);
        }
        else
        {
            isUnlocked = false;
            PanelLocked.SetActive(true);
        }
    }
}
