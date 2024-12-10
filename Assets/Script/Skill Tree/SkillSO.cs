using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public bool isUnlocked;
    public string SkillName;
    public Sprite SkillIcon;
    public int GoldRequire;
    public int currentLevel;
    public int maxLevel;
}
