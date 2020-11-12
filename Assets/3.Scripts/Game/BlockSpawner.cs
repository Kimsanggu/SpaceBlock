using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public List<GameBlockType> spawnList;
    public bool bSpawn = false;
    float time = 0f;
    float checkTime = 0f;
    float intervalTime = 1f;
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (!bSpawn && spawnList.Count > 0 && !BlockSpawnManager.Instance.bStop)
        {
            Spawn();
        }
    }
    void Spawn()
    {
        bSpawn = true;
        GameBlock prefab = null;
        switch (spawnList[0])
        {
            case GameBlockType.Bit:
                //prefab = Instantiate(Resources.Load<GameObject>("Prefabs/BitBlock")).GetComponent<GameBlock>();
                prefab = PoolManager.Instance.GetPool(PoolType.CharacterBlock).gameObject.GetComponent<GameBlock>();
                break;
            case GameBlockType.Color:
                //prefab = Instantiate(Resources.Load<GameObject>("Prefabs/ColorBlock")).GetComponent<GameBlock>();
                Transform tr = PoolManager.Instance.GetPool(PoolType.ColorBlock);
                if (tr != null)
                {
                    prefab = tr.gameObject.GetComponent<GameBlock>();
                }
                else
                {
                    Debug.Log("pool is empty");
                    return;
                }
                break;
            case GameBlockType.Item:
                
                prefab = Instantiate(Resources.Load<GameObject>("Prefabs/ItemBlock")).GetComponent<GameBlock>();
                prefab.GetComponent<ItemBlock>().itemType = ItemType.Bomb;
                prefab.GetComponent<ItemBlock>().Initialize();
                break;
            case GameBlockType.Gray:
                prefab = Instantiate(Resources.Load<GameObject>("Prefabs/GrayBlock")).GetComponent<GameBlock>();
                break;
        }

        prefab.transform.SetParent(transform);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.localScale = Vector3.one;
        switch (spawnList[0])
        {
            case GameBlockType.Bit:
                CharacterBlock chBlock = prefab.GetComponent<CharacterBlock>();
                    if (chBlock != null)
                    {
                        chBlock.target = MissionManager.Instance.bit;
                    }
                break;
            case GameBlockType.Color:
                ColorBlock colorBlock = prefab.GetComponent<ColorBlock>();

                if (colorBlock != null)
                {
                    colorBlock.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                    colorBlock.GetComponent<RectTransform>().localScale = Vector3.one;
                    colorBlock.Initialize();
                    colorBlock.bMove = true;
                    colorBlock.bBounce = false;
                    colorBlock.bByItemBlock = false;
                    if (Mathf.Abs(time - checkTime) > intervalTime)//나오는거 아래꺼 하나만 바운스할때
                    {
                        checkTime = time;
                    }
                }
                break;
            case GameBlockType.Item:
                break;
            case GameBlockType.Gray:
                break;
        }
        prefab.gameObject.SetActive(true);
        spawnList.RemoveAt(0);
        //Debug.Log("Respawn : " + colorBlock.pos);
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        bSpawn = false;
    }
}
