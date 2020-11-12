using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMissile : MonoBehaviour
{
    public RectTransform rTr;
    public Vector3 target;
    public float speed;
    public Vector3 initPos;
    GameObject fx = null;

    void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        rTr.anchoredPosition3D = initPos;
    }
    public void Setting()
    {
        
        //GameObject fx = Instantiate(Resources.Load<GameObject>("Prefabs/X_BombMissile"));
        Transform tr = PoolManager.Instance.GetPool(PoolType.Bomb_Missile);
        if (tr != null)
        {
            fx = tr.gameObject;
            fx.SetActive(true);
            fx.transform.SetParent(ParticleManager.Instance.particleCanvasF.transform);
            fx.transform.localScale = Vector3.one;
            fx.transform.localPosition = Vector3.zero;
            fx.GetComponent<FollowUI>().target = GetComponent<RectTransform>();
            fx.GetComponent<FollowUI>().subTarget = transform.parent.gameObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.Log("BombMissile empty" + PoolManager.Instance.poolList[(int)PoolType.Bomb_Missile].transform.childCount);
        }
    }
    public void Explosion()
    {
        StartCoroutine("Flow");
    }
    IEnumerator Flow()
    {
        while ((rTr.anchoredPosition3D - (target + initPos)).sqrMagnitude > 1f)
        {
            rTr.anchoredPosition3D = Vector3.Lerp(rTr.anchoredPosition3D, target + initPos, Time.deltaTime / 0.5f);
            yield return null;
        }
    }
    void OnDisable()
    {
        //rTr.anchoredPosition3D = initPos;
    }
}
