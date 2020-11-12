using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private static SettingManager instance;
    public static SettingManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<GameObject> btnBGMList;
    public List<GameObject> btnEffectList;

    bool bFirst = true;

    void Awake()
    {
        instance = GetComponent<SettingManager>();
    }
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        OnClickButtonBGM(MapManager.Instance.BGM == 1 ? true : false);
        OnClickButtonEffect(MapManager.Instance.Effect == 1 ? true : false);
        bFirst = false;
    }

    public void OnClickButtonBack()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        LobbyManager.Instance.SetPanel(LobbyPanel.Lobby);
    }
    public void OnClickButtonBGM(bool onff)
    {
        if (!bFirst)
        {
            SoundManager.Instance.PlayEffect("eff_button");
        }
        //SoundManager.Instance.PlayEffect(SoundName.eff_button.ToString(), true);
        AllCloseButtonBGM();
        MapManager.Instance.OnClickButtonBGM(onff);
        if (onff)
        {
            btnBGMList[0].SetActive(true);
            SoundManager.Instance.PlayFlag = true;
            SoundManager.Instance.PlayBGM(0);
            SoundManager.Instance.PlayBGM(.3f);
        }
        else
        {
            btnBGMList[1].SetActive(true);
            SoundManager.Instance.AllSoundStop(true);
        }
    }
    public void OnClickButtonEffect(bool onff)
    {
        if (!bFirst)
        {
            SoundManager.Instance.PlayEffect("eff_button");
        }
        //SoundManager.Instance.PlayEffect(SoundName.eff_button.ToString(), true);
        AllCloseButtonEffect();
        MapManager.Instance.OnClickButtonEffect(onff);
        if (onff)
        {
            btnEffectList[0].SetActive(true);
        }
        else
        {
            btnEffectList[1].SetActive(true);
        }
    }
    void AllCloseButtonBGM()
    {
        int count = btnBGMList.Count;
        for (int i = 0; i < count; i++)
        {
            btnBGMList[i].SetActive(false);
        }
    }
    void AllCloseButtonEffect()
    {
        int count = btnEffectList.Count;
        for (int i = 0; i < count; i++)
        {
            btnEffectList[i].SetActive(false);
        }
    }



}
