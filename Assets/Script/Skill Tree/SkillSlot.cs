using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    
    public SkillSO skillSO;
    public Image SkillIcon;
    public TextMeshProUGUI levelText;

    public Button SkillButton;
    public GameObject unlockedPanel;

    public int currentlevel;
    public bool isUnlocked;
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
            levelText.gameObject.SetActive(true);
            levelText.text = $"{currentlevel} / {skillSO.maxLevel}"; 
            SkillIcon.color = Color.white;
            
        }
        else
        {
            unlockedPanel.SetActive(true);
            levelText.gameObject.SetActive(false);
            SkillButton.interactable = false;
            SkillIcon.color = Color.grey;
            
        }
    }
    public void TryToUpgrade()
    {
        if (isUnlocked && currentlevel < skillSO.maxLevel)
        {
            currentlevel++;
            UpdateUI();
        }
    }
}
