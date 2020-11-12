using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererFollowTarget : MonoBehaviour {
    public GameObject target;
    LineRenderer line;
    public float time;
    float chTime = 0f;
    public int maxCount;
    int index = 0;
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = maxCount;
    }
	void FixedUpdate () {
        chTime += Time.fixedDeltaTime;
        if (chTime>time)
        {
            chTime = 0f;
            line.SetPosition(index, target.transform.position);
            index++;
            if (index >= maxCount)
            {
                index = 0;
            }
        }
	}
}
