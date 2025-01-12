using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;        // Tên âm thanh
    public AudioClip Clip;     // Clip âm thanh
    public float volume = 1f;  // Âm lượng (mặc định là 1.0)
    public bool loop = false;  // Tùy chọn lặp lại
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] musicSound, SFXSound;
    public AudioSource musicSource, SFX_Source; // Các nguồn phát âm thanh

    private void Awake()
    {
        if (instance == null)
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

    // Phát nhạc nền với tùy chọn lặp lại
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Music '{name}' not found!");
        }
        else
        {
            musicSource.clip = s.Clip;
            musicSource.volume = 0; // Bắt đầu với âm lượng 0
            musicSource.loop = s.loop; // Gán chế độ lặp lại
            musicSource.Play();

            StartCoroutine(FadeInMusic(s.volume, 1f)); // Tăng dần âm lượng
        }
    }

    // Phát hiệu ứng âm thanh (không ảnh hưởng đến nhạc nền)
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSound, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
        else
        {
            SFX_Source.PlayOneShot(s.Clip, s.volume); // Phát một lần, không ảnh hưởng đến nhạc nền
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
        Sound s = Array.Find(SFXSound, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
        else if (SFX_Source.isPlaying && SFX_Source.clip == s.Clip)
        {
            SFX_Source.Stop();
        }
    }

    // Coroutine để tăng dần âm lượng nhạc nền
    private IEnumerator FadeInMusic(float targetVolume, float duration)
    {
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        musicSource.volume = targetVolume; // Đảm bảo âm lượng đạt đúng mức cuối
    }
}
