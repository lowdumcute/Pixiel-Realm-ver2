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
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI NameStage;
    [SerializeField] private TextMeshProUGUI Decription;
    [SerializeField]
    private Button Click;

    [SerializeField] private Button NormalMode;
    [SerializeField] private Button ChallengeMode;


   

    void Start()
    {

        Click.onClick.AddListener(UpdateUI);
        NormalMode.onClick.AddListener(LoadNormalStage);
        ChallengeMode.onClick.AddListener(LoadChallengeStage);
    }
    void UpdateUI()
    {   
        Panel.GetComponent<Animator>().SetTrigger("Open");
        if (stageSO != null)
        {
            NameStage.text = stageSO.chapter;
            Decription.text = stageSO.Decription;
        }
    }
   private void LoadNormalStage()
    {
        Panel.GetComponent<LoadingScreen>().namesence = stageSO.NameSceneNormalStage;
        Panel.GetComponent<LoadingScreen>().LoadingLevel();

    }
    private void LoadChallengeStage()
    {
        Panel.GetComponent<LoadingScreen>().namesence = stageSO.NameSceneChallengeStage;
        Panel.GetComponent<LoadingScreen>().LoadingLevel();

    }
    // Update is called once per frame

}
