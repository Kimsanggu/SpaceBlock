using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour {
    bool bBounce = false;
    public float initDIstance;
    public float decreasePower;
    public float moveSpeed;

    RectTransform rTr;
    public float height;
    public float value = 0f;
    float time = 0f;
    bool bEnd = true;
    
    
    void Awake()
    {
        rTr = GetComponent<RectTransform>();
    }
    public bool Bounce
    {
        get
        {
            return bBounce;
        }
        set
        {
            bBounce = value;
            if (value)
            {
                StartCoroutine(BounceFlow());
            }
        }
    }
    IEnumerator BounceFlow()
    {
        if(bBounce)
        {
            bBounce = false;
            bEnd = false;
            time = 0f;
            height = initDIstance;
        }
        while (!bEnd)
        {
            time += Time.deltaTime * moveSpeed;
            yield return null;
            height -= decreasePower * Time.deltaTime;
            value = Mathf.Sin(time) * height;
            rTr.anchoredPosition3D = new Vector3(rTr.anchoredPosition3D.x, value, rTr.anchoredPosition3D.z);
            if (height < 0f)
            {
                bEnd = true;
            }
        }
    }
}
