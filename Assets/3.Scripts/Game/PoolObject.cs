using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
    public float time;
    public Transform parnet;
    void OnEnable()
    {
        Invoke("Off", time);
    }
    void Off()
    {
        transform.SetParent(parnet);
        gameObject.SetActive(false);
    }
}
