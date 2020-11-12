using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBlock : MonoBehaviour
{
    public iVector3 pos;
    public Transform target;
    public string colorType;
    public int colorCount;

    RectTransform rTr;
    Rigidbody2D rd;
    Mission mission;
    float flyingSpeed = 1500f;
    float scaleSpeed = 200f;
    float rotateSpeed = 360f;
    int index = -1;

    void OnEnable()
    {
        //gameObject.SetActive(true);
        transform.SetParent(InGameManager.Instance.positionList[BlockTools.iVector3ToIndex(pos)].transform);
        rd = GetComponent<Rigidbody2D>();
        rTr = gameObject.GetComponent<RectTransform>();
        rTr.anchoredPosition3D = Vector3.zero;
        rTr.localScale = Vector3.one;
        rTr.sizeDelta = new Vector2(150f, 150f);
        rTr.rotation = Quaternion.Euler(Vector3.zero);
        //transform.SetParent(target);
        index = 0;
        rd.gravityScale = 30f;
        mission = MissionManager.Instance.GetMission(colorType, colorCount);
        target = MissionManager.Instance.missionParentDummy.transform.Find(string.Format("M" + mission.index.ToString()));
        GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Images/" + colorType));
    }
    void Update()
    {
        transform.SetAsLastSibling();
        switch (index)
        {
            case 0:
                if (rTr.anchoredPosition3D.y < -100f)
                {
                    rd.gravityScale = 0f;
                    index = 1;
                }
                break;
            case 1:
                rd.velocity = Vector2.zero;
                transform.SetParent(target);
                rTr.anchoredPosition3D = Vector3.MoveTowards(rTr.anchoredPosition3D, Vector3.zero, Time.deltaTime * flyingSpeed);
                rTr.sizeDelta = Vector2.MoveTowards(rTr.sizeDelta, new Vector2(50f, 50f), Time.deltaTime * scaleSpeed);
                if (rTr.anchoredPosition3D.sqrMagnitude < 500f)
                {
                    index = 2;
                }
                break;
            case 2:
                index = -1;
                mission.mObject.transform.parent.gameObject.GetComponent<Animator>().Play("Missionitem_blink");
                MissionManager.Instance.AddBlock(colorType,colorCount);
                DestroyObject();
                break;
        }
        if (index > 0)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);

        }
    }
    public void DestroyObject()
    {
        transform.SetParent(PoolManager.Instance.poolList[(int)PoolType.MissionBlock]);
        gameObject.SetActive(false);
    }
}