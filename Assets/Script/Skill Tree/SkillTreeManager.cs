using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    
    void Start()
    {
        foreach(SkillSlot slot in skillSlots)
        {
            slot.SkillButton.onClick.AddListener(slot.TryToUpgrade);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
