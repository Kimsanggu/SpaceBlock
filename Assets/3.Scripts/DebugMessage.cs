using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMessage : SingletonClass<DebugMessage>
{
    public GameObject debugView = null;
    public Text txtDebugLog = null;

    void Awake()
    {
        OnLog("Debug On");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            debugView.SetActive((debugView.activeSelf) ? false : true);
        }    
    }

    public void OnReadMemoryTest()
    {
       
    
    }

    public void OnLogClear()
    {
        if (txtDebugLog == null) return;
        txtDebugLog.text = "";
    }

    public void OnLog(string msg)
    {
        Debug.Log("DebugLog : " + msg);
        if (txtDebugLog == null) return;

        debugView.SetActive(true);
        //if (!debugView.activeSelf) debugView.SetActive(true);
        
        string txtLog = txtDebugLog.text;
        System.DateTime data = System.DateTime.Now;

        txtLog += "\n" + (data.Hour + ":" + data.Minute+":" +data.Second+ "." + data.ToString("fff")) + msg;
        txtDebugLog.text = txtLog;
    }
}
