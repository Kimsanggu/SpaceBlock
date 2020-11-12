using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum ColorType
{
    R,
    O,
    Y,
    G,
    B
}
public class ColorBlock : GameBlock
{
    public ColorType colorType;
    public List<ColorBlock> connectedColorBlockList = new List<ColorBlock>();
    public int colorCount;
    public Text txtCount;

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
    }


    public void Initialize()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        rd.isKinematic = false;
        bDestroy = false;
        bMove = false;
        connectedColorBlockList.Clear();
        SetImage(GetRandomColor());
        Drop();
    }

    public override void Explosion(bool onff = true)
    {
        if (!bDestroy)
        {
            if (!bByItemBlock)
            {
                SoundManager.Instance.PlayEffect("eff_crush1");
            }
            StarManager.Instance.AddBlock();
            bDestroy = true;
            InGameManager.Instance.RespawnColorBlock(new iVector3(pos.x, pos.y, pos.z));
            if (MissionManager.Instance.IsMissionBlock(colorType.ToString(), colorCount))
            {
                StartCoroutine(ExplosionEffect(colorType.ToString(), colorCount,pos));
            }

            GameObject fx_Block = PoolManager.Instance.GetPool(PoolType.FX_Block).gameObject;
            fx_Block.GetComponent<FX_Block>().Initialize(colorType);
            ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_Block, transform.parent.GetComponent<RectTransform>());

            GameObject fx_Smoke = PoolManager.Instance.GetPool(PoolType.FX_Smoke).gameObject;
            ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_Smoke, transform.parent.GetComponent<RectTransform>());

            if (onff)
            {
                FindGrayBlock();
            }
            DestroyObject();
        }
    }
    IEnumerator ExplosionEffect(string colorType,int colorCount,iVector3 p)
    {
        if (MissionManager.Instance.GetMission(colorType, colorCount).blockCount.CompareTo(1) == 0)
        {
            if (MissionManager.Instance.MissionClear())
            {
                MissionManager.Instance.bClear = true;
                MissionManager.Instance.MissionClearSendMessage();
            }
        }
        //MissionBlock missionBlock = Instantiate(Resources.Load<GameObject>("Prefabs/MissionBlock")).GetComponent<MissionBlock>();
        MissionBlock missionBlock = PoolManager.Instance.GetPool(PoolType.MissionBlock).gameObject.GetComponent<MissionBlock>();
        missionBlock.pos = p;
        missionBlock.colorType = colorType;
        missionBlock.colorCount = colorCount;
        missionBlock.gameObject.SetActive(true);
        yield return null;
    }



    public override void SetAnimation(GameBlockAni animation)
    {
        ani.Play(animation.ToString());
    }

    public override void OnClickButton()
    {
        //Debug.Log("OnClickButton");
        if (MissionManager.Instance.bClear) return;
        if (InGameManager.Instance.IsGameOver()) return;
        if (MissionManager.Instance.blockCount <= 0) return;


        if (colorCount >= 4)//아이템 만들기
        {
            MakeNumber(colorCount);
            MissionManager.Instance.UseBlock();
            for (int i = 0; i < colorCount; i++)
            {
                if (!connectedColorBlockList[i].pos.IsEquals(pos))
                {
                    connectedColorBlockList[i].Explosion(true);
                }
            }

            //ItemBlock itemBlock = Instantiate(Resources.Load<GameObject>("Prefabs/ItemBlock")).GetComponent<ItemBlock>();
            ItemBlock itemBlock = PoolManager.Instance.GetPool(PoolType.ItemBlock).gameObject.GetComponent<ItemBlock>();
            itemBlock.gameObject.SetActive(true);
            itemBlock.pos = pos;
            itemBlock.transform.SetParent(transform.parent);
            itemBlock.rTr.anchoredPosition3D = Vector3.zero;
            itemBlock.rTr.localScale = Vector3.one;
            itemBlock.bIsParentPos = true;
            
            switch (colorCount)
            {
                case 4:
                    itemBlock.itemType = (ItemType)UnityEngine.Random.Range(0, 2);

                    break;
                default:
                    itemBlock.itemType = ItemType.Bomb;
                    break;
            }
            MissionManager.Instance.AddBlock(colorType.ToString(), colorCount);
            MissionManager.Instance.AddBlock(itemBlock.itemType.ToString(), 1);

            itemBlock.Initialize();

            FindGrayBlock();

            DestroyObject();
        }
        else
        {
            if (colorCount.CompareTo(1) == 0)
            {
                //안터지고 애니메이션 실행
                Fail();
            }
            else
            {
                //MissionManager.Instance.AddBlock(colorType.ToString(), colorCount);
                MissionManager.Instance.UseBlock();
                for (int i = 0; i < colorCount; i++)
                {
                    connectedColorBlockList[i].Explosion(true);
                }
                MakeNumber(colorCount);
            }
        }
    }
    void MakeNumber(int colorCount)
    {
        GameObject number = Instantiate(Resources.Load<GameObject>("Prefabs/FX_Number")) as GameObject;
        number.transform.SetParent(ParticleManager.Instance.particleCanvasF.transform);
        number.transform.localPosition = Vector3.zero;
        number.transform.localScale = Vector3.one;
        number.gameObject.GetComponent<RectTransform>().anchoredPosition3D = InGameManager.Instance.positionList[BlockTools.iVector3ToIndex(pos)].gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        number.GetComponent<FX_Number>().value = colorCount;
        number.GetComponent<FX_Number>().Initialize();
    }
    public void Fail()
    {
        SoundManager.Instance.PlayEffect("eff_bad");
        SetAnimation(GameBlockAni.GameBlock_fail);
    }
    public override void UpdateState()
    {
        //check count
        //Debug.Log("UpdateState : " + pos.x + "," + pos.y);
        connectedColorBlockList.Clear();
        connectedColorBlockList.Add(this);
        colorCount = connectedColorBlockList.Count;
        SendCheckColorMessage();
    }
    List<ColorBlock> Find4Direction(iVector3 pos)
    {
        List<ColorBlock> list = new List<ColorBlock>();
        for (int i = 0; i < 4; i++)
        {
            iVector3 p = new iVector3();
            switch (i)
            {
                case 0: p = pos.Right; break;
                case 1: p = pos.Left; break;
                case 2: p = pos.Up; break;
                case 3: p = pos.Down; break;
            }
            if (BlockTools.ValidPos(p))
            {
                if (InGameManager.Instance != null)
                {
                    GameBlock gameBlock = InGameManager.Instance.GetGameBlock(p);
                    if (gameBlock != null)
                    {
                        ColorBlock colorBlock = gameBlock.GetComponent<ColorBlock>();
                        if (colorBlock != null)
                        {
                            if (colorBlock.colorType.CompareTo(colorType) == 0)
                            {
                                list.Add(colorBlock);
                            }
                        }
                    }
                }
            }
        }
        return list;
    }
    void SendCheckColorMessage()
    {
        List<ColorBlock> checkList = Find4Direction(pos);

        while (checkList.Count > 0)
        {
            List<ColorBlock> tempList = new List<ColorBlock>();
            int count = checkList.Count;
            for (int i = 0; i < count; i++)
            {
                if (!OverLap(checkList[i]))
                {
                    connectedColorBlockList.Add(checkList[i]);
                    List<ColorBlock> list = Find4Direction(checkList[i].pos);
                    if (list.Count > 0)
                    {
                        int listCount = list.Count;
                        for (int j = 0; j < listCount; j++)
                        {
                            if (!OverLap(list[j]))
                            {
                                tempList.Add(list[j]);
                            }
                        }
                    }
                }
            }
            checkList.Clear();
            checkList = tempList;
        }

        colorCount = connectedColorBlockList.Count;
        //connectedColorBlockList.Add(this);
    }
    bool OverLap(ColorBlock colorBlock)
    {
        int count = connectedColorBlockList.Count;
        for (int i = 0; i < count; i++)
        {
            if (connectedColorBlockList[i].pos.IsEquals(colorBlock.pos))
            {
                return true;
            }
        }
        return false;
    }
    ColorType GetRandomColor()
    {
        return (ColorType)(UnityEngine.Random.Range(0, 5));
    }
    void SetImage(ColorType colorType)
    {
        this.colorType = colorType;
        image.sprite = Resources.Load<Sprite>("Images/" + colorType.ToString());
    }
    void FindGrayBlock()
    {
        if (CheckGrayBlock(pos.Left))
        {
            GetGrayBlock(pos.Left).Explosion(true);
        }
        if (CheckGrayBlock(pos.Right))
        {
            GetGrayBlock(pos.Right).Explosion(true);
        }
        if (CheckGrayBlock(pos.Up))
        {
            GetGrayBlock(pos.Up).Explosion(true);
        }
        if (CheckGrayBlock(pos.Down))
        {
            GetGrayBlock(pos.Down).Explosion(true);
        }
    }
    GrayBlock GetGrayBlock(iVector3 position)
    {
        GrayBlock grayBlock = null;
        GameBlock gameBlock = InGameManager.Instance.GetGameBlock(position);
        if (gameBlock != null)
        {
            grayBlock = gameBlock.GetComponent<GrayBlock>();
        }
        return grayBlock;
    }
    bool CheckGrayBlock(iVector3 position)
    {
        if (!BlockTools.ValidPos(position)) return false;
        GameBlock gameBlock = InGameManager.Instance.GetGameBlock(position);
        if (gameBlock != null)
        {
            GrayBlock grayBlock = gameBlock.GetComponent<GrayBlock>();
            if (grayBlock != null)
            {
                return true;
            }
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.CompareTo("BottomLine") == 0)
        {
            Debug.Log(name + " : Bottom");
            bMove = false;
        }
        GameBlock gameBlock = coll.gameObject.GetComponent<GameBlock>();
        if (gameBlock != null)
        {
            //Debug.Log(name+" : Block");
            bMove = false;
        }
    }
    public override void SendUpBlockDropMessage()
    {
        iVector3 up = pos.Up;
        while (up.y <= 4)
        {
            if (BlockTools.ValidPos(up))
            {
                GameBlock gameBlock = InGameManager.Instance.GetGameBlock(up);
                if (gameBlock != null)
                {
                    //Debug.Log(gameBlock.transform.parent.name + " - Drop");
                    if (!gameBlock.bMove)
                    {
                        gameBlock.Drop();
                    }
                }
            }
            up = up.Up;
        }
    }
    public void DestroyObject()
    {
        transform.SetParent(PoolManager.Instance.poolList[(int)PoolType.ColorBlock]);
        bIsParentPos = false;
        gameObject.SetActive(false);
        if (!bByItemBlock)
        {
            //SendUpBlockDropMessage();
        }
        
    }
}
