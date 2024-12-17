using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Panel Của Audio
    
    //2 thanh Slider
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    //2 nút bật tắt Panel
    [SerializeField]
    private Animator ani;
    private void Start()
    {
        MusicSlider.value = AudioManager.instance.musicSource.volume;
        SFXSlider.value = AudioManager.instance.SFX_Source.volume;
        ani = GetComponent<Animator>();
    }

    public void OpenPanel()
    {
        AudioManager.instance.PlaySFX("Pop");
        ani.SetTrigger("Open");
    }
    public void ClosePanel()
    {

        AudioManager.instance.PlaySFX("Pop");
        ani.SetTrigger("Close");
    }
    public void MusicVolume()
    {
        AudioManager.instance.VolumeMusic(MusicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.instance.VolumeSFX(SFXSlider.value);
    }
}
