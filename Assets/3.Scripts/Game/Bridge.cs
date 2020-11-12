using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {
    public GameObject fx_Steam;
    Animator ani;
    bool bLock = false;

    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void Lock(bool bL)
    {
        SoundManager.Instance.PlayEffect("eff_arm",0.2f);
        SoundManager.Instance.PlayEffect("eff_steam",0.2f);
        if (bL)
        {
            bLock = true;
            ani.Play("Bridge_lock");
            fx_Steam.SetActive(true);
        }
        else
        {
            bLock = false;
            ani.Play("Bridge_unlock");
            fx_Steam.SetActive(true);
        }
    }
    public bool GetLock()
    {
        return bLock;
    }
}
