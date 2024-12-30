using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound[] musicSound, SFXSound;
    // Ngưồn phát âm thanh 
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

    // Phát nhạc nền
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Không có âm thanh");
        }
        else
        {
            musicSource.clip = s.Clip;
            musicSource.Play();
        }
    }

    // Phát hiệu ứng âm thanh
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Không có âm thanh");
        }
        else
        {
            SFX_Source.clip = s.Clip;
            SFX_Source.Play();
        }
    }

    // Điều chỉnh âm lượng nhạc nền
    public void VolumeMusic(float volume)
    {
        musicSource.volume = volume;
    }

    // Điều chỉnh âm lượng hiệu ứng âm thanh
    public void VolumeSFX(float volume)
    {
        SFX_Source.volume = volume;
    }

    // Dừng nhạc nền
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Dừng hiệu ứng âm thanh
    public void StopSFX(string name)
    {
        // Tìm âm thanh trong danh sách
        Sound s = Array.Find(SFXSound, x => x.name == name);
        
        if (s == null)
        {
            Debug.Log("Không có âm thanh");
        }
        else
        {
            // Kiểm tra xem âm thanh có đang phát không, nếu có thì dừng lại
            if (SFX_Source.isPlaying && SFX_Source.clip == s.Clip)
            {
                SFX_Source.Stop();
            }
        }
    }
}
