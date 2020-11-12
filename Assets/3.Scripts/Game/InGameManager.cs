using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Info,
    Game,
    Fail,
    Success
}
public enum RocketBit
{
    RocketBit_idle,
    RocketBit_happy,
    RocketBit_sad,
    RocketBit_happy_loop,
    RocketBit_sad_loop
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<GameObject> positionList;
    public GameObject boardParent;
    public GameObject btn_Exit;
    public Rocket rocket;
    public GameObject fuel;
    public GameObject uiParent;
    public ParticleSystem FX_Electric;
    public GameObject FX_Electric_Bridge;
    public GameObject FX_Smoke_Black;
    public Transform GC;
    bool bGameOver = true;
    bool bGameClear = true;

    public bool bMove = true;

    public bool bDelay = false;
    public float delayTime = 0f;

    public Animator rocketBit;

    void Awake()
    {
        instance = GetComponent<InGameManager>();
    }
    void Start()
    {
        if (SceneController.Instance != null)
        {
            SceneController.Instance.StopLoading();
        }
        SoundManager.Instance.PlayFlag = true;
        SoundManager.Instance.PlayBGM(1);
        SoundManager.Instance.PlayBGM(.3f);
        StartCoroutine("StartFlow");
        //StartCoroutine(Auto());
    }
    IEnumerator StartFlow()
    {
        yield return null;
        SetRocketBit(RocketBit.RocketBit_happy_loop);
        RectTransform rTr = rocket.gameObject.GetComponent<RectTransform>();
        rocket.Fire(1);
        while (rTr.anchoredPosition3D.sqrMagnitude > 10f)
        {
            rTr.anchoredPosition3D = Vector3.Lerp(rTr.anchoredPosition3D, Vector3.zero, Time.deltaTime * rocket.speed);
            yield return null;
        }
        SoundManager.Instance.PlayEffect("eff_rocket_stop");
        SetRocketBit(RocketBit.RocketBit_idle);
        rocket.Bounce();
        rocket.Fire(0);
        
        yield return new WaitForSeconds(0.5f);
        Initialize();
    }
    void Initialize()
    {
        bGameOver = false;
        bGameClear = false;
        MissionManager.Instance.Initizlie();
        LoadPosition();
        LoadBlocks();
        StarManager.Instance.Initialize();
        StartCoroutine(CheckMixFlow());
        StartCoroutine(CheckDelayTime());
    }
    IEnumerator CheckMixFlow()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1f);
            if (CheckMix())
            {
                MIx();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            if (bGameClear) yield break;
            if (bGameOver) yield break;
        }
    }
    IEnumerator CheckDelayTime()
    {
        while (gameObject.activeInHierarchy)
        {
            if (bDelay)
            {
                if (delayTime > 0f)
                {
                    delayTime -= Time.fixedDeltaTime;
                }
                else
                {
                    bDelay = false;
                    delayTime = 0f;
                    MoveStart();
                }   
            }
            yield return null;
        }
    }
   
    void LoadPosition()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                positionList.Add(boardParent.transform.Find(string.Format("pos_" + x.ToString() + "_" + y.ToString())).gameObject);
            }
        }
    }
    void LoadBlocks()
    {
        int count = positionList.Count;
        Map map = MapManager.Instance.mapList[MapManager.Instance.Level - 1];
        for (int i = 0; i < count; i++)
        {
            switch (map.list[i])//전처리
            {
                case "*":
                    BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Color, BlockTools.IndexToiVector3(i));
                    break;
                case "G":
                    BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Gray, BlockTools.IndexToiVector3(i));
                    break;
                case "Bit":
                    BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Bit, BlockTools.IndexToiVector3(i));
                    break;
                case "V":
                case "H":
                case "Bomb":
                    BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Item, BlockTools.IndexToiVector3(i), map.list[i]);
                    //BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Item, BlockTools.IndexToiVector3(i));
                    break;
                default:
                    break;
            }
        }
    }
    public GameBlock GetGameBlock(iVector3 pos)
    {
        GameBlock gameBlock = null;
        if (pos.x >= 0 && pos.y <= 4 && pos.z == 0)
        {
            if (positionList != null && positionList.Count != 0)
            {
                gameBlock = positionList[BlockTools.iVector3ToIndex(pos)].GetComponentInChildren<GameBlock>();
            }
        }
        return gameBlock;
    }
    public void RespawnColorBlock(iVector3 pos)
    {
        BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Color, pos);
    }
    public void Respawn(iVector3 pos)
    {
        BlockSpawnManager.Instance.AddSpawnList(GameBlockType.Color, pos);
    }
    bool CheckMix()
    {
        int counting = 0;
        int totalCount = 0;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                GameBlock gameBlock = GetGameBlock(new iVector3(x, y, 0));
                ColorBlock ColorBlock = null;
                if (gameBlock != null)
                {
                    totalCount++;
                    ColorBlock = gameBlock.GetComponent<ColorBlock>();
                }
                if (ColorBlock != null)
                {
                    if (ColorBlock.colorCount > 1)
                    {
                        counting++;
                    }
                }
            }
        }
        if (totalCount.CompareTo(25) != 0)
        {
            return false;
        }
        if (counting.CompareTo(0) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void MIx()
    {
        Debug.Log("Mix");
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                GameBlock gameBlock = GetGameBlock(new iVector3(x, y, 0));
                if (gameBlock != null)
                {
                    ColorBlock ColorBlock = gameBlock.GetComponent<ColorBlock>();
                    if (ColorBlock != null)
                    {
                        ColorBlock.Initialize();
                    }
                }
            }
        }
    }
    public void GameOver()
    {
        if (MissionManager.Instance.bClear) return;
        if (bGameClear) return;
        if (!bGameOver)
        {
            SoundManager.Instance.PlayEffect("Monplaisir_-_09_-_Defeat");
            bGameOver = true;
            //panel_GameOver.SetActive(true);
            StartCoroutine(ClearEffect(false));
        }
    }
    public void OnClickButtonRetry()
    {
        SoundManager.Instance.PlayEffect("eff_button");
        SceneController.Instance.MoveScene(SCENENAME.Game);
    }
    IEnumerator Auto()//test
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            int count = positionList.Count;
            List<int> index = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                index.Add(i);
            }
            BlockTools.Shuffle(index);
            int maxCount = 0;
            int maxIndex = -1;
            int missionIndex = -1;
            int missionMaxCount = 0;
            int itemIndex = -1;
            for (int i = 0; i < count; i++)
            {
                GameBlock gameBlock = positionList[index[i]].GetComponentInChildren<GameBlock>();
                if (gameBlock != null)
                {
                    if (gameBlock.type == GameBlockType.Color)
                    {
                        ColorBlock ColorBlock = gameBlock.GetComponent<ColorBlock>();
                        if (ColorBlock != null)
                        {
                            if (ColorBlock.colorCount > 1)
                            {
                                int missionCount = MissionManager.Instance.missionList.Count;
                                for (int j = 0; j < missionCount; j++)
                                {
                                    if (MissionManager.Instance.missionList[j].blockCount > 0)
                                    {
                                        if (ColorBlock.colorType.ToString() == MissionManager.Instance.missionList[j].blockType)
                                        {
                                            //미션에 해당하는 색깔블럭
                                            if (missionMaxCount < ColorBlock.colorCount)
                                            {
                                                missionMaxCount = ColorBlock.colorCount;
                                                missionIndex = index[i];
                                            }
                                        }
                                    }
                                }
                                if (maxCount < ColorBlock.colorCount)
                                {
                                    maxCount = ColorBlock.colorCount;
                                    maxIndex = index[i];
                                }
                                //ColorBlock.OnClickButton();
                            }
                        }
                    }
                    else if (gameBlock.type == GameBlockType.Item)
                    {
                        ItemBlock itemBlock = gameBlock.GetComponent<ItemBlock>();
                        if (itemBlock != null)
                        {
                            //itemBlock.OnClickButton();
                            itemIndex = index[i];
                        }
                    }
                }
            }
            if (missionIndex != -1)
            {
                InGameManager.instance.positionList[missionIndex].GetComponentInChildren<GameBlock>().OnClickButton();
            }
            else if (itemIndex != -1)
            {
                InGameManager.instance.positionList[itemIndex].GetComponentInChildren<GameBlock>().OnClickButton();
            }
            else if(maxIndex!=-1)
            {
                InGameManager.instance.positionList[maxIndex].GetComponentInChildren<GameBlock>().OnClickButton();
            }
            if (bGameClear)
            {
                //OnClickButtonNext();
                yield break;
            }
            if (bGameOver)
            {
                OnClickButtonRetry();
                yield break;
            }
        }
    }
    public void MissionClear()
    {
        if (bGameOver) return;
        if (!bGameClear)
        {
            bGameClear = true;
            StartCoroutine(ClearEffect(true));
        }
    }
    void CreateStar(int index,GameBlock gameBlock)
    {
        MissionManager.Instance.blockCount--;
        MissionManager.Instance.txtBlockCount.text = MissionManager.Instance.blockCount.ToString();
        //블록카운트에서 아이템블록 생성위치간의 이펙트 생성
        StartCoroutine(FlyStar(index, gameBlock));
    }
    IEnumerator FlyStar(int index,GameBlock gameBlock)
    {
        GameObject dummyStar = new GameObject("DummyStar");
        //GameObject star = Instantiate(Resources.Load<GameObject>("Prefabs/FX_LightBall_A"));
        GameObject star = PoolManager.Instance.GetPool(PoolType.FX_LightBall_A).gameObject;
        star.SetActive(true);
        RectTransform starRTr = dummyStar.AddComponent<RectTransform>();
        star.transform.SetParent(ParticleManager.Instance.particleCanvasF.transform);
        dummyStar.transform.SetParent(rocket.transform);
        starRTr.anchoredPosition3D = new Vector3(-450f, 300f, 0f);
        starRTr.localScale = Vector3.one;
        FollowUI ui = star.AddComponent<FollowUI>();
        ui.bFollow = true;
        ui.target = positionList[index].gameObject.GetComponent<RectTransform>();
        ui.subTarget = starRTr;
        float speed = 4f;
        iVector3 p = new iVector3(gameBlock.pos.x, gameBlock.pos.y, gameBlock.pos.z);
        star.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        star.transform.localScale = Vector3.one;
        FX_Electric.gameObject.SetActive(true);
        FX_Electric.Play();
        SoundManager.Instance.PlayEffect("eff_plasma");
        SoundManager.Instance.PlayEffect("eff_finale");
        dummyStar.transform.SetParent(gameBlock.transform);
        
        while ((starRTr.anchoredPosition3D).sqrMagnitude > 100f)
        {
            starRTr.anchoredPosition3D = Vector3.Lerp(starRTr.anchoredPosition3D, Vector3.zero, Time.deltaTime * speed);
            yield return null;
            if (starRTr == null) break;
        }

        
        star.SetActive(false);
        dummyStar.SetActive(false);

        //ParticleManager.Instance.SetParticle(ParticleType.ParticleF, Particle.FX_SpreadStar, positionList[BlockTools.iVector3ToIndex(p)].GetComponent<RectTransform>());
        GameObject fx_SpreadStar = PoolManager.Instance.GetPool(PoolType.FX_SpreadStar).gameObject;
        ParticleManager.Instance.SetParticle(ParticleType.ParticleF, fx_SpreadStar, positionList[BlockTools.iVector3ToIndex(p)].GetComponent<RectTransform>());

        if (gameBlock != null)
        {
            ColorBlock colorBlock = gameBlock.GetComponent<ColorBlock>();
            if (colorBlock != null)
            {
                colorBlock.DestroyObject();
            }
            GrayBlock grayBlock = gameBlock.GetComponent<GrayBlock>();
            if (grayBlock != null)
            {
                grayBlock.DestroyObject();
            }
        }

        //이떄 생성되는 느낌의 파티클 한개더
        //ItemBlock itemBlock = Instantiate(Resources.Load<GameObject>("Prefabs/ItemBlock")).GetComponent<ItemBlock>();
        ItemBlock itemBlock = PoolManager.Instance.GetPool(PoolType.ItemBlock).GetComponent<ItemBlock>();
        itemBlock.gameObject.SetActive(true);
        itemBlock.pos = p;
        itemBlock.itemType = (ItemType)UnityEngine.Random.Range(0, 2);
        itemBlock.transform.SetParent(positionList[index].transform);
        itemBlock.transform.localPosition = Vector3.zero;
        itemBlock.transform.localScale = Vector3.one;
        itemBlock.bIsParentPos = true;
        itemBlock.Initialize();
    }
    IEnumerator ClearEffect(bool bSuccess)
    {
        if (bSuccess)
        {
            SetRocketBit(RocketBit.RocketBit_happy_loop);
            List<int> indexList = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                indexList.Add(i);
            }
            BlockTools.Shuffle(indexList);
            BlockSpawnManager.Instance.MoveStop(10f);
            InGameManager.instance.MoveStop(10f);

            if (25 - GetItemBlockCount() < MissionManager.Instance.blockCount)
            {//남은 블록개수가 아이템으로 바꿔줘야 하는 블록보다 많은 경우
                MissionManager.Instance.blockCount = 25 - GetItemBlockCount();
            }
            List<int> fxList = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                if (MissionManager.Instance.blockCount.CompareTo(0) == 0) { break; }
                if (fxList.Contains(indexList[i])) continue;

                GameBlock gameBlock = positionList[indexList[i]].GetComponentInChildren<GameBlock>();
                if (gameBlock.type == GameBlockType.Color || gameBlock.type == GameBlockType.Gray)
                {
                    CreateStar(indexList[i], gameBlock);
                    fxList.Add(indexList[i]);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(2f);
            //BlockSpawnManager.Instance.MoveStart();
            //MoveStart();
            for (int i = 0; i < 25; i++)
            {
                GameBlock gameBlock = positionList[indexList[i]].GetComponentInChildren<GameBlock>();
                if (gameBlock != null)
                {
                    if (gameBlock.type == GameBlockType.Item)
                    {
                        gameBlock.Explosion(true);
                        break;
                    }
                }
            }

            while (GetItemBlockCount() > 0)
            {
                yield return new WaitForEndOfFrame();
                if (CheckMoveBlock()) continue;
                //if (bDelay) continue;
                for (int i = 0; i < 25; i++)
                {
                    GameBlock gameBlock = positionList[indexList[i]].GetComponentInChildren<GameBlock>();
                    ItemBlock itemBlock = null;
                    if (gameBlock != null)
                    {
                        if (gameBlock.type == GameBlockType.Item)
                        {
                            if (gameBlock.bDestroy) continue;
                            yield return null;
                            //Debug.Log("ClearEffect : " + gameBlock.pos.ToString2());
                            if (gameBlock == null) continue;
                            itemBlock = gameBlock.GetComponent<ItemBlock>();
                            gameBlock.Explosion(true);
                            switch (itemBlock.itemType)
                            {
                                case ItemType.Bomb:
                                    InGameManager.instance.MoveStop(0.84f);
                                    BlockSpawnManager.Instance.MoveStop(0.84f);
                                    //yield return new WaitForSeconds(2f);
                                    break;
                                case ItemType.Horizental:
                                case ItemType.Vertical:
                                    InGameManager.instance.MoveStop(0.42f);
                                    BlockSpawnManager.Instance.MoveStop(0.42f);
                                    //yield return new WaitForSeconds(2f);
                                    break;
                            }
                            break;
                        }
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(1f);
            rocket.Fire(2);
        }
        else//gameover
        {
            SetRocketBit(RocketBit.RocketBit_sad_loop);
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayEffect("eff_alert");
            FuelManager.Instance.AllLock(false);
            SoundManager.Instance.PlayEffect("eff_electrical_sparks");
            yield return new WaitForSeconds(2f);
            FX_Smoke_Black.SetActive(true);
            MissionManager.Instance.MissionPanelOff();
            yield return new WaitForSeconds(1f);
            
            FX_Electric.gameObject.SetActive(true);
            FX_Electric.Play();
            SoundManager.Instance.PlayEffect("eff_plasma");
            FX_Electric.gameObject.GetComponent<FXLoop>().loopTime = 1.3f;
            FX_Electric.gameObject.GetComponent<FXLoop>().bLoop = true;

            rocket.randomBounce.Bounce();
            FX_Electric_Bridge.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        
        MissionManager.Instance.missionParent.gameObject.SetActive(false);
        RectTransform rTr = rocket.gameObject.GetComponent<RectTransform>();
        SoundManager.Instance.PlayEffect("eff_rocket_start");
        while (rTr.anchoredPosition3D.y < 1000f)
        {
            rTr.anchoredPosition3D = Vector3.Lerp(rTr.anchoredPosition3D, new Vector3(0f, 1100f, 0f), Time.deltaTime * rocket.speed);
            yield return null;
        }
        SoundManager.Instance.StopEffect(SoundName.eff_alert);
        boardParent.SetActive(false);
        rocket.gameObject.SetActive(false);
        fuel.SetActive(false);
        FX_Electric_Bridge.SetActive(false);
        FX_Smoke_Black.SetActive(false);
        FX_Electric.gameObject.SetActive(false);
        btn_Exit.SetActive(false);
        ResultManager.Instance.Initialize(bSuccess);
    }
    bool CheckMoveBlock()
    {
        for (int i = 0; i < 25; i++)
        {
            GameBlock gameBlock = positionList[i].GetComponentInChildren<GameBlock>();
            if (gameBlock != null)
            {
                if (!gameBlock.bDestroy)
                {
                    if (gameBlock.bMove)
                    {
                        return true;
                    }
                }
            }
        }
        //Debug.Log("move false");
        return false;
        
    }
    void RespawnItem(iVector3 pos)
    {
        ItemBlock itemBlock = Instantiate(Resources.Load<GameObject>("Prefabs/ItemBlock")).GetComponent<ItemBlock>();
        itemBlock.pos = pos;

        itemBlock.transform.SetParent(InGameManager.instance.positionList[BlockTools.iVector3ToIndex(pos)].transform);
        itemBlock.rTr.anchoredPosition3D = Vector3.zero;
        itemBlock.rTr.localScale = Vector3.one;
        itemBlock.itemType = (ItemType)UnityEngine.Random.Range(0, 3);
        itemBlock.Initialize();
    }
    public void MoveStart()
    {
        bMove = true;
        bDelay = false;
        delayTime = 0f;
    }
    public void MoveStop(float time =0.45f)
    {
        bMove = false;
        bDelay = true;
        delayTime = time;
    }
    public void OnClickButtonExit()
    {
        SoundManager.Instance.AllSoundStop(true);
        SoundManager.Instance.PlayEffect("eff_button");
        SceneController.Instance.MoveScene(SCENENAME.Lobby);
    }
    public bool IsGameOver()
    {
        return bGameOver;
    }
    int GetItemBlockCount()
    {
        int count = 0;
        for (int i = 0; i < 25; i++)
        {
            GameBlock gameBlock = positionList[i].GetComponentInChildren<GameBlock>();
            if (gameBlock != null)
            {
                if (gameBlock.type == GameBlockType.Item)
                {
                    count++;
                }
            }
        }
        return count;
    }
    public void SetRocketBit(RocketBit type)
    {
        rocketBit.Play(type.ToString());
    }
}

