using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    // Màn hình loading
    [SerializeField] private GameObject LoadingPanel;
    //Thanh Loading
    [SerializeField] private Slider LoadingSlider;
    // Tên Scene muốn chuyển
    [SerializeField] private string name;

    private void Start()
    {
        LoadingLevel();
    }
    public void LoadingLevel()
    {
       
        LoadingPanel.SetActive(true);
        StartCoroutine(LoadScene(name));
    }

    // Update is called once per frame
    IEnumerator LoadScene(string LevelToLoad)
    {
        //Tiến trình LoadScene
        AsyncOperation loadGame = SceneManager.LoadSceneAsync(LevelToLoad);
        //Nếu Chưa Load Xong thì
        while(!loadGame.isDone)
        {
            //thanh tiến trình bằng với Tiến trình loadScene
            float progess = Mathf.Clamp01(loadGame.progress / 0.9f);
            //gán giá trị cho slider
            LoadingSlider.value = progess;
            //cập nhật mỗi khung hình 
            yield return null;
        }

    }
}
