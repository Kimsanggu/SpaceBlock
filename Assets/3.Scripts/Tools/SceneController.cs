using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public enum SCENENAME
{
    Title,
    Lobby,
    Game,
    EmptyScene,
}
public class SceneController : MonoBehaviour {
    private static SceneController instance;

    public static SceneController Instance
    {
        get
        {
            return instance;
        }
    }
    public GameObject Canvas_Loading;
    public Image loadingBar;

    private const float DefaultLoadingTime = 1f;

    private SCENENAME _NextScene_Name;
    private SCENENAME _PrevScene_Name;
    float loadingTime = 3f;

    public SCENENAME NextSceneName {
        get { return _NextScene_Name; }
    }
    public SCENENAME PreSceneName
    {
        get { return _PrevScene_Name; }
    }

    void Awake()
    {
        instance = GetComponent<SceneController>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void StopLoading()
    {
        Canvas_Loading.SetActive(false);
    }
    public void MoveScene(SCENENAME sceneName)
    {
        SoundManager.Instance.StopBGM();
        loadingBar.fillAmount = 0f;
        _PrevScene_Name = (SCENENAME)SceneManager.GetActiveScene().buildIndex;
        //SoundController.Instance.AllSoundStop(false);
        _NextScene_Name = sceneName;
        Canvas_Loading.SetActive(true);
        Debug.Log("Canvas_Loading true :" +_NextScene_Name.ToString());
        StartCoroutine("LoadScene");
    }
    public void MoveScene(string name)
    {
        LogMessage("MoveScene : " + name);
         SCENENAME sceneName = (SCENENAME) Enum.Parse(typeof(SCENENAME), name);

         loadingBar.fillAmount = 0f;
         _PrevScene_Name = (SCENENAME)SceneManager.GetActiveScene().buildIndex;
         //SoundController.Instance.AllSoundStop(false);
         _NextScene_Name = sceneName;
         Canvas_Loading.SetActive(true);
         //Debug.Log("Canvas_Loading true :" + _NextScene_Name.ToString());
         StartCoroutine("LoadScene");
    }
    public void MoveScene(int index)
    {
        loadingBar.fillAmount = 0f;
        _PrevScene_Name = (SCENENAME)SceneManager.GetActiveScene().buildIndex;
        _NextScene_Name = (SCENENAME)index;
        Canvas_Loading.SetActive(true);
        //Debug.Log("Canvas_Loading true");
        StartCoroutine("LoadScene");
    }
    IEnumerator LoadScene()
    {
        float chTime = 0f;
        AsyncOperation async_Empty = SceneManager.LoadSceneAsync("EmptyScene");
        async_Empty.allowSceneActivation = false;
        float speed = 1f;
        while (loadingBar.fillAmount < 0.45f)
        {
            chTime += Time.deltaTime;
            //loadingBar.fillAmount = async_Empty.progress / 2f;
            loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, (async_Empty.progress / 2f), Time.deltaTime * speed);
            yield return null;
        }
        loadingBar.fillAmount = 0.5f;
        async_Empty.allowSceneActivation = true;
        //Canvas_Loading.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync((int)_NextScene_Name);
        Debug.Log("Loading : " + _NextScene_Name.ToString());
        async.allowSceneActivation = false;
        //chTime = 0f;
        while (loadingBar.fillAmount < 0.9f)
        {
            chTime += Time.deltaTime;
            loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, (async_Empty.progress / 2f) + 0.5f, Time.deltaTime * speed);
            yield return null;
        }
        while (chTime < loadingTime)
        {
            chTime += Time.deltaTime;
            yield return null;
        }
        loadingBar.fillAmount = 1f;
        async.allowSceneActivation = true;
        yield return null;
        //Canvas_Loading.SetActive(false);
    }
    public void Init(float value=0f)
    {
        //Debug.Log("LoadingScene Init");
        //Debug.Log(loadingBar.name);
        loadingBar.fillAmount = 0f;
        Canvas_Loading.SetActive(false);
    }
    public void LogMessage(string msg, bool bClear = false)
    {
        if (DebugMessage.Instance == null) return;

        if (bClear) DebugMessage.Instance.OnLogClear();

        DebugMessage.Instance.OnLog(msg);
    }
}
