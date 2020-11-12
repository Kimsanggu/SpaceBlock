using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum LobbyPanel
{
    Lobby,
    Setting,
    Exit,
    Guide
}
public class LobbyManager : MonoBehaviour {
    private static LobbyManager instance;
    public static LobbyManager Instance
    {
        get
        {
            return instance;
        }
    }
    public List<GameObject> panels;
    public ScrollRect scroll;

    int selectedstage;

    void Awake()
    {
        instance = GetComponent<LobbyManager>();
    }

    void Start()
    {
        SceneController.Instance.StopLoading();
        selectedstage = 1;
        SetPanel(LobbyPanel.Lobby);
        SoundManager.Instance.PlayFlag = true;
        SoundManager.Instance.PlayBGM(0);
        SoundManager.Instance.PlayBGM(.3f);
        AutoScroll();
    }
    public void AutoScroll()
    {
        int maxLevel = MapManager.Instance.MaxLevel;
        float moveX = 0f;
        float sizeX = scroll.content.gameObject.GetComponent<GridLayoutGroup>().cellSize.x + scroll.content.gameObject.GetComponent<GridLayoutGroup>().spacing.x;
        if (maxLevel < 3)
        {
            moveX = 0f;
        }
        else if (maxLevel >= 3 && maxLevel < 23)
        {
            moveX=-sizeX*(maxLevel-3f);
        }
        else
        {
            moveX = -sizeX * 20f;
        }
        
        scroll.content.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(moveX, 0f, 0f);
    }
    public void SetPanel(LobbyPanel state)
    {
        AllClose();
        panels[(int)state].SetActive(true);
    }
    void AllClose()
    {
        int count = panels.Count;
        for (int i = 0; i < count; i++)
        {
            panels[i].SetActive(false);
        }
    }
    public void OnClickButtonGameStart()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        MapManager.Instance.Level = selectedstage;
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    public void OnClickButtonExit()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SetPanel(LobbyPanel.Exit);
    }
    public void OnClickButtonExit_Yes()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        Application.Quit();
    }
    public void OnClickButtonExit_No()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SetPanel(LobbyPanel.Lobby);
    }
    public void OnClickButtonSetting()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SetPanel(LobbyPanel.Setting);
    }
    public void OnClickButtonGuide()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SetPanel(LobbyPanel.Guide);
    }
    public void OnClickButtonGuideConfirm()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SetPanel(LobbyPanel.Lobby);
    }
}
