using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TextMeshProUGUI text;
    public int availabePoint;


    //Unity Event
    private void OnEnable()
    {
        SkillSlot.onAbilitypointSpent += PointSpent;
        SkillSlot.onAbilitypointSpent += SkillSpent;
        SkillSlot.onMaxLevelSkill += SkillMax;
    }
    private void OnDisable()
    {
        SkillSlot.onAbilitypointSpent -= PointSpent;
        SkillSlot.onAbilitypointSpent -= SkillSpent;
        SkillSlot.onMaxLevelSkill -= SkillMax;
    }
    void Start()
    {
        //gán nút nâng cấp cho mỗi skill button
        foreach(SkillSlot slot in skillSlots)
        {
            slot.SkillButton.onClick.AddListener(() => checkAvailablePoint(slot));
        }
        abilityPoint(0);
    }
    
    
    //hàm kiểm tra xem skill có max hay chưa nếu rồi thì mở skill tiếp theo

    private void SkillMax(SkillSlot skillSlot)
    {
        foreach(SkillSlot slot in skillSlots)
        {
            if(!slot.isUnlocked && slot.CanbeUnlock())
            {
                slot.Unlocked();
            }
        }
    }
    //điểm để nâng cấp Skill(tạm thời)
    //sau này đổi thành tài nguyên sau

    public void abilityPoint(int amount)
    {
        availabePoint += amount;
        text.text = availabePoint.ToString();
    }
    private void PointSpent(SkillSlot skilllot)
    {
        if (availabePoint > 0)
        {
            abilityPoint(-1);
        }
    }
    //kiểm tra xem có đủ điểm để nâng cấp hay không nếu không thì không nâng cấp
    private void checkAvailablePoint(SkillSlot slot)
    {
        if (availabePoint > 0)
        {
            slot.TryToUpgrade();
        }
    }
    //Hàm kiểm tra xem việc nâng cấp là gì
    private void SkillSpent(SkillSlot skilllot)
    {
        string skillname = skilllot.name;
        switch(skillname)
        {
            //Tên skill muốn nâng
            case "1":
                //Gọi hàm nâng cấp 1 lần
                break;
            case "":

                break;
             default:
                Debug.Log("Không rõ tên skill muốn nâng");
                break;
        }
    }
}
