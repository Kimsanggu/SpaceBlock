using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {
    public Image guageBar;
    float time = 2f;
    float chTime = 0f;
    bool bCheck = false;
    
    void Start()
    {
        guageBar.fillAmount = 0f;
        
    }
    void Update()
    {
        chTime += Time.deltaTime;
        if (chTime > time)
        {
            if (!bCheck)
            {
                bCheck = true;
                MoveScene();
            }
        }
        guageBar.fillAmount = chTime / time;
    }
    void MoveScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SCENENAME.Lobby);
    }
}
