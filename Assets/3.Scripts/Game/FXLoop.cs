using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLoop : MonoBehaviour {
    public bool bLoop;
    public float loopTime;
    ParticleSystem fx;

    void Start()
    {
        fx = GetComponent<ParticleSystem>();
        StartCoroutine("Flow");
    }
    IEnumerator Flow()
    {
        while (bLoop)
        {
            yield return new WaitForSeconds(loopTime);
            fx.Play();
        }
    }
	
}
