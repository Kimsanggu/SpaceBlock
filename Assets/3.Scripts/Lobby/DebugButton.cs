using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour {
    void Start()
    {
        if (MapManager.Instance.bDebug)
        {
            GetComponent<Image>().color = Color.blue;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
    public void OnClickButton()
    {
        if (MapManager.Instance.bDebug)
        {
            MapManager.Instance.bDebug = false;
            GetComponent<Image>().color = Color.white;
        }
        else
        {
            MapManager.Instance.bDebug = true;
            GetComponent<Image>().color = Color.blue;
        }
        SceneController.Instance.MoveScene(SCENENAME.Lobby);
    }
}
