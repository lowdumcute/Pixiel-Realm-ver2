using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public bool SkillIsUnlocked;
    public string SkillName;
    public Sprite SkillIcon;
    public int GoldNeed;
    public int StartNeed;
    public int currentLevel;
    public int maxLevel;

}
