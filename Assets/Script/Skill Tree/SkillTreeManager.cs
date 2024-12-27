using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TextMeshProUGUI text;


    //Unity Event
    private void OnEnable()
    {
        SkillSlot.onAbilitypointSpent += SkillSpent;
        SkillSlot.onMaxLevelSkill += SkillMax;
    }
    private void OnDisable()
    {
        SkillSlot.onAbilitypointSpent -= SkillSpent;
        SkillSlot.onMaxLevelSkill -= SkillMax;
    }
    void Start()
    {
    }
    
    
    //hàm kiểm tra xem skill có max hay chưa nếu rồi thì mở skill tiếp theo

    private void SkillMax(SkillSlot skillSlot)
    {
        foreach(SkillSlot slot in skillSlots)
        {
            if(!slot.isUnlocked && slot.CanbeUnlock())  // Nếu skill chưa được unlock và có thể unlock
            {
                slot.Unlocked(); // Mở khóa skill
            }
        }
    }
    //điểm để nâng cấp Skill(tạm thời)
    //sau này đổi thành tài nguyên sau


    //kiểm tra xem có đủ điểm để nâng cấp hay không nếu không thì không nâng cấp
    //Hàm kiểm tra xem việc nâng cấp là gì
    private void SkillSpent(SkillSlot skilllot)
    {
        string skillname = skilllot.name;

        switch(skillname)
        {
            //Tên skill của Skill SO
            case "1":
                //Gọi hàm nâng cấp 1 lần hoặc mở khóa skill mới dựa theo tên
                break;
            case "":

                break;
             default:
                break;
        }
    }
}
