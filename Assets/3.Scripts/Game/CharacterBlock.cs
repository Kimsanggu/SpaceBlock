using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BitAnimation
{
    Bit_idle,
    Bit_smile,
    Bit_fail,
    Bit_talking,
    Bit_runleft,
    Bit_runright
}
public class CharacterBlock : GameBlock {
    public GameObject target;
    public List<BitAnimation> aniList;
    
    bool bClear = false;
    bool bBlock = true;

    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        SetAnimationList();
        StartCoroutine(CheckCharacter());
    }
    public override void Explosion(bool onff)
    {
        if (!bDestroy && !bClear && !bBlock)
        {
            bDestroy = true;
            StarManager.Instance.AddBlock();
            Debug.Log("BitDestroy");
            bClear = true;
            SoundManager.Instance.PlayEffect("eff_bit");
            InGameManager.Instance.Respawn(pos);
            StartCoroutine(DestroyEffect());
        }
    }
    IEnumerator CheckCharacter()
    {
        yield return null;
        float time = 0;
        float aniTime = 0f;
        float aniPlayTime=3f;
        while (!bDestroy)
        {
            aniTime += Time.deltaTime;
            if (IsClear())
            {
                time += Time.deltaTime;
                if (time > 1f)
                {
                    if (!bClear)
                    {
                        bBlock = false;
                        Explosion(true);
                        yield break;
                    }
                }
            }
            if (aniTime > aniPlayTime)
            {
                ani.Play(GetRandomAnimation().ToString());
                aniTime = 0f;
            }
            yield return null;
        }
    }
    IEnumerator DestroyEffect()
    {
        //BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        //int length = colliders.Length;
        //for (int i = 0; i < length; i++)
        //{
        //    colliders[i].enabled = false;
        //}
        Mission m = MissionManager.Instance.GetMission("Bit", 1);
        transform.SetParent(MissionManager.Instance.missionParentDummy.transform.Find("M" + m.index.ToString()));
        ani.Play("Bit_rotate");
        rd.isKinematic = true;
        while (rTr.anchoredPosition3D.sqrMagnitude > 4000f)
        {
            rTr.anchoredPosition3D = Vector3.Lerp(rTr.anchoredPosition3D, Vector3.zero, Time.deltaTime * clearSpeed);
            yield return null;
        }
        MissionManager.Instance.AddBlock("Bit", 1);
        MissionManager.Instance.BitBlink();
        DestroyObject();
        yield return null;
    }
    void DestroyObject()
    {
        //SendUpBlockDropMessage();
        transform.SetParent(PoolManager.Instance.poolList[(int)PoolType.CharacterBlock]);
        gameObject.SetActive(false);
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
    BitAnimation GetRandomAnimation()
    {
        BlockTools.Shuffle(aniList);
        BitAnimation animation = aniList[0];
        aniList.RemoveAt(0);
        if (aniList.Count.CompareTo(0) == 0)
        {
            SetAnimationList();
        }
        return animation;
    }
    bool IsClear()
    {
        if (pos.y.CompareTo(0) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void SetAnimationList()
    {
        aniList.Clear();
        aniList = new List<BitAnimation>();
        for (int i = 0; i < 6; i++)
        {
            aniList.Add((BitAnimation)i);
        }
        BlockTools.Shuffle(aniList);
    }
}
