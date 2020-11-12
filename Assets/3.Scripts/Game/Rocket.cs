using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    public float speed;
    public BounceObject bounce;
    public Animator ani_Fire;
    public GameObject fx_Smoke;
    public Animator lightAni;
    public RandomBounce randomBounce;

    public void Fire(int type)
    {
        switch (type)
        {
            case 0:
                ani_Fire.Play("RocketFire_idle");
                fx_Smoke.SetActive(false);
                SoundManager.Instance.StopEffect(SoundName.eff_flame);
                break;
            case 1:
                SoundManager.Instance.PlayEffect(SoundName.eff_flame,1f,true);
                ani_Fire.Play("RocketFire_fire1");
                fx_Smoke.SetActive(true);
                StartCoroutine(FireSound());
                break;
            case 2:
                ani_Fire.Play("RocketFire_fire2");
                fx_Smoke.SetActive(true);
                break;
        }
    }
    public void Bounce()
    {
        bounce.Bounce = true;
    }
    IEnumerator FireSound()
    {
        Transform tr = SoundManager.Instance.transform.Find("(Audio) eff_flame");
        yield return new WaitForSeconds(1f);
        if (tr != null)
        {
            AudioSource source = tr.GetComponent<AudioSource>();
            while (source.volume > 0.1f)
            {
                source.volume = Mathf.MoveTowards(source.volume, 0f, Time.deltaTime);
                yield return null;
            }
        }
    }
}
