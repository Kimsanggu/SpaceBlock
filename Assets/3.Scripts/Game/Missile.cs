using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}
public class Missile : MonoBehaviour
{
    public iVector3 pos;
    RectTransform rTr;
    public Direction dir;
    public Image image;
    public float speed;
    public bool bMove = false;
    bool isFinish = false;
    public int count = 0;
    void Awake()
    {
        rTr = GetComponent<RectTransform>();
    }
    public void SetMissile(Direction dir, bool isFinish)
    {
        this.isFinish = isFinish;
        this.dir = dir;
        bMove = true;
        switch (dir)
        {
            case Direction.Left:
                image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                count = pos.x;
                break;
            case Direction.Right:
                image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                count = 4-pos.x;
                break;
            case Direction.Up:
                image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                count = 4-pos.y;
                break;
            case Direction.Down:
                image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
                count = pos.y;
                break;
        }
    }
    void Update()
    {
        if (bMove)
        {
            Move();
        }
    }
    void Move()
    {
        Vector3 targetPos = Vector3.one;

        switch (dir)
        {
            case Direction.Left:
                targetPos = Vector3.left;
                break;
            case Direction.Right:
                targetPos = Vector3.right;
                break;
            case Direction.Up:
                targetPos = Vector3.up;
                break;
            case Direction.Down:
                targetPos = Vector3.down;
                break;
        }
        transform.Translate(targetPos * Time.deltaTime * speed);
        if (rTr.anchoredPosition3D.x + transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D.x < -375f ||
            rTr.anchoredPosition3D.x + transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D.x > 375f ||
            rTr.anchoredPosition3D.y + transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D.y > 375f ||
            rTr.anchoredPosition3D.y + transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D.y < -375f)
        {
            bMove = false;
            if (isFinish)
            {
                //BlockSpawnManager.Instance.MoveStart();
                //InGameManager.Instance.bMove = true;
            }
            DestroyFlow();
        }
    }
    void DestroyFlow()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        image.enabled = false;
        Invoke("DestroyObject", .2f);
    }
    public void DestroyObject()
    {
        transform.SetParent(PoolManager.Instance.poolList[(int)PoolType.Missile]);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        GameBlock gameBlock = coll.gameObject.GetComponent<GameBlock>();
        if (gameBlock != null)
        {
            if (!gameBlock.bDestroy && !gameBlock.pos.IsEquals(pos))
            {
                gameBlock.bByItemBlock = true;
                gameBlock.Explosion(false);
            }
            if (gameBlock.type == GameBlockType.Item)
            {
                if (!gameBlock.bDestroy)
                {
                    isFinish = false;
                    gameBlock.Explosion(false);
                }
            }
        }
    }
}

