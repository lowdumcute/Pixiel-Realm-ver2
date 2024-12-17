using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelActive : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Button openPanel;
    [SerializeField] private Button closePanel;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
        openPanel.onClick.AddListener(Open);
        closePanel.onClick.AddListener(Close);
        Panel.SetActive(false);
    }

    private void Open()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            AudioManager.instance.PlaySFX("Button");
            Panel.SetActive(true);
        }
        

    }
    private void Close()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
        else
        {
            AudioManager.instance.PlaySFX("Button");
            Panel.SetActive(false);
        }
    }
    
}
