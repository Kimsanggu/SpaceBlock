using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour {
    private static StarManager instance;
    public static StarManager Instance { get { return instance; } }

    public Transform fuelParent;
    public List<GameObject> fuelList;
    public List<int> starPoint;
    public List<GameObject> starList;
    public List<ParticleSystem> fx_FuelStarList;


    float totalCount = 100f;
    public float currentRate = 0f;
    float ratePerBlock = .2f;
    int star;
    void Awake()
    {
        instance = GetComponent<StarManager>();
    }

    public void Initialize()
    {
        star = 0;
        currentRate = 0f;
        currentRate = 0f;
        ratePerBlock = 100f / totalCount;
        LoadStarPoint();
        SetStar();
    }
    void LoadStarPoint()
    {
        starPoint.Add(MapManager.Instance.missionList[MapManager.Instance.Level - 1].star1);
        starPoint.Add(MapManager.Instance.missionList[MapManager.Instance.Level - 1].star2);
        starPoint.Add(MapManager.Instance.missionList[MapManager.Instance.Level - 1].star3);
    }
    public void AddBlock()
    {
        currentRate += ratePerBlock;
        UpdateUI();
    }
    public void AddBlock(int count)
    {
        currentRate += ratePerBlock * count;
        UpdateUI();
    }
    void SetStar()
    {
        int count = starPoint.Count;
        for (int i = 0; i < count; i++)
        {
            int index = starPoint[i] / 10;
            GameObject star = Instantiate(Resources.Load<GameObject>("Prefabs/FuelStar"));
            star.transform.SetParent(fuelParent);
            star.transform.localScale=Vector3.one;
            star.GetComponent<RectTransform>().anchoredPosition3D = fuelList[index-1].GetComponent<RectTransform>().anchoredPosition3D;
            fx_FuelStarList[i].gameObject.GetComponent<FollowUI>().subTarget = fuelList[index - 1].GetComponent<RectTransform>();
            starList.Add(star);
        }
    }
    bool IsStar(int index)
    {
        int count = starPoint.Count;
        for (int i = 0; i < count; i++)
        {
            if (index+1 == starPoint[i]/10)
            {
                return true;
            }
        }
        return false;
    }
    int GetFXIndex(int index)
    {
        int count = starPoint.Count;
        for (int i = 0; i < count; i++)
        {
            if (index + 1 == starPoint[i] / 10)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetStar()
    {
        return star;
    }
    void UpdateUI()
    {
        int count = (int)currentRate/10;
        
        for (int i = 0; i < count; i++)
        {
            if (i > 9) continue;

            if (!fuelList[i].activeInHierarchy)
            {
                if (IsStar(i))
                {
                    SoundManager.Instance.PlayEffect("eff_get_star", 0.3f);
                    int index = GetFXIndex(i);
                    if (index != -1)
                    {
                        fx_FuelStarList[index].gameObject.SetActive(true);
                    }

                }
            }
            fuelList[i].SetActive(true);
        }
        if (star < 3)
        {
            if (currentRate >= starPoint[star])
            {
                star++;
            }
        }
    }
}
