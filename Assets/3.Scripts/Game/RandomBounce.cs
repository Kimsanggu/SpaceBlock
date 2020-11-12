using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBounce : MonoBehaviour {
    public iVector2 size;
    public int count;
    public float time;
    public bool bBounce = false;
    RectTransform rTr;
    Vector3 initPos;
    

    void Awake()
    {
        rTr = GetComponent<RectTransform>();
        initPos = rTr.anchoredPosition3D;
    }
    void Update()
    {
        if (bBounce)
        {
            Bounce();
            bBounce = false;
        }
    }
    public void Bounce()
    {
        StartCoroutine(Flow());
    }
    IEnumerator Flow()
    {
        int c = 0;
        while (c < count)
        {
            Vector3 pos = new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), 0f);
            rTr.anchoredPosition3D = initPos + pos;
            yield return new WaitForSeconds(time);
            rTr.anchoredPosition3D = initPos;
            yield return new WaitForSeconds(time);
            c++;
        }
    }
    
}
