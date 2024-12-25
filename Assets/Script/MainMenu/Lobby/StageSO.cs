using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewStage", menuName = "Stage")]
public class StageSO:ScriptableObject
{
    public bool isUnlocked;
    public int Star;
    public string chapter;
    public string Decription;
    public int NormalStageEnergy;
    public int ChanllengeStageEnergy;
    public Sprite[] BonusImage;
    public string NameSceneNormalStage;
    public string NameSceneChallengeStage;

    public event System.Action OnDataChange;
    private void OnValidate()
    {
        OnDataChange?.Invoke();
    }
}
