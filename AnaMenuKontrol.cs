using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnaMenuKontrol : MonoBehaviour
{
    public GameObject LoadingPanel;
    public GameObject CikisPanel;
    public Slider LoadingSlider;
    public void OyunaBasla()
    {
        StartCoroutine(sahneYuklemeLoading());
    }
    public void OyundanCik()
    {
       CikisPanel.SetActive(true);
    }
    public void Evet()
    {
        Debug.Log("çýkýldý");
        Application.Quit();
    }
    public void Hayir()
    {
        CikisPanel.SetActive(false);
    }
    public void Yeniden()
    {
        PlayerPrefs.DeleteKey("OyunBasladiMi");
        StartCoroutine(sahneYuklemeLoading());
    }
    IEnumerator sahneYuklemeLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        LoadingPanel.SetActive(true);
        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            LoadingSlider.value = ilerleme;
            yield return null;
        }
    }
}
