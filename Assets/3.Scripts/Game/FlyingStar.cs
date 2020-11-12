using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStar : MonoBehaviour {
    public Vector3 targetPos;
    public Vector3 initPos;
    public RectTransform starRTr;
    public GameObject starImage;
    public GameObject FX_SpreadStar;
    public GameObject trail;
    public float downSpeed;
    public float leftSpeed;
    public float rotationSpeed;

    public bool bMove = false;
    public bool bMoveLeft = false;
    public bool bMoveDown = false;

    IEnumerator Flow()
    {
        while (bMove)
        {
            starImage.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
            //downSpeed += 1f;
            if (bMoveLeft)
            {
                starRTr.transform.Translate(Vector3.left * Time.deltaTime * leftSpeed);
            }
            if (bMoveDown)
            {
                starRTr.transform.Translate(Vector3.down * Time.deltaTime * downSpeed);
            }
            if (starRTr.anchoredPosition3D.x < 0.1f)
            {
                starRTr.anchoredPosition3D = new Vector3(0f, starRTr.anchoredPosition3D.y, starRTr.anchoredPosition3D.z);
                bMoveLeft = false;
            }
            if (starRTr.anchoredPosition3D.y < 0.1f)
            {
                starRTr.anchoredPosition3D = new Vector3(starRTr.anchoredPosition3D.x, 0f, starRTr.anchoredPosition3D.z);
                bMoveDown = false;
            }
            if (!bMoveDown && !bMoveLeft)
            {
                bMove = false;
                starImage.transform.localRotation = Quaternion.Euler(Vector3.zero);
                FX_SpreadStar.SetActive(true);
                trail.SetActive(false);
                SoundManager.Instance.PlayEffect("eff_star", true);
                //spread particle
            }
            yield return null;
        }
    }
    public void Initialize()
    {
        starRTr.anchoredPosition3D = initPos;
        bMove = true;
        bMoveLeft = true;
        bMoveDown = true;
        trail.SetActive(true);
        StartCoroutine("Flow");
    }
}
