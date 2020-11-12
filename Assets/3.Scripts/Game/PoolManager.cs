using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    ColorBlock,
    FX_Smoke,
    FX_LightBall_A,
    FX_SpreadStar,
    ItemBlock,
    Missile,
    Bomb_Missile,
    FX_Rocks,
    MissionBlock,
    CharacterBlock,
    FX_Block,
}
public class PoolManager : MonoBehaviour {
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get { return instance; }
    }


    public List<Transform > poolList;

	void Awake () {
        instance = GetComponent<PoolManager>();
	}
    public Transform GetPool(PoolType type)
    {
        Transform tr = null;
        int count = poolList[(int)type].childCount;
        for (int i = 0; i < count; i++)
        {
            if (!poolList[(int)type].GetChild(i).gameObject.activeInHierarchy)
            {
                tr = poolList[(int)type].GetChild(i);
            }
        }
        return tr;
    }
    GameObject GetPoolByTyp (PoolType type){
        GameObject obj = GetPool(type).gameObject;
        return obj;
    }

}

