using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] AssetDisplay assetDisplay;
    public List<SkillSlot> listSkill;
    public SkillSO skillSO;
    public Image SkillIcon;
    public TextMeshProUGUI levelText;
    public Button SkillButton;
    public GameObject unlockedPanel;

    public bool isUnlocked;

    public Stats relatedStat; // Tham chiếu đến Stats liên quan

    //Tham số đến tài nguyên đang có 
    public Asset asset;
    // TextMessPro để hiện thông tin tài nguyên cần nâng cấp 
    //Chỉ số vàng
    public TextMeshProUGUI Goldcurrent;
    public TextMeshProUGUI GoldNeed;
    //chỉ số ngôi sao 
    public TextMeshProUGUI StarCurrent;
    public TextMeshProUGUI StarNeed;
    
    //Event Trigger khi nâng cấp 
    public static event Action<SkillSlot> onAbilitypointSpent;
    public static event Action<SkillSlot> onMaxLevelSkill;

    private void Start()
    {
        isUnlocked = skillSO.SkillIsUnlocked;
        GoldUI();


        unlockedPanel.SetActive(false);

        PreviewNextStats(); // Cập nhật preview stats ban đầu
        UpdateUI();
    }

    public void OpenPanel()
    {
        // Kiểm tra nếu skill chưa được unlock, không cho phép mở panel
        if (!isUnlocked)
        {
            unlockedPanel.SetActive(false); // Tắt panel nếu skill chưa được unlock
        }
        else
        {
            // Kiểm tra nếu skill đã đạt max level thì không cho mở panel
            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                unlockedPanel.SetActive(false); // Tắt panel nếu skill đã đạt max level
            }
            else
            {
                unlockedPanel.SetActive(true); // Mở panel nếu skill chưa đạt max level
            }
        }
    }
    public void ClosePnael()
    {
        unlockedPanel.SetActive(false);
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

        if (asset.Gold >= skillSO.GoldNeed)
        {
            Goldcurrent.color = Color.yellow;
            SkillButton.interactable = true;
        }
        else
        {
            Goldcurrent.color = Color.red;
            SkillButton.interactable = false;
        }
        PreviewNextStats(); // Cập nhật giá trị preview sau mỗi thay đổi
    }

    public void TryToUpgrade()
    {
        if (isUnlocked && skillSO.currentLevel < skillSO.maxLevel)
        {
            skillSO.currentLevel++;
            //trigger event
            onAbilitypointSpent?.Invoke(this);
            //trừ tiền sau khi nâng cấp
            asset.Gold -= skillSO.GoldNeed;
            //cập nhật lại tiền sau nâng cấp
            GoldUI();
            UpdateStats(); // Cập nhật Stats sau khi nâng cấp
            UpdateUI();

            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                onMaxLevelSkill?.Invoke(this);
                UpdateUI(); // Cập nhật lại UI khi đạt max level
            }
            assetDisplay.UpdateDisplay();
        }
    }
    //Cập nhật lại chữ tiền 
    private void GoldUI()
    {
        Goldcurrent.text = asset.Gold.ToString();
        GoldNeed.text = "/ " + skillSO.GoldNeed.ToString();
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
        skillSO.SkillIsUnlocked = true;
        UpdateUI();  // Cập nhật giao diện sau khi skill được mở khóa
        GameObject Panel = transform.GetChild(2).gameObject; // Mở panel sau khi skill được mở khóa
        Panel.SetActive(false);
    }

    // Kiểm tra và tự động mở khóa skill nếu tất cả skill trong list đạt max level
    public bool CanbeUnlock()
    {
        bool allSkillsMaxLevel = true;

        // Kiểm tra tất cả các skill trong listSkill
        foreach (SkillSlot slot in listSkill)
        {
            if (slot.skillSO.currentLevel < slot.skillSO.maxLevel)  // Nếu có bất kỳ skill nào chưa đạt max level
            {
                allSkillsMaxLevel = false;
                break;  // Thoát vòng lặp nếu phát hiện có skill chưa max level
            }
        }

        // Nếu tất cả skill đều đạt max level và skill hiện tại chưa unlock, thì mở khóa
        if (allSkillsMaxLevel && !isUnlocked)
        {
            Unlocked(); // Mở khóa skill
        }

        return allSkillsMaxLevel;  // Trả về true nếu tất cả skill đạt max level
    }
}
