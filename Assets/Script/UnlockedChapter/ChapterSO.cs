using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chapter SO",menuName = "Chapter SO")]
public class ChapterSO :ScriptableObject
{
    public bool isUnlocked;
    public StageSO[] stage;
}
