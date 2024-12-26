using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Panel Của Audio
    [SerializeField] private GameObject audioPanel;    
    //2 thanh Slider
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        audioPanel.SetActive(false);
        MusicSlider.value = AudioManager.instance.musicSource.volume;
        SFXSlider.value = AudioManager.instance.SFX_Source.volume;
    }

    public void OpenPanel()
    {
        AudioManager.instance.PlaySFX("Pop");
        audioPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        audioPanel.SetActive(false);
        AudioManager.instance.PlaySFX("Pop");
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
