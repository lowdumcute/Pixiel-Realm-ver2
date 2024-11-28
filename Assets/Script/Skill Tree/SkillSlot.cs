using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    //danh sách các skill cần mở khóa trước khi mở skll này
    public List<SkillSlot> listSkill;

    //Skill SO đã tạo 
    public SkillSO skillSO;

    //ảnh của Button
    public Image SkillIcon;

    //hiển thị skill mở khóa hoặc đang ở level nào
    public TextMeshProUGUI levelText;

    //nút nhấn khi nâng cấp 
    public Button SkillButton;

    //Panel để hiện lên xem nâng cấp hay không
    public GameObject unlockedPanel;

    public int currentlevel;
    public bool isUnlocked;
    //Event gọi trong SkillTreemanager
    public static event Action<SkillSlot> onAbilitypointSpent;
    public static event Action<SkillSlot> onMaxLevelSkill;

    private void OnValidate()//hàm này được gọi khi mà có thay đổi thông số của gameobject
    {
        UpdateUI();
    }

    
    //Cập nhật UI
    void UpdateUI()
    {
        if (skillSO!= null)
        {
            SkillIcon.sprite = skillSO.SkillIcon;
        }
        if(isUnlocked)
        {
            unlockedPanel.SetActive(false);
            SkillButton.interactable = true;
            
            levelText.text = $"{currentlevel} / {skillSO.maxLevel}"; 
            SkillIcon.color = Color.white;
            
        }
        else
        {
            unlockedPanel.SetActive(true);
            levelText.text = "LOCKED";
            SkillButton.interactable = false;
            SkillIcon.color = Color.grey;
            
        }
    }
    //Hàm nâng cấp Level
    public void TryToUpgrade() 
    {
        if (isUnlocked && currentlevel < skillSO.maxLevel)
        {
            currentlevel++;
            onAbilitypointSpent?.Invoke(this);
            UpdateUI();
            if (currentlevel >= skillSO.maxLevel)
            {
                onMaxLevelSkill?.Invoke(this);
                UpdateUI();
            }
            
        }
    }
    //2 hàm kiểm tra xem có nâng cấp hay không
    public void Unlocked()
    {
        isUnlocked = true;
        UpdateUI();
    }
    public bool CanbeUnlock()
    {
        foreach(SkillSlot slot in listSkill)
        {
            if(!slot.isUnlocked || slot.currentlevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }

}
