using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour {
    private static FuelManager instance;
    public static FuelManager Instance
    {
        get { return instance; }
    }

    public List<Bridge> bridgeList;

    void Awake()
    {
        instance = GetComponent<FuelManager>();
    }

    public void AllLock(bool bLock)
    {
        StartCoroutine(BridgeFlow(bLock));
    }

    IEnumerator BridgeFlow(bool bLock)
    {
        int count = bridgeList.Count;
        for (int i = 0; i < count; i++)
        {
            if (bridgeList[i].GetLock().CompareTo(bLock) != 0)
            {
                bridgeList[i].Lock(bLock);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public void BridgeLock(int index, bool bLock)
    {
        if (bridgeList[index].GetLock().CompareTo(bLock) != 0)
        {
            bridgeList[index].Lock(bLock);
        }
    }
}
