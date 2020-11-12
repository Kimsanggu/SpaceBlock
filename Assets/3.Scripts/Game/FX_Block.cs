using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Block : MonoBehaviour {
    public List<Material> mList;
    public ParticleSystem fx;
    public void Initialize(ColorType type)
    {
        GetComponent<ParticleSystemRenderer>().material = mList[(int)type];
        fx.Play();
    }
    public void InitializeGray()
    {
        GetComponent<ParticleSystemRenderer>().material = mList[5];
        fx.Play();
    }
}
