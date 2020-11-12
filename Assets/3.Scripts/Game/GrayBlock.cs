using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayBlock : GameBlock {
    public override void Explosion(bool onff)
    {
        if (!bDestroy)
        {
            SoundManager.Instance.PlayEffect("eff_crush2");
            StarManager.Instance.AddBlock();
            bDestroy = true;
            MissionManager.Instance.AddBlock("Gray",1);
            //ParticleManager.Instance.SetParticle(ParticleType.ParticleF, Particle.FX_Gray, transform.parent.GetComponent<RectTransform>());
            GameObject fx_Block = PoolManager.Instance.GetPool(PoolType.FX_Block).gameObject;
            fx_Block.GetComponent<FX_Block>().InitializeGray();
            ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_Block, transform.parent.GetComponent<RectTransform>());

            InGameManager.Instance.Respawn(pos);
            transform.SetParent(InGameManager.Instance.GC);
            gameObject.SetActive(false);
        }
    }
    public override void SetAnimation(GameBlockAni animation)
    {
        ani.Play(animation.ToString());
    }
    public override void OnClickButton()
    {
        SoundManager.Instance.PlayEffect("eff_bad");
        SetAnimation(GameBlockAni.GameBlock_fail);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
