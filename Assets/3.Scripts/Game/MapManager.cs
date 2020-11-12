using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// * : ColorBlock, G : GrayBlock
/// </summary>
[System.Serializable]
public struct Map
{
    public List<string> list;
}
[System.Serializable]
public struct MissionData
{
    public int level;
    public int moveCount;
    public int redCount;
    public int redNo;
    public int orangeCount;
    public int orangeNo;
    public int yellowCount;
    public int yellowNo;
    public int greenCount;
    public int greenNo;
    public int blueCount;
    public int blueNo;
    public int gray;
    public int anyBlockCount;
    public int anyBlockNo;
    public int bit;
    public int star1;
    public int star2;
    public int star3;
}
public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance
    {
        get
        {
            return instance;
        }
    }
    public List<TextAsset> mapDataList;
    public List<Map> mapList;

    public TextAsset missionData;
    public List<MissionData> missionList;

    public int bgm;
    public int effect;
    public bool bDebug;
    int currentLevel;
    public int maxLevel;

    public int MaxLevel
    {
        get
        {
            return maxLevel;
        }
        set
        {
            maxLevel = value;
        }
    }
    public int Level// 1 base
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }
    }

    void Awake()
    {
        instance = GetComponent<MapManager>();
        bgm = PlayerPrefs.GetInt("BGM", 1);
        effect = PlayerPrefs.GetInt("Effect", 1);
        maxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
    }

    void Start()
    {
        currentLevel = 1;
        LoadMap();
        LoadMission();
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("BGM", bgm);
        PlayerPrefs.SetInt("Effect", effect);
        if (!bDebug)
        {
            PlayerPrefs.SetInt("MaxLevel", maxLevel);
        }
    }
    void LoadMission()
    {
        missionList = new List<MissionData>();
        string[] lines = missionData.text.Split('\n');
        int lineCount = lines.Length;
        for (int i = 1; i < lineCount; i++)
        {
            MissionData mission = new MissionData();
            string[] splits = lines[i].Split(',');
            mission.level = int.Parse(splits[0]);
            mission.moveCount = int.Parse(splits[1]);
            mission.redCount = int.Parse(splits[2]);
            mission.redNo = int.Parse(splits[3]);
            mission.orangeCount = int.Parse(splits[4]);
            mission.orangeNo = int.Parse(splits[5]);
            mission.yellowCount = int.Parse(splits[6]);
            mission.yellowNo = int.Parse(splits[7]);
            mission.greenCount = int.Parse(splits[8]);
            mission.greenNo = int.Parse(splits[9]);
            mission.blueCount = int.Parse(splits[10]);
            mission.blueNo = int.Parse(splits[11]);
            mission.gray = int.Parse(splits[12]);
            mission.anyBlockCount = int.Parse(splits[13]);
            mission.anyBlockNo = int.Parse(splits[14]);
            mission.bit = int.Parse(splits[15]);
            mission.star1 = int.Parse(splits[16]);
            mission.star2 = int.Parse(splits[17]);
            mission.star3 = int.Parse(splits[18]);

            missionList.Add(mission);
        }
    }
    void LoadMap()
    {
        mapList = new List<Map>();
        for (int i = 0; i < 25; i++)
        {
            mapDataList.Add(Resources.Load<TextAsset>(string.Format("Map/" + (i + 1).ToString())));
        }
        int mapCount = mapDataList.Count;
        for (int i = 0; i < mapCount; i++)
        {
            Map map = new Map();
            map.list = new List<string>();
            string[] lines = mapDataList[i].text.Split('\n');
            int lineCount = lines.Length;
            for (int j = lineCount - 1; j >= 0; j--)
            {
                string[] splits = lines[j].Split(',');
                int splitsCount = splits.Length;
                for (int k = 0; k < splitsCount; k++)
                {

                    map.list.Add(splits[k].Trim());
                }
            }
            mapList.Add(map);
        }
    }
    public int BGM//1 on 0 off
    {
        get { return bgm; }
        set { bgm = value; }
    }
    public int Effect//1 on 0 off
    {
        get { return effect; }
        set { effect = value; }
    }
    public void OnClickButtonBGM(bool onff)
    {
        if (onff)
        {
            BGM = 1;
        }
        else
        {
            BGM = 0;
        }
    }
    public void OnClickButtonEffect(bool onff)
    {
        if (onff)
        {
            Effect = 1;
        }
        else
        {
            Effect = 0;
        }
    }
    public void SaveStage()
    {
        if (currentLevel == maxLevel)
        {
            maxLevel = currentLevel + 1;
        }
    }
}
