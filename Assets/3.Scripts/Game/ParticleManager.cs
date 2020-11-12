using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Particle
{
    FX_R,
    FX_O,
    FX_Y,
    FX_G,
    FX_B,
    FX_Gray,
    FX_Smoke,
    FX_Number,
    FX_Rocks,
    FX_SpreadStar
}
public enum ParticleType
{
    ParticleF,
    ParticleB
}
public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;
    public static ParticleManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Transform particleCanvasF;
    public Transform particleCanvasB;
    public Transform particleCanvasF2;
    public Transform particleCanvasB2;

    void Awake()
    {
        instance = GetComponent<ParticleManager>();
    }

    public void SetParticle(ParticleType type, Particle particle, RectTransform rTr)
    {
        GameObject fx = Instantiate(Resources.Load<GameObject>(string.Format("Prefabs/" + particle.ToString())));
        Transform parent = null;
        switch (type)
        {
            case ParticleType.ParticleB: parent = particleCanvasB; break;
            case ParticleType.ParticleF: parent = particleCanvasF; break;
        }
        fx.transform.SetParent(parent);
        fx.transform.localScale = Vector3.one;
        fx.transform.localPosition = Vector3.zero;
        fx.GetComponent<RectTransform>().anchoredPosition3D = rTr.anchoredPosition3D;
        StartCoroutine(DestroyObject(fx, 2f));
    }
    IEnumerator DestroyObject(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        obj.transform.SetParent(InGameManager.Instance.GC);
        obj.SetActive(false);
    }
    public void SetParticle(ParticleType type, GameObject fx, RectTransform rTr)
    {
        Transform parent = null;
        switch (type)
        {
            case ParticleType.ParticleB: parent = particleCanvasB; break;
            case ParticleType.ParticleF: parent = particleCanvasF; break;
        }
        fx.transform.SetParent(parent);
        fx.transform.localScale = Vector3.one;
        fx.transform.localPosition = Vector3.zero;
        fx.GetComponent<RectTransform>().anchoredPosition3D = rTr.anchoredPosition3D;
        fx.SetActive(true);
    }
}
