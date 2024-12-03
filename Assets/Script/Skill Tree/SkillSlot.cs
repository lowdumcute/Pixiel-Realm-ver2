using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> listSkill;
    public SkillSO skillSO;
    public Image SkillIcon;
    public TextMeshProUGUI levelText;
    public Button SkillButton;
    public GameObject unlockedPanel;

    public bool isUnlocked;

    public Stats relatedStat; // Tham chiếu đến Stats liên quan

    public static event Action<SkillSlot> onAbilitypointSpent;
    public static event Action<SkillSlot> onMaxLevelSkill;

    private void Start()
    {
        unlockedPanel.SetActive(false);
        PreviewNextStats(); // Cập nhật preview stats ban đầu
    }

    public void OpenPanel()
    {
        // Kiểm tra nếu skill đã đạt max level thì không cho mở panel
        if (skillSO.currentLevel >= skillSO.maxLevel)
        {
            unlockedPanel.SetActive(false);
        }
        else
        {
            unlockedPanel.SetActive(true);
        }
    }

    private void OnValidate()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (skillSO != null)
        {
            SkillIcon.sprite = skillSO.SkillIcon;
        }

        if (isUnlocked)
        {
            SkillButton.interactable = true;
            levelText.text = $"{skillSO.currentLevel} / {skillSO.maxLevel}";
            SkillIcon.color = Color.white;

            // Ẩn panel nếu đã đạt max level
            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                unlockedPanel.SetActive(false); // Không cho mở panel nữa khi max level
            }
            else
            {
                unlockedPanel.SetActive(true); // Nếu chưa max level, vẫn cho phép mở panel
            }
        }
        else
        {
            levelText.text = "LOCKED";
            SkillButton.interactable = false;
            SkillIcon.color = Color.grey;
            unlockedPanel.SetActive(false); // Ẩn panel khi chưa mở khóa
        }

        PreviewNextStats(); // Cập nhật giá trị preview sau mỗi thay đổi
    }

    public void TryToUpgrade()
    {
        if (isUnlocked && skillSO.currentLevel < skillSO.maxLevel)
        {
            skillSO.currentLevel++;
            onAbilitypointSpent?.Invoke(this);
            UpdateStats(); // Cập nhật Stats sau khi nâng cấp
            UpdateUI();

            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                onMaxLevelSkill?.Invoke(this);
                UpdateUI(); // Cập nhật lại UI khi đạt max level
            }
        }
    }

    void UpdateStats()
    {
        if (relatedStat != null)
        {
            int statIncrease = 10; // Giá trị tăng mỗi level
            relatedStat.UpdateQuantity(statIncrease);
        }
    }

    void PreviewNextStats()
    {
        if (relatedStat != null && isUnlocked && skillSO.currentLevel < skillSO.maxLevel)
        {
            int nextStatIncrease = 10; // Giá trị preview mỗi level
            relatedStat.PreviewNextQuantity(nextStatIncrease);
        }
    }

    public void Unlocked()
    {
        isUnlocked = true;
        UpdateUI();
    }

    public bool CanbeUnlock()
    {
        foreach (SkillSlot slot in listSkill)
        {
            if (!slot.isUnlocked || slot.skillSO.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }
}
