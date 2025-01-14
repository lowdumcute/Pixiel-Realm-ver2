using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject audioPanel;    
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Start()
    {
        audioPanel.SetActive(false);

        // Lấy giá trị âm lượng từ PlayerPrefs, nếu không có thì mặc định là 1
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        // Cập nhật Slider và âm lượng
        MusicSlider.value = savedMusicVolume;
        SFXSlider.value = savedSFXVolume;

        AudioManager.instance.VolumeMusic(savedMusicVolume);
        AudioManager.instance.SetSFXVolume(savedSFXVolume);
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

    public void OnMusicSliderChanged()
    {
        float newMusicVolume = MusicSlider.value;
        AudioManager.instance.VolumeMusic(newMusicVolume);

        // Lưu giá trị âm lượng vào PlayerPrefs
        PlayerPrefs.SetFloat(MusicVolumeKey, newMusicVolume);
        PlayerPrefs.Save(); // Ghi giá trị ngay lập tức
    }

    public void OnSFXSliderChanged()
    {
        float newSFXVolume = SFXSlider.value;
        AudioManager.instance.SetSFXVolume(newSFXVolume);

        // Lưu giá trị âm lượng vào PlayerPrefs
        PlayerPrefs.SetFloat(SFXVolumeKey, newSFXVolume);
        PlayerPrefs.Save(); // Ghi giá trị ngay lập tức
    }
}
