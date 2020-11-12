using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {
    private static ResultManager instance;
    public static ResultManager Instance
    {
        get { return instance; }
    }

    public GameObject panel_Success;
    public GameObject FX_Line;
    public GameObject success;
    public GameObject fail;
    public Image panel;
    public Image panel_overlay;
    
    public RectTransform rocket;
    public RectTransform successUI;
    public RectTransform failUI;
    public Animator fireAni;
    public List<FlyingStar> starList;
    public List<ParticleSystem> failList;
    public Animator panelPoint;

    bool bOnClick = false;
    

    public float alphaSpeed;
    public float rocketSpeed;
    bool bSuccess;

    void Awake()
    {
        instance = GetComponent<ResultManager>();
    }
    void Start()
    {
        //Initialize(true);
    }
    public void Initialize(bool bSuccess)
    {
        SoundManager.Instance.PlayFlag = true;
        SoundManager.Instance.PlayBGM(2);
        SoundManager.Instance.SetBGMVolume(.3f);

        this.bSuccess = bSuccess;
        panel_Success.SetActive(true);
        if (bSuccess)
        {
            success.SetActive(true);
            StartCoroutine("SuccessFlow");
        }
        else
        {
            fail.SetActive(true);
            ParticleOn();
            StartCoroutine("FailFlow");
        }
        SoundManager.Instance.PlayEffect("eff_rocket_start");
    }
    void ParticleOn()
    {
        int count = failList.Count;
        for (int i = 0; i < count; i++)
        {
            failList[i].gameObject.SetActive(true);
        }
    }
    void StarAllClose()
    {
        int count = starList.Count;
        for (int i = 0; i < count; i++)
        {
            starList[i].gameObject.SetActive(false);
        }
    }
    IEnumerator FailFlow()
    {
        StarAllClose();
        Color col = panel.color;
        rocket.GetComponent<Animator>().Play("FailRocket_idle");
        while (col.a < 0.95f)
        {
            col.a = Mathf.MoveTowards(col.a, 1f, Time.deltaTime * alphaSpeed);
            panel.color = col;
            yield return null;
        }
        fireAni.Play("RocketFire_fire1");
        FX_Line.SetActive(true);
        while (rocket.anchoredPosition3D.x > 0f)
        {
            rocket.anchoredPosition3D = Vector3.Lerp(rocket.anchoredPosition3D, new Vector3(-50f, rocket.anchoredPosition3D.y, 0f), Time.deltaTime * rocketSpeed);
            yield return null;
        }
        while (failUI.anchoredPosition3D.y < 0f)
        {
            failUI.anchoredPosition3D = Vector3.Lerp(failUI.anchoredPosition3D, new Vector3(0f, 10f, 0f), Time.deltaTime * 2f);
            yield return null;
        }
    }
    IEnumerator SuccessFlow()
    {
        Color col = panel.color;
        while (col.a < 0.95f)
        {
            col.a = Mathf.MoveTowards(col.a, 1f, Time.deltaTime * alphaSpeed);
            panel.color = col;
            yield return null;
        }
        fireAni.Play("RocketFire_fire2");
        FX_Line.SetActive(true);
        while (rocket.anchoredPosition3D.x > 0f)
        {
            rocket.anchoredPosition3D = Vector3.Lerp(rocket.anchoredPosition3D, new Vector3(-50f, rocket.anchoredPosition3D.y, 0f), Time.deltaTime * rocketSpeed);
            yield return null;
        }
        int count = StarManager.Instance.GetStar();
        int savedStar = PlayerPrefs.GetInt(string.Format("Stage" + MapManager.Instance.Level.ToString()),0);
        if (savedStar < count)
        {
            PlayerPrefs.SetInt(string.Format("Stage" + MapManager.Instance.Level.ToString()), count);
        }
        MapManager.Instance.SaveStage();
        for (int i = 0; i < count; i++)
        {
            starList[i].Initialize();
            yield return new WaitForSeconds(0.5f);
        }
        while (successUI.anchoredPosition3D.y < 0f)
        {
            successUI.anchoredPosition3D = Vector3.Lerp(successUI.anchoredPosition3D, new Vector3(0f, 10f, 0f), Time.deltaTime * 2f);
            yield return null;
        }
    }
    public void OnClickButtonExit()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        if (!bSuccess)
        {
            panelPoint.Play("Panel_fly");
        }
        StartCoroutine(MoveScene(0));
    }
    public void OnClickButtonRetry()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        if (!bSuccess)
        {
            panelPoint.Play("Panel_fly");
        }
        StartCoroutine(MoveScene(1));
    }
    public void OnClickButtonNext()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        StartCoroutine(MoveScene(2));
    }
    IEnumerator FadeOut()
    {
        Color col = panel_overlay.color;
        while (col.a < 0.95f)
        {
            col.a = Mathf.MoveTowards(col.a, 1f, Time.deltaTime * alphaSpeed);
            panel_overlay.color = col;
            yield return null;
        }
    }
    IEnumerator MoveScene(int state)
    {
        if (bOnClick) yield break;
        bOnClick = true;
        StartCoroutine("FadeOut");
        while (rocket.anchoredPosition3D.x > -1500f)
        {
            rocket.anchoredPosition3D = Vector3.Lerp(rocket.anchoredPosition3D, new Vector3(-1600f, rocket.anchoredPosition3D.y, 0f), Time.deltaTime * rocketSpeed);
            yield return null;
        }
        if (SceneController.Instance != null)
        {
            switch (state)
            {
                case 0: SceneController.Instance.MoveScene(SCENENAME.Lobby); break;
                case 1: SceneController.Instance.MoveScene(SCENENAME.Game); break;
                case 2:
                    MapManager.Instance.Level++;
                    if (MapManager.Instance.Level > 25)
                    {
                        SceneController.Instance.MoveScene(SCENENAME.Lobby);
                    }
                    else
                    {
                        SceneController.Instance.MoveScene(SCENENAME.Game);
                    }
                    break;
            }
        }
    }
    
}
