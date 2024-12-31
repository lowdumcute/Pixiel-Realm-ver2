using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] AssetDisplay assetDisplay;
    public List<SkillSlot> listSkill;
    public SkillSO skillSO;
    public Image SkillIcon;
    public TextMeshProUGUI levelText;
    public Button SkillButton;
    public GameObject unlockedPanel;
    public Image connectionLine; // Đường nối đổi màu khi cấp tối đa

    public bool isUnlocked;

    public Stats relatedStat; // Tham chiếu đến Stats  sẽ cộng 
    public Asset asset;// Tham  chiếu đến tài nguyên

    //2 Textmeshpro vàng trong panel
    public TextMeshProUGUI currentGold;
    public TextMeshProUGUI GoldNeeded;

    public static event Action<SkillSlot> onAbilitypointSpent;
    public static event Action<SkillSlot> onMaxLevelSkill;
    private void Awake()
    {
        isUnlocked = skillSO.isUnlocked;
        UpdateUI();
    }
    private void Start()
    {
        
        UpdateTextGold();
        unlockedPanel.SetActive(false);
        PreviewNextStats(); // Cập nhật preview stats ban đầu   
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
    public void ClosePanel()
    {
        unlockedPanel.SetActive(false);
    }

    private void OnValidate()
    {
        UpdateUI();
    }
    void UpdateTextGold()
    {
        currentGold.text = asset.Gold.ToString();
        GoldNeeded.text = "/ "+skillSO.GoldRequire.ToString();
    }
    private void UpdateUI()
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

            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                unlockedPanel.SetActive(false);
            }
            else
            {
                unlockedPanel.SetActive(true);
            }
        }
        else
        {
            levelText.text = "LOCKED";
            SkillButton.interactable = false;
            SkillIcon.color = Color.grey;
            unlockedPanel.SetActive(false);
        }

        if (asset.Gold >= skillSO.GoldRequire)
        {
            SkillButton.interactable = true;
            currentGold.color = Color.yellow;
        }
        else
        {
            currentGold.color = Color.red;
            SkillButton.interactable = false;
        }

        PreviewNextStats();
        UpdateConnectionLineColor(); // Cập nhật màu đường nối
    }

    private void UpdateConnectionLineColor()
    {
        if (connectionLine != null)
        {
            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                connectionLine.color = Color.green; // Màu xanh lá nếu max cấp
            }
            else
            {
                connectionLine.color = new Color(0.5f, 0f, 0.5f); // Màu tím đậm nếu chưa max cấp
            }
        }
    }

    public void TryToUpgrade()
    {
        if (isUnlocked && skillSO.currentLevel < skillSO.maxLevel)
        {
            skillSO.currentLevel++;
            onAbilitypointSpent?.Invoke(this);
            asset.Gold -= skillSO.GoldRequire;
            UpdateTextGold();
            UpdateStats();
            UpdateUI();

            if (skillSO.currentLevel >= skillSO.maxLevel)
            {
                onMaxLevelSkill?.Invoke(this);
                UpdateUI();
            }

            assetDisplay.UpdateDisplay();
        }
    }

    void UpdateStats()
    {
        if (relatedStat != null)
        {
            int statIncrease = relatedStat.AddQuantity;
            relatedStat.UpdateQuantity(statIncrease);
        }
    }

    void PreviewNextStats()
    {
        if (relatedStat != null && isUnlocked && skillSO.currentLevel < skillSO.maxLevel)
        {
            relatedStat.PreviewNextQuantity();
        }
    }

    public void Unlocked()
    {
        isUnlocked = true;
        skillSO.isUnlocked = true;
        UpdateUI();  // Cập nhật giao diện sau khi skill được mở khóa
        OpenPanel(); // Mở panel sau khi skill được mở khóa

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
