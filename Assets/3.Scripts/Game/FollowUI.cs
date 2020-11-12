using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour {
    public RectTransform target;
    public RectTransform subTarget;
    public bool bFollow;
    public Vector3 offset;
    RectTransform rTr;
	void Start () {
        rTr = GetComponent<RectTransform>();
	}
	
	void LateUpdate () {
        if (bFollow)
        {
            if (target != null)
            {
                if (subTarget == null)
                {
                    rTr.anchoredPosition3D = target.anchoredPosition3D + offset;
                }
                else
                {
                    rTr.anchoredPosition3D = target.anchoredPosition3D + offset + subTarget.anchoredPosition3D; ;
                }
            }
            else
            {
                bFollow = false;
            }
        }
	}
}
