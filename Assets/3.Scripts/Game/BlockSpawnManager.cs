using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoBehaviour {
    private static BlockSpawnManager instance;
    public static BlockSpawnManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<BlockSpawner> list;
    public bool bStop = false;
    public bool bDelay = false;
    float delayTime = 0f;

    void Awake()
    {
        instance = GetComponent<BlockSpawnManager>();
    }

    public void AddSpawnList(GameBlockType type, iVector3 pos,string itemType=null)
    {
        list[pos.x].spawnList.Add(type);
    }
    public void MoveStop(float time=0.5f)
    {
        bStop = true;
        bDelay = true;
        delayTime = time;
    }
    public void MoveStart()
    {
        bStop = false;
        bDelay = false;
        delayTime = 0f;
    }
    void FixedUpdate()
    {
        if (bDelay)
        {
            if (delayTime > 0f)
            {
                delayTime -= Time.fixedDeltaTime;
            }
            else
            {
                bDelay = false;
                delayTime = 0f;
                MoveStart();
            }
        }
    }
}
