using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BitAniController : MonoBehaviour
{

    ///////////////////////////////////////////////////////
    const float BIT_IDLE_TIME = 12f;

    ///////////////////////////////////////////////////////
    public GameObject[] HeadObj = new GameObject[2];
    public GameObject[] Eyes = new GameObject[2];
    public GameObject[] EyeDown = new GameObject[4];
    public GameObject[] EyeUp = new GameObject[4];
    public GameObject[] FireLeft = new GameObject[4];
    public GameObject[] FireRight = new GameObject[4];
    ///////////////////////////////////////////////////////

    public bool testMode = true;
    public bool UpTest = false;
    public bool FireUse = true;


    ///////////////////////////////////////////////////////
    public float FireSpeed = 0.3f;
    public Animator aniBit;

    ///////////////////////////////////////////////////////
    private float bitIdleTime = 0f;
    private bool bAniPlay = true;

    public bool AniPlay {
        get { return bAniPlay; }
        set { bAniPlay = value; }
    }

    ///////////////////////////////////////////////////////
    void Awake() {

        // 바닥 로켓 
        OnBitFireHideAll();

        if (FireUse) {
            FireLeft[0].SetActive(true);
            FireRight[0].SetActive(true);
        }

        // 머리 내리기 표정 웃음
        SetBitHead(false, 1);

        // 대기시간 : 0
        bitIdleTime = 0f;
    }

    void Start() {
        // 로켓 시작
        if (FireUse) StartCoroutine("coFireAni");
        // 대기 시간 시작
        StartCoroutine("coBitIdleTime");
    }


    //////////////////////////////////////////////////////////////////////////
    // Animation
    public float SetAnimation(string ani) {
        if (!AniPlay) return -1;
        if (aniBit == null) return -1;
        if (!aniBit.isActiveAndEnabled) return -1;

        SetFace(ani); // 표정바꾸기
        bitIdleTime = 0f;

        if (ani == "idle") {
            MobloTools.AnimationResetParameters(aniBit);
            return 0.0f;
        } else {
            MobloTools.AnimationResetParameters(aniBit);
            aniBit.SetTrigger(ani);
        }

        float aniTime = MobloTools.GetAniTime(aniBit, ani);
        return aniTime;
    }

    //////////////////////////////////////////////////////////////////////////
    // 표정 
    public void SetFace(string tag) {

        StopCoroutine("coBitFaceHi");

        switch (tag) {
            case "success":
                SetBitHead(true, (MobloTools.GetRandomNum(0, 100) % 2 == 0) ? 1 : 2);
                Invoke("OnBitHeadFree", 1.0f);
                break;
            case "good":
                SetBitHead(true, (MobloTools.GetRandomNum(0, 100) % 2 == 0) ? 1 : 2);
                Invoke("OnBitHeadFree", 1.0f);
                break;
            case "uoops":
            case "sadness":
                SoundManager.Instance.PlayEffect("Bit_sound_f", 2f);
                SetBitHead(true, 0);
                Invoke("OnBitHeadFree", 1.0f);
                break;
            case "uoops2":
                SoundManager.Instance.PlayEffect("Bit_sound_f", 2f);
                SetBitHead(true, 2);
                Invoke("OnBitHeadFree", 1.0f);
                break;
            case "sadnesslong":
                SoundManager.Instance.PlayEffect("Bit_sound_f", 2f);
                CancelInvoke("OnBitHeadFree");
                SetBitHead(true, 0);
                break;
            case "hi":
                SoundManager.Instance.PlayEffect("Bit_sound_bubble", 2f);
                StartCoroutine("coBitFaceHi");
                break;
            case "music":
            case "music2":
            case "music3":
                SetBitHead(true, 2);
                Invoke("OnBitHeadFree", 1.0f);
                break;
            case "haha":
                SetBitHead(true, 1);
                Invoke("OnBitHeadFree", 1.0f);
                break;

        }
    }

    //////////////////////////////////////////////////////////////////////////
    // 마우스 클릭 
    bool bDubleClick = false;
    void OnMouseDown() {
        if (bDubleClick) return;
        bDubleClick = true;

        StartCoroutine("coMouseDownAni");
    }

    //////////////////////////////////////////////////////////////////////////
    // 비트 바닥 로켓
    public void SetBitFire(bool Show) {
        if (Show) {
            FireLeft[0].SetActive(true);
            FireRight[0].SetActive(true);
            StartCoroutine("coFireAni");
        } else {
            StopCoroutine("coFireAni");
            OnBitFireHideAll();
        }
    }

    // 불 모두 감추기
    void OnBitFireHideAll() {
        for (int i = 0; i < FireLeft.Length; i++) {
            FireLeft[i].SetActive(false);
            FireRight[i].SetActive(false);
        }
    }

    // 비트 머리 (들기, 내리기)
    public void SetBitHead(bool up, int face) {

        if (up) {
            HeadObj[0].SetActive(false);
            HeadObj[1].SetActive(true);
            Eyes[0].SetActive(false);
            Eyes[1].SetActive(true);
        } else {
            HeadObj[0].SetActive(true);
            HeadObj[1].SetActive(false);
            Eyes[0].SetActive(true);
            Eyes[1].SetActive(false);
        }

        // 0:졸림, 1:웃음, 2:큰눈, 3:유지
        if (face < 3) {
            SetBitFace(face);
        }
    }

    // 비트 표정
    void SetBitFace(int eNum) {

        for (int i = 0; i < EyeDown.Length; i++) {
            EyeDown[i].SetActive(false);
            EyeUp[i].SetActive(false);
        }

        switch (eNum) {
            case 0:  // sleep
                EyeDown[0].SetActive(true);
                EyeUp[0].SetActive(true);
                break;
            case 1:  // smile
                EyeDown[1].SetActive(true);
                EyeUp[1].SetActive(true);
                break;
            case 2:  // open
                EyeDown[2].SetActive(true);
                EyeUp[2].SetActive(true);
                EyeDown[3].SetActive(true);
                EyeUp[3].SetActive(true);
                break;
        }

    }

    // 비트 발아래 불꽃
    IEnumerator coFireAni() {

        yield return new WaitForEndOfFrame();

        int setNum = 0;

        for (; ; ) {

            for (int i = 0; i < FireLeft.Length; i++) {
                if (setNum == i) {
                    FireLeft[i].SetActive(true);
                    FireRight[i].SetActive(true);
                } else {
                    FireLeft[i].SetActive(false);
                    FireRight[i].SetActive(false);
                }
            }

            yield return new WaitForSeconds(FireSpeed);
            //yield return new WaitForEndOfFrame();

            setNum++;
            if (setNum >= FireLeft.Length) setNum = 0;
        }

    }

    // 머리 자동 내리기
    void OnBitHeadFree() {
        SetBitHead(false, 1);
    }

    // 비트 하이 동작 중 머리 동작
    IEnumerator coBitFaceHi() {
        SetBitHead(true, 1);
        yield return new WaitForSeconds(0.3f);
        SetBitHead(false, 1);
        yield return new WaitForSeconds(0.3f);
        SetBitHead(true, 1);
        yield return new WaitForSeconds(0.3f);
        SetBitHead(false, 1);
    }

    // 비트 대기중 동작
    IEnumerator coBitIdleTime() {

        for (; ; ) {

            if (!AniPlay) {
                yield return new WaitForSeconds(1.2f);
                continue;
            }

            bitIdleTime += 1;

            if (bitIdleTime > BIT_IDLE_TIME) {
                bitIdleTime = 0;

                int rNum = MobloTools.GetRandomNum(0, 100) % 4;
                if (rNum == 0) {
                    SetAnimation("music");  // 왼쪽
                } else if (rNum == 1) {
                    SetAnimation("music3"); // 오른쪽
                } else if (rNum == 2) {
                    SetAnimation("music2"); // 양쪽
                } else {
                    SetAnimation("idle");   // 풀기
                }
            }

            yield return new WaitForSeconds(1.2f);
        }
    }

    IEnumerator coMouseDownAni() {

        bitIdleTime = 0;


        int rNum = MobloTools.GetRandomNum(0, 100) % 2;
        if (rNum == 0) {
            SetAnimation("uoops");
            yield return new WaitForSeconds(1.5f);
        } else if (rNum == 1) {
            SetAnimation("uoops2");
            yield return new WaitForSeconds(1.5f);
        } 

        rNum = MobloTools.GetRandomNum(0, 100) % 3;
        if (rNum == 0) {
            SetAnimation("music");  // 왼쪽
        } else if (rNum == 1) {
            SetAnimation("music3"); // 오른쪽
        } else {
            SetAnimation("music2"); // 양쪽
        }
        yield return new WaitForSeconds(0.5f);
        bDubleClick = false;
    }

    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Test Mode
    /// </summary>
    private bool PreFlag1;
    private bool PreFlag2;
    void Update() {

        if (!testMode) return;

        // 머리 올리고 내리기
        if (UpTest != PreFlag1) {
            if (UpTest) {
                SetBitHead(true, 3);
            } else {
                SetBitHead(false, 3);
            }
            PreFlag1 = UpTest;
        }

        // 불 On/Off
        if (FireUse != PreFlag2) {
            SetBitFire(FireUse);
            PreFlag2 = FireUse;
        }
    }

}

