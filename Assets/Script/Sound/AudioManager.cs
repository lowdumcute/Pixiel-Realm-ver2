using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound[] musicSound, SFXSound;
    //Ngưồn phát âm thanh 
    public AudioSource musicSource, SFX_Source;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);       
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("MainMenuTheme");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Khong co am thanh");
        }
        else
        {
            musicSource.clip = s.Clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Khong co am thanh");
        }
        else
        {
            SFX_Source.clip = s.Clip;
            SFX_Source.Play();
        }
    }
    public void VolumeMusic(float volume)
    {
        musicSource.volume = volume;
    }
    public void VolumeSFX(float volume)
    {
        SFX_Source.volume = volume;
    }
}
