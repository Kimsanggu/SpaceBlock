using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum AlignmentWidth
{
    Left,
    Middle,
    Right
}
public enum AlignmentHeight
{
    Top,
    Middle,
    Bottom
}
[ExecuteInEditMode]
public class TextUsedImage : MonoBehaviour {
    public string _string;  //오직 숫자만 가능
    public float spacingX;
    public float fontSize = 1f;
    public AlignmentWidth alignWidth;
    public AlignmentHeight alignHeight;
    public string texturePath = "Textures/number_timer_0";//Resources에서 로드 단,Sprite Multiple editor로 0~9 순서대로 되어있어야 한다.
    public bool useAlignment = true;

    private List<Sprite> numberss = new List<Sprite>(); //로드된 숫자
    private List<Vector2> rects = new List<Vector2>();  //각 숫자의 rect 정보
    private GameObject content;                         //숫자들의 부모
    private RectTransform rectTr;

    private Vector2 maxSize;
    private Vector2 avgSize;
    private Vector2 minSize;
    private Vector2 totalSize;
    private Vector2 standardPos;

    private float spaceSize;
    void Awake()
    {
        rectTr = GetComponent<RectTransform>();
        
        LoadTexture();
        LoadContent();
    }
	void Start () {
        //SetImage(_string);
        //text = "0123456789";
	}
    
    public string text
    {
        get
        {
            return _string;
        }
        set
        {
            if (_string.CompareTo(value) == 0)
                return;

            _string = value;
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            SetImage(_string);
        }
    }
    public void SetImage(string value){
        char[] letters = value.ToCharArray();
        SortImage(letters);
    }
    public void SetImage()
    {
        char[] letters = _string.ToCharArray();
        SortImage(letters);
    }
    void SortImage(char[] letters)
    {
        ContentClear();
        
        //Debug.Log("standardPos : " + standardPos);
        if(!Verify(letters)){
            Debug.LogError(gameObject.name+" : TextUserdImage 컴포넌트는 공백 포함 숫자만 가능합니다.");
            return;
        }
        int[] space = Calculate(letters);//.ToString().Trim().ToCharArray());
        standardPos = GetStandardPos(letters);
        MakeImage(letters, space);
        SetAlignMent(alignWidth, alignHeight);
    }
    public void SetAlignMent(AlignmentWidth width,AlignmentHeight height)
    {
        if (useAlignment)
        {
            switch (width)
            {
                case AlignmentWidth.Left:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, content.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                case AlignmentWidth.Middle:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-totalSize.x / 2f, content.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                case AlignmentWidth.Right:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-totalSize.x, content.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
            }
            switch (height)
            {
                case AlignmentHeight.Top:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x, maxSize.y);
                    break;
                case AlignmentHeight.Middle:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x, 0f);
                    break;
                case AlignmentHeight.Bottom:
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x, -maxSize.y);
                    break;
            }
        }
    }
    void MakeImage(char[] letters,int[] space)
    {
        bool isSpace = false;
        if (space[0].CompareTo(-1) != 0)//공백있음
        {
            isSpace = true;
        }
        else
        {
            //Debug.Log("공백없음");
        }

        int count = letters.Length;
        int spaceCount = space.Length;
        for (int i = 0; i < count; i++)
        {
            if (isSpace)
            {
                for (int j = 0; j < spaceCount; j++)
                {
                    if (i == space[j])
                    {
                        //Debug.Log(i+"번쨰 공백");
                        //do space standard
                        standardPos = new Vector2(standardPos.x + avgSize.x, standardPos.y);
                        continue;
                    }
                }
            }
            GameObject temp = new GameObject("letter");
            temp.AddComponent<Image>();
            temp.GetComponent<Image>().sprite = GetSprite(letters[i]);
            temp.GetComponent<Image>().raycastTarget = false;
            temp.GetComponent<RectTransform>().sizeDelta = GetRect(letters[i]);
            temp.transform.SetParent(content.transform);
            //temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = Vector3.one*fontSize;
            temp.transform.localPosition = standardPos;
            
            if (i == count - 1)
            {
                continue;
            }
            if (letters[i + 1].CompareTo('1') == 0)
            {
                //standardPos = new Vector2(standardPos.x + (45f), standardPos.y);//((GetRect(letters[i]).x * fontSize) / 2f + (GetRect(letters[i+1]).x * fontSize) / 2f)+spacingX, standardPos.y);
            }
            else
            {
                //standardPos = new Vector2(standardPos.x + (50f), standardPos.y);//((GetRect(letters[i]).x * fontSize) / 2f + (GetRect(letters[i+1]).x * fontSize) / 2f)+spacingX, standardPos.y);
            }
            standardPos = new Vector2(standardPos.x + ((GetRect(letters[i]).x * fontSize) / 2f + (GetRect(letters[i+1]).x * fontSize) / 2f)+spacingX, standardPos.y);
        }
        
        
    }
    public Vector2 GetRect(int ch)
    {
        Vector2 pos = Vector2.zero;
        switch (ch)
        {
            case '0':
                pos = rects[0];
                break;
            case '1':
                pos = rects[1];
                break;
            case '2':
                pos = rects[2];
                break;
            case '3':
                pos = rects[3];
                break;
            case '4':
                pos = rects[4];
                break;
            case '5':
                pos = rects[5];
                break;
            case '6':
                pos = rects[6];
                break;
            case '7':
                pos = rects[7];
                break;
            case '8':
                pos = rects[8];
                break;
            case '9':
                pos = rects[9];
                break;
            default:
                pos = rects[0];
                break;
        }
        return pos;
    }
    public Vector2 GetStandardPos(char[] letters)
    {
        Vector2 standardPos = Vector2.zero;
        
        float alignX=0f;
        float alignY=0f;
        alignX = GetSprite(letters[0]).rect.width / 2f * fontSize;
        
        //Debug.Log("AlignX : " + alignX);
        return standardPos+new Vector2(alignX,alignY);
    }
    int[] Calculate(char[] letters)
    {

        int count = letters.Length;
        float maxX =float.MinValue;
        float maxY = float.MinValue;
        float avgX=0f;
        float avgY=0f;
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float sumX = 0f;
        float sumY = 0f;
        int spaceCount=0;
        for (int i = 0; i < count; i++)
        {
            //Debug.Log("letters[" + i + "] : " + letters[i]+" - "+GetSprite(letters[i]).rect.width);
            if (letters[i].CompareTo(' ') == 0)
            {
                spaceCount++;
                continue;
            }
            Sprite temp = GetSprite(letters[i]);
            sumX += temp.rect.width * fontSize;
            //Debug.Log("sumX : " + sumX);
            sumY += temp.rect.height * fontSize;
            if (minX >= temp.rect.width)
            {
                minX = temp.rect.width;
            }
            if (minY >= temp.rect.height)
            {
                minY = temp.rect.height;
            }
            if (maxX <= temp.rect.width)
            {
                maxX = temp.rect.width;
            }
            if (maxY <= temp.rect.height)
            {
                maxY = temp.rect.height;
            }
        }
        int[] space;
        if (spaceCount != 0)
        {
            space = new int[spaceCount];
        }
        else
        {
            space = new int[1];
            space[0] = -1;
        }
        
        avgX = sumX / (count-spaceCount);
        avgY = sumY / (count-spaceCount);

        totalSize = new Vector2(sumX+(spacingX*(letters.Length-1)), sumY);
        //Debug.Log("totalSize : " + totalSize.x);
        minSize = new Vector2(minX, minY);
        maxSize = new Vector2(maxX, maxY);
        avgSize = new Vector2(avgX, avgY);
        return space;
    }
    public Sprite GetSprite(int ch)
    {
        Sprite sp = null;
        //Debug.Log("ch : " + ch);
        switch (ch)
        {
            case '0':
                sp = numberss[0];
                break;
            case '1':
                sp = numberss[1];
                break;
            case '2':
                sp = numberss[2];
                break;
            case '3':
                sp = numberss[3];
                break;
            case '4':
                sp = numberss[4];
                break;
            case '5':
                sp = numberss[5];
                break;
            case '6':
                sp = numberss[6];
                break;
            case '7':
                sp = numberss[7];
                break;
            case '8':
                sp = numberss[8];
                break;
            case '9':
                sp = numberss[9];
                break;
            default:
                sp = numberss[0];
                break;
        }
        //Debug.Log("GetSprite : " + sp.name);
        return sp;
    }
    bool Verify(char[] letters)
    {
        int count = letters.Length;
        int temp=0;
        for (int i = 0; i < count; i++)
        {

            if (letters[i].CompareTo(' ') == 0)
            {
                //Debug.Log(i + "번쨰는 공백");
                continue;
            }
            if (!(System.Int32.TryParse(letters[i].ToString(), out temp)))
            {
                //Debug.Log(letters[i].ToString() + "는 숫자가 아니다.");
                return false;
            }
        }
        return true;
    }
    void LoadContent()
    {
        if (gameObject.transform.Find("Content") == null)
        {
            Debug.LogError("TextUsedImage에 Content자식오브젝트를 만들어주세요");
            return;
        }
        content = gameObject.transform.Find("Content").gameObject;
    }
    public void ContentClear()
    {
        if (content == null)
        {
            LoadContent();
        }
        int count = content.transform.childCount;
        while(content.transform.childCount>0)
        {
            DestroyImmediate(content.transform.GetChild(0).gameObject);
        }
    }
    public void LoadTexture()
    {
        if (Resources.Load(texturePath) == null)
        {
            Debug.LogError("Resources/" + texturePath + "경로에 파일이 존재하지 않습니다.");
            return;
        }
        Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath);
        numberss = new List<Sprite>();
        rects = new List<Vector2>();
        int count = sprites.Length;
        for (int i = 0; i < count; i++)
        {
            numberss.Add(sprites[i]);
            Vector2 rect = new Vector2(sprites[i].rect.width, sprites[i].rect.height);
            rects.Add(rect);
            //Debug.Log((i + 1).ToString() + " : " + rect.x + " , " + rect.y);
        }
    }
}
