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

    public SkillSO skillSO;
    public Image SkillIcon;
    public TextMeshProUGUI levelText;

    public Button SkillButton;
    public GameObject unlockedPanel;

    public int currentlevel;
    public bool isUnlocked;
    public static event Action<SkillSlot> onAbilitypointSpent;
    public static event Action<SkillSlot> onMaxLevelSkill;
    private void OnValidate()
    {
        UpdateUI();
    }

    // Update is called once per frame
    
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
