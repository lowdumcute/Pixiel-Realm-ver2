using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO;
    public Image SkillIcon;

    private void OnValidate()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUI()
    {
        if (skillSO!= null)
        {
            SkillIcon.sprite = skillSO.SkillIcon;
        }
    }
}
