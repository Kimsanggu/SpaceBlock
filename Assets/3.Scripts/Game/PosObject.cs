using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosObject : MonoBehaviour
{
    public iVector3 pos;
    public float time = 0f;
    public float power;
    void Start()
    {
        pos = new iVector3(int.Parse(name.Split('_')[1]), int.Parse(name.Split('_')[2]), 0);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        GameBlock gameBlock = coll.gameObject.GetComponent<GameBlock>();
        if (gameBlock != null)
        {
            if (gameBlock.bDestroy) return;
            //if (gameBlock.pos.x.CompareTo(pos.x) != 0) { return; }
            gameBlock.transform.SetParent(transform);
            gameBlock.pos = pos;
            if (pos.y.CompareTo(4) == 0)
            {
                gameBlock.bIsParentPos = true;
            }
        }
        time = 0f;
    }
}
