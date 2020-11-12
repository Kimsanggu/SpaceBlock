using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Horizental,
    Vertical,
    Bomb,
}
public class ItemBlock : GameBlock
{
    public ItemType itemType;
    public List<BombMissile> missileList;
    public void Initialize()
    {
        bDestroy = false;
        SetImage();
    }
    void SetImage()
    {
        image.sprite = Resources.Load<Sprite>(string.Format("Images/" + itemType.ToString()));
        //Debug.Log("sprite : " + image.sprite.name);
        if (ani.gameObject.activeInHierarchy)
        {
            ani.Play(string.Format("ItemBlock_" + itemType.ToString()));
        }
        switch (itemType)
        {
            case ItemType.Horizental:
                image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(132f, 42.5f);
                break;
            case ItemType.Vertical:
                image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(42.5f, 132f);
                break;
            case ItemType.Bomb:
                image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 90f);
                transform.Find("Bomb").gameObject.SetActive(true);
                ResetMissile();
                break;
        }
        image.enabled = true;
    }
    void ResetMissile()
    {
        int count = missileList.Count;
        for (int i = 0; i < count; i++)
        {
            missileList[i].Init();
        }
    }
    public override void OnClickButton()
    {
        if (MissionManager.Instance.bClear) return;
        if (InGameManager.Instance.IsGameOver()) return;
        if (MissionManager.Instance.blockCount <= 0) return;
        MissionManager.Instance.UseBlock();
        Explosion(true);
    }
    public override void Explosion(bool onff)
    {
        if (bDestroy) { return; }
        
        bDestroy = true;
        GetComponent<BoxCollider2D>().enabled = false;

        switch (itemType)
        {
            case ItemType.Horizental:
                StarManager.Instance.AddBlock(1);
                InGameManager.Instance.MoveStop(.4f);
                BlockSpawnManager.Instance.MoveStop(.4f);
                bool left = false;
                bool right = false;
                if (pos.x == 2)
                {
                    right = true;
                }
                else if (pos.x < 2)
                {
                    right = true;
                }
                else
                {
                    left = true;
                }
                SoundManager.Instance.PlayEffect("eff_whoosh");
                CreateMissile(Direction.Left, left);
                CreateMissile(Direction.Right, right);
                GameObject fx_RocksH = PoolManager.Instance.GetPool(PoolType.FX_Rocks).gameObject;
                ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_RocksH, rTr.transform.parent.gameObject.GetComponent<RectTransform>());
                break;
            case ItemType.Vertical:
                StarManager.Instance.AddBlock(1);
                InGameManager.Instance.MoveStop(.4f);
                BlockSpawnManager.Instance.MoveStop(.4f);
                bool up = false;
                bool down = false;
                if (pos.y == 2)
                {
                    up = true;
                }
                else if (pos.y < 2)
                {
                    up = true;
                }
                else
                {
                    down = true;
                }
                SoundManager.Instance.PlayEffect("eff_whoosh");
                CreateMissile(Direction.Up, up);
                CreateMissile(Direction.Down, down);
                GameObject fx_RocksV = PoolManager.Instance.GetPool(PoolType.FX_Rocks).gameObject;
                ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_RocksV, rTr.transform.parent.gameObject.GetComponent<RectTransform>());
                break;
            case ItemType.Bomb:
                StarManager.Instance.AddBlock(2);
                InGameManager.Instance.MoveStop(1f);
                BlockSpawnManager.Instance.MoveStop(1f);
                StartCoroutine("ExplosionFlow");
                return;
        }
        MissionManager.Instance.AddBlock(itemType.ToString(), 1);
        InGameManager.Instance.Respawn(pos);
        DestroyObject();
    }
    IEnumerator ExplosionFlow()
    {
        SoundManager.Instance.PlayEffect("eff_item_whoosh2");
        GetComponent<Animator>().Play("ItemBlock_bombexplosion");
        yield return new WaitForSeconds(0.416f);
        SoundManager.Instance.PlayEffect("eff_whoosh");
        image.enabled = false;
        //ParticleManager.Instance.SetParticle(ParticleType.ParticleF, Particle.FX_Rocks, rTr.transform.parent.gameObject.GetComponent<RectTransform>());
        GameObject fx_Rocks = PoolManager.Instance.GetPool(PoolType.FX_Rocks).gameObject;
        ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_Rocks, rTr.transform.parent.gameObject.GetComponent<RectTransform>());

        int count = missileList.Count;
        GameObject bomb = transform.Find("Bomb").gameObject;
        bomb.transform.SetParent(InGameManager.Instance.uiParent.transform);
        for (int i = 0; i < count; i++)
        {
            missileList[i].Setting();
            missileList[i].Explosion();
        }
        bool isFinish = true;
        yield return new WaitForSeconds(0.416f);
        for (int y = pos.y - 1; y < pos.y + 2; y++)
        {
            for (int x = pos.x - 1; x < pos.x + 2; x++)
            {
                if (BlockTools.ValidPos(new iVector3(x, y, 0)))
                {
                    if (pos.IsEquals(new iVector3(x, y, 0)))
                    {
                        continue;
                    }
                    GameBlock gameBlock = InGameManager.Instance.GetGameBlock(new iVector3(x, y, 0));
                    if (gameBlock != null)
                    {
                        if (gameBlock.type == GameBlockType.Item)
                        {
                            if (!gameBlock.bDestroy)
                            {
                                isFinish = false;
                            }
                        }
                        gameBlock.Explosion(false);
                    }
                }
            }
        }
        bomb.transform.SetParent(transform);
        bomb.transform.SetAsFirstSibling();
        bomb.transform.localPosition = Vector3.zero;
        bomb.SetActive(false);
        if (isFinish)
        {
            //BlockSpawnManager.Instance.MoveStart();
            //InGameManager.Instance.MoveStart();
        }

        MissionManager.Instance.AddBlock(itemType.ToString(), 1);
        InGameManager.Instance.Respawn(pos);
        DestroyObject();
    }
    void CreateMissile(Direction dir, bool bmove)
    {
        //Missile missile = Instantiate(Resources.Load<GameObject>("Prefabs/Missile")).GetComponent<Missile>();
        Missile missile = PoolManager.Instance.GetPool(PoolType.Missile).GetComponent<Missile>();
        missile.gameObject.SetActive(true);
        missile.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        missile.image.enabled = true;
        missile.transform.SetParent(transform.parent);
        missile.transform.localPosition = Vector3.zero;
        missile.transform.localScale = Vector3.one;
        missile.pos = pos;
        missile.SetMissile(dir, bmove);
    }
    public void DestroyObject()
    {
        transform.SetParent(PoolManager.Instance.poolList[(int)PoolType.ItemBlock]);
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
