using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TextMeshProUGUI text;
    public int availabePoint;
    private void OnEnable()
    {
        SkillSlot.onAbilitypointSpent += PointSpent;
        SkillSlot.onMaxLevelSkill += SkillMax;
    }
    private void OnDisable()
    {
        SkillSlot.onAbilitypointSpent -= PointSpent;
        SkillSlot.onMaxLevelSkill -= SkillMax;
    }
    void Start()
    {
        foreach(SkillSlot slot in skillSlots)
        {
            slot.SkillButton.onClick.AddListener(() => checkAvailablePoint(slot));
        }
        abilityPoint(0);
    }
    private void checkAvailablePoint(SkillSlot slot)
    {
        if(availabePoint > 0)
        {
            slot.TryToUpgrade();
        }
    }
    private void PointSpent(SkillSlot skilllot)
    {
        if(availabePoint > 0)
        {
            abilityPoint(-1);
        }
    }
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
    public void abilityPoint(int amount)
    {
        availabePoint += amount;
        text.text = availabePoint.ToString();
    }
     
}
