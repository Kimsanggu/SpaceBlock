using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : MonoBehaviour {
    public int stage;
    public List<GameObject> starList;
    public GameObject lockObj;

	void Start () {
        stage = int.Parse(gameObject.name);
        lockObj = transform.Find("Lock").gameObject;
        starList.Add(transform.Find("Star/Star1").gameObject);
        starList.Add(transform.Find("Star/Star2").gameObject);
        starList.Add(transform.Find("Star/Star3").gameObject);

        SetLock();

        if (!lockObj.activeInHierarchy)
        {
            SetStarPosition(PlayerPrefs.GetInt(string.Format("Stage" + stage.ToString(), 0)));
        }
	}
    void SetLock()
    {
        if (MapManager.Instance.bDebug) return;
        if (stage > MapManager.Instance.MaxLevel)
        {
            lockObj.SetActive(true);
        }
    }
    void SetStarPosition(int starCount)
    {
        switch (starCount)
        {
            case 1:
                starList[0].SetActive(true);
                starList[0].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 150f, 0f);
                break;
            case 2:
                starList[0].SetActive(true);
                starList[0].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-32.5f, 150f, 0f);
                starList[1].SetActive(true);
                starList[1].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(32.5f, 150f, 0f);
                break;
            case 3:
                starList[0].SetActive(true);
                starList[0].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-65f, 150f, 0f);
                starList[1].SetActive(true);
                starList[1].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 150f, 0f);
                starList[2].SetActive(true);
                starList[2].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(65f, 150f, 0f);
                break;
        }
    }

    public void OnClickButton()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        if (lockObj.activeInHierarchy)
        {

        }
        else
        {
            MapManager.Instance.Level = stage;
            SceneController.Instance.MoveScene(SCENENAME.Game);
        }
    }
	
}
