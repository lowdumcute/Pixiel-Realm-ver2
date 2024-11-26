using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
//tạo skill ngoài Editor
[CreateAssetMenu(fileName = "NewSkill",menuName = "SkillTree/Skill" ) ]
public class SkillSO : ScriptableObject
{
    public string SkillName;
    public int maxLevel;
    public Sprite SkillIcon;
}
