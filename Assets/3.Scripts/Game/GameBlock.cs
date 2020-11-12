using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameBlockType
{
    Color,
    Bit,
    Item,
    Gray
}
public enum GameBlockAni
{
    GameBlock_idle,
    GameBlock_fail
}
public class GameBlock : MonoBehaviour {
    public iVector3 pos;
    public GameBlockType type;
    public bool bDestroy = false;
    public Image image;
    public Rigidbody2D rd;
    public RectTransform rTr;
    public Animator ani;

    public float clearSpeed;
    public float pushPower;
    public bool bMove = false;
    public bool bBounce = true;
    public bool bIsParentPos = false;
    public float speed;
    public bool bByItemBlock = false;

    public virtual void Explosion(bool onff) { }
    public virtual void Drop() {
        if (!transform.parent.name.Contains("pos"))
        {
            bMove = true;
            return;
        }
        //Debug.Log(pos.ToString() + " : Drop");
        iVector3 down = pos.Down;
        if (rTr.anchoredPosition3D.sqrMagnitude > 1f)
        {
            bMove = true;
        }
        if (down.y.CompareTo(0) < 0) return;
        bool drop = false;
        if (BlockTools.ValidPos(down))
        {
            GameBlock gameBlock = InGameManager.Instance.GetGameBlock(down);
            if (gameBlock == null)
            {
                drop = true;
            }
            else
            {
                if (gameBlock.bMove)
                {
                    //drop = true;
                }
                if (gameBlock.bDestroy)
                {
                    drop = true;
                }
                if (!gameBlock.bMove && !gameBlock.bDestroy)
                {
                    if (!bBounce)
                    {
                        bBounce = true;
                        //Debug.Log("Bounce : " + pos.ToString());
                        Bounce();
                    }
                }
            }
        }
        if (drop)
        {
            pos = down;
            transform.SetParent(InGameManager.Instance.positionList[BlockTools.iVector3ToIndex(down)].transform);
            bMove = true;
        }
        
    }
    public virtual void Bounce()
    {
        BounceObject bounce = image.GetComponent<BounceObject>();
        if (bounce != null)
        {
            bounce.Bounce = true;
        }
    }
    public virtual void OnClickButton() { }
    public virtual void UpdateState() { }
    public virtual void SetAnimation(GameBlockAni animation) { }
    public virtual void SendUpBlockDropMessage() { }


    void FixedUpdate()
    {
        UpdateState();
        if (!InGameManager.Instance.bMove) return;
        if (bMove)
        {
            if (bIsParentPos)
            {
                rTr.anchoredPosition3D = Vector3.MoveTowards(rTr.anchoredPosition3D, Vector3.zero, Time.deltaTime * speed);
                if (rTr.anchoredPosition3D.sqrMagnitude < 1f)
                {
                    bMove = false;
                    Drop();
                }
            }
            else
            {
                rTr.anchoredPosition3D = Vector3.MoveTowards(rTr.anchoredPosition3D, new Vector3(rTr.anchoredPosition3D.x, -300f, 0f), Time.deltaTime * speed);
            }
        }
        else
        {
            Drop();
        }
    }
}
