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
    public AudioSource musicSource; // Nguồn phát nhạc nền
    private List<AudioSource> sfxSources = new List<AudioSource>(); // Danh sách nguồn phát SFX

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

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
        // Lấy âm lượng từ PlayerPrefs hoặc đặt mặc định là 1
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        // Áp dụng giá trị âm lượng
        VolumeMusic(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Phát nhạc nền ban đầu
        PlayMusic("MainMenuTheme");
    }

    // Phát nhạc nền
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

    // Phát hiệu ứng âm thanh
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSound, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
        else
        {
            AudioSource source = GetOrCreateAudioSource();
            source.clip = s.Clip;
            source.volume = s.volume;
            source.loop = s.loop;
            source.Play();

            // Nếu không lặp lại, tự động hủy nguồn phát sau khi phát xong
            if (!s.loop)
            {
                StartCoroutine(ReleaseAudioSourceAfterPlay(source));
            }
        }
    }

    // Lấy hoặc tạo một AudioSource mới
    private AudioSource GetOrCreateAudioSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        sfxSources.Add(newSource);
        return newSource;
    }

    // Hủy AudioSource sau khi phát xong
    private IEnumerator ReleaseAudioSourceAfterPlay(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        source.clip = null;
    }

    // Điều chỉnh âm lượng nhạc nền và lưu vào PlayerPrefs
    public void VolumeMusic(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // Điều chỉnh âm lượng hiệu ứng âm thanh và lưu vào PlayerPrefs
    public void SetSFXVolume(float volume)
    {
        foreach (var source in sfxSources)
        {
            source.volume = volume;
        }

        // Cập nhật âm lượng mặc định cho các SFX mới
        foreach (var sound in SFXSound)
        {
            sound.volume = volume;
        }

        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // Lấy âm lượng hiện tại của SFX
    public float GetCurrentSFXVolume()
    {
        if (sfxSources.Count > 0)
        {
            return sfxSources[0].volume;
        }
        return SFXSound.Length > 0 ? SFXSound[0].volume : 1f;
    }

    // Tăng dần âm lượng nhạc nền
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

        musicSource.volume = targetVolume;
    }
}
