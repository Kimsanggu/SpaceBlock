using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct Mission
{
    public GameObject mObject;
    public string blockType;
    public int blockNo;
    public int blockCount;
    public Text txtCount;
    public int temp;
    public int index;
}
public class MissionManager : MonoBehaviour
{
    private static MissionManager instance;
    public static MissionManager Instance
    {
        get { return instance; }
    }
    public List<int> countList;

    public Transform missionParent;
    public Transform missionParentDummy;//UI용
    public List<Mission> missionList;

    public TextUsedImage txtBlockCount;
    public GameObject blockCountParent;
    public int blockCount;
    public GameObject bit;
    public bool bClear = false;

    void Awake()
    {
        instance = GetComponent<MissionManager>();
    }
    
    public void Initizlie()
    {
        bClear = false;
        blockCountParent.SetActive(true);
        ResetCountList();
        
        LoadMission();
        UpdateMissionUI();
        StartCoroutine("StartFlow");
    }
    IEnumerator StartFlow()
    {
        yield return new WaitForSeconds(1f);
        RectTransform panelRTr = missionParent.gameObject.GetComponent<RectTransform>();
        Vector3 targetPos = new Vector3(15f,0f,0f);//-450f
        float speed = 300f;
        while ((panelRTr.anchoredPosition3D - targetPos).sqrMagnitude > 1f)
        {
            panelRTr.anchoredPosition3D = Vector3.MoveTowards(panelRTr.anchoredPosition3D, targetPos, Time.deltaTime * speed);
            yield return null;
        }
        panelRTr.anchoredPosition3D = targetPos;
        FuelManager.Instance.AllLock(true);
    }
    IEnumerator EndFlow()
    {
        RectTransform panelRTr = missionParent.gameObject.GetComponent<RectTransform>();
        Vector3 targetPos = new Vector3(-350f, 0f, 0f);//-790f
        float speed = 300f;
        while ((panelRTr.anchoredPosition3D - targetPos).sqrMagnitude > 1f)
        {
            panelRTr.anchoredPosition3D = Vector3.MoveTowards(panelRTr.anchoredPosition3D, targetPos, Time.deltaTime * speed);
            yield return null;
        }
        panelRTr.anchoredPosition3D = targetPos;
    }
    public void UseBlock()
    {
        blockCount--;
        if (blockCount < 0) blockCount = 0;
        UpdateMissionUI();
        if (blockCount.CompareTo(0) == 0)
        {
            if (bClear) return;
            Invoke("CheckGameOver", 3f);
            
        }
    }
    void CheckGameOver()
    {
        if (MissionClear())
        {
            bClear = true;
            MissionPanelOff();
            ClearMessage();
        }
        else
        {
            InGameManager.Instance.GameOver();
        }
    }

    public void BitBlink()
    {
        bit.GetComponent<Animator>().Play("Missionitem_blink");
    }
    void LoadMission()
    {
        int stage = MapManager.Instance.Level;
        MissionData mission = MapManager.Instance.missionList[stage - 1];

        blockCount = mission.moveCount;

        missionList = new List<Mission>();
        if (mission.redCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "R";
            m.blockNo = mission.redNo;
            m.blockCount = mission.redCount;
            missionList.Add(m);
        }
        if (mission.orangeCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "O";
            m.blockNo = mission.orangeNo;
            m.blockCount = mission.orangeCount;
            missionList.Add(m);
        }
        if (mission.yellowCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "Y";
            m.blockNo = mission.yellowNo;
            m.blockCount = mission.yellowCount;
            missionList.Add(m);
        }
        if (mission.greenCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "G";
            m.blockNo = mission.greenNo;
            m.blockCount = mission.greenCount;
            missionList.Add(m);
        }
        if (mission.blueCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "B";
            m.blockNo = mission.blueNo;
            m.blockCount = mission.blueCount;
            missionList.Add(m);
        }
        if (mission.gray > 0)
        {
            Mission m = new Mission();
            m.blockType = "Gray";
            m.blockNo = 1;
            m.blockCount = mission.gray;
            missionList.Add(m);
        }
        if (mission.anyBlockCount > 0)
        {
            Mission m = new Mission();
            m.blockType = "AnyBlock";
            m.blockNo = mission.anyBlockNo;
            m.blockCount = mission.anyBlockCount;
            missionList.Add(m);
        }
        if (mission.bit > 0)
        {
            Mission m = new Mission();
            m.blockType = "Bit";
            m.blockNo = 1;
            m.blockCount = mission.bit;
            missionList.Add(m);
            
        }

        int missionCount = missionList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>(string.Format("Prefabs/Mission/" + missionList[i].blockType)));
            obj.transform.SetParent(missionParent);
            obj.transform.localScale = Vector3.one;
            Mission m = missionList[i];
            m.txtCount = obj.transform.Find("Text").gameObject.GetComponent<Text>();
            m.mObject = obj.transform.Find("Image").gameObject;
            m.index = i;
            missionList[i] = m;
            GameObject image = obj.transform.Find("Image").gameObject;
            if (image != null)
            {
                Transform txtobj = image.transform.Find("Text");
                if (txtobj != null)
                {
                    Text blockNo = txtobj.GetComponent<Text>();
                    if (m.blockNo.CompareTo(1) != 0)
                    {
                        blockNo.text = m.blockNo.ToString();
                    }
                }
            }
            if (missionList[i].blockType == "Bit")
            {
                bit = obj;
            }
            obj.GetComponent<RectTransform>().anchoredPosition3D = GetMissionUIPosition(i);
        }
    }
    Vector3 GetMissionUIPosition(int index)
    {
        Vector3 p = Vector3.one;
        switch (index)
        {
            case 0:
                p = new Vector3(-75f, 60f, 0f);
                break;
            case 1:
                p = new Vector3(35f, 60f, 0f);
                break;
            case 2:
                p = new Vector3(-75f, -32f, 0f);
                break;
            case 3:
                p = new Vector3(35f, -32f, 0f);
                break;
        }
        return p;
    }

    void ResetCountList()
    {
        countList.Clear();
        countList = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            countList.Add(0);
        }
    }
    public void UpdateBlockCount()
    {
        if (blockCount.CompareTo(5) == 0)
        {
            InGameManager.Instance.rocket.lightAni.speed = 2f;
        }
        txtBlockCount.text = blockCount.ToString();
    }
    public void Test()
    {
        Debug.Log(missionList[0].mObject.transform.childCount);
    }
    public void UpdateMissionUI()
    {
        UpdateBlockCount();
        int missionCount = missionList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            missionList[i].txtCount.text = missionList[i].blockCount.ToString();
            if (missionList[i].blockCount.CompareTo(0) == 0)
            {
                missionList[i].txtCount.text = "";
                if (!missionList[i].mObject.transform.parent.Find("Check").gameObject.activeInHierarchy)
                {
                    SoundManager.Instance.PlayEffect("eff_check");
                }
                missionList[i].mObject.transform.parent.Find("Check").gameObject.SetActive(true);
            }
        }
        if (MissionClear())
        {
            if (!bClear)
            {
                SoundManager.Instance.PlayEffect("Komiku_-_11_-_WIN");
                bClear = true;
                Invoke("MissionClearSendMessage",1f);
            }
        }
    }
    public void MissionPanelOff()
    {
        StartCoroutine("EndFlow");
    }
    public void MissionClearSendMessage()
    {
        MissionPanelOff();
        Invoke("ClearMessage", 2f);
    }
    void ClearMessage()
    {
        InGameManager.Instance.MissionClear();
    }
    public bool MissionClear()
    {
        int missionCount = missionList.Count;
        int clearCount = 0;
        for (int i = 0; i < missionCount; i++)
        {
            if (missionList[i].blockCount.CompareTo(0) == 0)
            {
                clearCount++;
            }
        }

        switch (missionCount)
        {
            case 1:
                if (clearCount.CompareTo(1) == 0)
                {
                    FuelManager.Instance.AllLock(false);
                }
                break;
            case 2:
                if (clearCount.CompareTo(1) == 0)
                {
                    FuelManager.Instance.BridgeLock(0, false);
                    FuelManager.Instance.BridgeLock(1, false);
                }
                else if (clearCount.CompareTo(2) == 0)
                {
                    FuelManager.Instance.AllLock(false);
                }
                break;
            case 3:
                switch (clearCount)
                {
                    case 1: FuelManager.Instance.BridgeLock(0, false);break;
                    case 2: FuelManager.Instance.BridgeLock(1, false); break;
                    case 3: FuelManager.Instance.BridgeLock(2, false); FuelManager.Instance.BridgeLock(3, false); break;
                }
                break;
            case 4:
                switch (clearCount)
                {
                    case 1: FuelManager.Instance.BridgeLock(0, false); break;
                    case 2: FuelManager.Instance.BridgeLock(1, false); break;
                    case 3: FuelManager.Instance.BridgeLock(2, false); break;
                    case 4: FuelManager.Instance.BridgeLock(3, false); break;
                }
                break;
        }

        if (clearCount.CompareTo(missionCount) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public bool IsMissionBlock(string type,int count)
    {
        int missionCount = missionList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            if (missionList[i].blockType.CompareTo(type) == 0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    if (missionList[i].blockCount > 0)
                    {
                        return true;
                    }
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    if (missionList[i].blockCount > 0)
                    {
                        return true;
                    }
                }
            }
            else if (missionList[i].blockType.CompareTo("AnyBlock")==0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    if (missionList[i].temp < missionList[i].blockNo)
                    {
                        if (missionList[i].blockCount > 0)
                        {
                            return true;
                        }
                    }
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    if (missionList[i].blockCount > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void AddBlock(string type, int count)
    {
        int missionCount = missionList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            if (missionList[i].blockType.CompareTo(type) == 0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    Mission m = missionList[i];
                    m.temp++;
                    if (m.temp.CompareTo(m.blockNo) == 0)
                    {
                        m.blockCount--;
                        m.temp = 0;
                    }
                    if (m.blockCount < 0) m.blockCount = 0;
                    missionList[i] = m;
                    break;
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    Mission m = missionList[i];
                    m.blockCount --;
                    if (m.blockCount < 0) m.blockCount = 0;
                    missionList[i] = m;
                    break;
                }
            }
            else if (missionList[i].blockType.CompareTo("AnyBlock")==0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    Mission m = missionList[i];
                    m.temp++;
                    if (m.temp.CompareTo(m.blockNo) == 0)
                    {
                        m.blockCount--;
                        m.temp = 0;
                    }
                    if (m.blockCount < 0) m.blockCount = 0;
                    missionList[i] = m;
                    break;
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    Mission m = missionList[i];
                    m.blockCount--;
                    if (m.blockCount < 0) m.blockCount = 0;
                    missionList[i] = m;
                    break;
                }
            }
        }
        switch (type)
        {
            case "R":
                countList[0] += count;
                break;
            case "O":
                countList[1] += count;
                break;
            case "Y":
                countList[2] += count;
                break;
            case "G":
                countList[3] += count;
                break;
            case "B":
                countList[4] += count;
                break;
            case "Bomb":
                countList[5] += count;
                break;
            case "Vertical":
                countList[6] += count;
                break;
            case "Horizental":
                countList[7] += count;
                break;
            case "Gray":
                countList[8] += count;
                break;
            case "Bit":
                countList[9] += count;
                break;
        }
        UpdateMissionUI();
    }
    public Mission GetMission(string type, int count)
    {
        Mission m = new Mission();
        int missionCount = missionList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            if (missionList[i].blockType.CompareTo(type) == 0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    m = missionList[i];
                    break;
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    m = missionList[i];
                    break;
                }
            }
            else if (missionList[i].blockType.CompareTo("AnyBlock")==0)
            {
                if (missionList[i].blockNo.CompareTo(count) == 0)
                {
                    if (missionList[i].temp < missionList[i].blockNo)
                    {
                        m = missionList[i];
                        break;
                    }
                }
                else if (missionList[i].blockNo.CompareTo(1) == 0)
                {
                    m = missionList[i];
                    break;
                }
            }
        }
        return m;
    }
}

