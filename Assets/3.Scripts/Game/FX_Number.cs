using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FX_Number : MonoBehaviour {
    public TextUsedImage text;

    public int value;
    public float timeToLive;
    public float speed;
    public float alphaSpeed;
    public float scaleSpeed;
    public float targetScale;
    bool bStart = false;
    float time;

    void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        text.text = value.ToString();
        transform.localScale = Vector3.one;
        Transform tr = text.transform.Find("Content");
        int count = tr.childCount;
        for (int i = 0; i < count; i++)
        {
            Color col = tr.GetChild(i).gameObject.GetComponent<Image>().color;
            col.a = 1f;
            tr.GetChild(i).gameObject.GetComponent<Image>().color = col;
            tr.GetChild(i).gameObject.GetComponent<Image>().raycastTarget = false;
        }
        time = 0f;
        bStart = true;
    }
    void Update()
    {
        if (bStart)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * targetScale, Time.deltaTime * scaleSpeed);
            SetAlpha();
            if (time > timeToLive)
            {
                //BlockTools.Destroy(gameObject);
                transform.SetParent(InGameManager.Instance.GC);
                gameObject.SetActive(false);
            }
        }
    }
    void SetAlpha()
    {
        Transform tr = text.transform.Find("Content");
        int count = tr.childCount;
        for (int i = 0; i < count; i++)
        {
            Color col = tr.GetChild(i).gameObject.GetComponent<Image>().color;
            col.a = Mathf.MoveTowards(col.a, 0f, Time.deltaTime * alphaSpeed);
            tr.GetChild(i).gameObject.GetComponent<Image>().color = col;
        }
    }
}
