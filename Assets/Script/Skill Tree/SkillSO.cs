using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public string SkillName;
    public Sprite SkillIcon;
    public int currentLevel;
    public int maxLevel;
}
