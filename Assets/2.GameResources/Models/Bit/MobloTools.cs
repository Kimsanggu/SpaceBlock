using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//BlendMode
public enum BlendMode
{
    Opaque = 0,
    Cutout,
    Fade,
    Transparent,
}


[Serializable]
public struct iVector2
{
    public int x;
    public int y;

    public iVector2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static iVector2 One
    {
        get { return new iVector2(1, 1); }
    }
    public static iVector2 Zero
    {
        get { return new iVector2(0, 0); }
    }

    public iVector2 Left
    {
        get { return new iVector2(x - 1, y); }
    }
    public iVector2 Right
    {
        get { return new iVector2(x + 1, y); }
    }
    public iVector2 Up
    {
        get { return new iVector2(x, y + 1); }
    }
    public iVector2 UpLeft
    {
        get { return new iVector2(x - 1, y + 1); }
    }
    public iVector2 UpRight
    {
        get { return new iVector2(x + 1, y + 1); }
    }
    public iVector2 Down
    {
        get { return new iVector2(x, y - 1); }
    }
    public iVector2 DownLeft
    {
        get { return new iVector2(x - 1, y - 1); }
    }
    public iVector2 DownRight
    {
        get { return new iVector2(x + 1, y - 1); }
    }

    public int Xz  // Zerobase
    {
        get { return x - 1; }
    }
    public int Yz  // Zerobase
    {
        get { return y - 1; }
    }
    public bool IsEquals(iVector2 v2)
    {
        if (x == v2.x && y == v2.y) {
            return true;
        } else {
            return false;
        }
    }
}

[Serializable]
public struct iCardID2
{
    public int n0;
    public int n1;

    public iCardID2(int _n0, int _n1)
    {
        n0 = _n0;
        n1 = _n1;
    }

    public static iCardID2 Zero
    {
        get { return new iCardID2(0, 0); }
    }

    public bool IsEquals(iCardID2 card)
    {
        if (n0 == card.n0 && n1 == card.n1) {
            return true;
        } else {
            return false;
        }
    }
}


public static class MobloTools
{

    static public bool PointClickArea(Vector3 inPoint)
    {
#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { return false; }
#else
        // UI 버튼에 마우스가 올라가 있는지 확인 
        if(Input.touchCount > 0){
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
                && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null) { return false; }
        }
#endif
        return true;
    }


    ////////////////////////////////////////////////////////////////////////
    // Sleep 
    // time -> 1/100 초 
    static public IEnumerator Sleep(float time)
    {
        yield return new WaitForSeconds(time);
    }

    ///////////////////////////////////////////////////////////////////////////
    // XML File Save & Load
    public static string GetQuaternionToString(Quaternion value)
    {
        return string.Format("({0},{1},{2},{3})", value.x, value.y, value.z, value.w);
    }
    public static string GetVectorToString(Vector2 value)
    {
        return string.Format("({0},{1})", value.x, value.y);
    }
    public static string GetVectorToString(Vector3 value)
    {
        return string.Format("({0},{1},{2})", value.x, value.y, value.z);
    }
    public static string GetIVectorToString(iVector3 value)
    {
        return string.Format("({0},{1},{2})", value.x, value.y, value.z);
    }
    public static string GetArrayVectorToString(Vector3[] value)
    {
        string str = "";
        for (int i = 0; i < value.Length; i++) {
            if (i != 0) {
                str += "/";
                //str +=  value[i].ToString();    // ToString 은 소수점 한자리 올림으로 값을 왜곡시킴 
                str += string.Format("({0},{1},{2})", value[i].x, value[i].y, value[i].z);
            } else {
                str = string.Format("({0},{1},{2})", value[i].x, value[i].y, value[i].z);
            }
        }
        return str;
    }
    public static string GetArrayVector2ToString(Vector2[] value)
    {
        string str = "";
        if (value == null) return str;

        for (int i = 0; i < value.Length; i++) {
            if (i != 0) {
                str += "/";
                //str +=  value[i].ToString();    // ToString 은 소수점 한자리 올림으로 값을 왜곡시킴 
                str += string.Format("({0},{1})", value[i].x, value[i].y);
            } else {
                str = string.Format("({0},{1})", value[i].x, value[i].y);
            }
        }
        return str;
    }
    public static Vector2 GetStringReplaceAndSplitV2(string value, char sp)
    {
        value = value.Replace("(", "");
        value = value.Replace(")", "");
        string[] temp = value.Split(sp);

        if (temp.Length == 2) {
            return new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
        } else {
            return Vector2.zero;
        }
    }
    public static Vector3 GetStringReplaceAndSplitV(string value, char sp)
    {
        value = value.Replace("(", "");
        value = value.Replace(")", "");
        string[] temp = value.Split(sp);

        if (temp.Length == 3) {
            return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
        } else {
            return Vector3.zero;
        }
    }
    public static iVector3 GetStringReplaceAndSplitiV3(string value, char sp)
    {
        value = value.Replace("(", "");
        value = value.Replace(")", "");
        string[] temp = value.Split(sp);

        if (temp.Length == 3) {
            return new iVector3(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
        } else {
            return iVector3.Zero;
        }
    }
    public static Quaternion GetStringReplaceAndSplitQ(string value, char sp)
    {
        value = value.Replace("(", "");
        value = value.Replace(")", "");
        string[] temp = value.Split(sp);

        if (temp.Length == 4) {
            return new Quaternion(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3]));
        } else {
            return Quaternion.identity;
        }
    }
    public static Vector3[] GetStringReplaceAndSplitV(string value, char sp1, char sp2)
    {
        string[] temp = value.Split(sp1); // '/'
        Vector3[] rValue = new Vector3[temp.Length];

        for (int i = 0; i < temp.Length; i++) {
            string str = temp[i].Replace("(", "");
            str = str.Replace(")", "");

            string[] subTemp = str.Split(sp2); // ','
            if (subTemp.Length == 3) {
                rValue[i] = new Vector3(float.Parse(subTemp[0]), float.Parse(subTemp[1]), float.Parse(subTemp[2]));
            } else {
                rValue[i] = Vector3.zero;
            }
        }

        return rValue;
    }
    public static Vector2[] GetStringReplaceAndSplitV2(string value, char sp1, char sp2)
    {
        string[] temp = value.Split(sp1); // '/'
        Vector2[] rValue = new Vector2[temp.Length];

        for (int i = 0; i < temp.Length; i++) {
            string str = temp[i].Replace("(", "");
            str = str.Replace(")", "");

            string[] subTemp = str.Split(sp2); // ','
            if (subTemp.Length == 3) {
                rValue[i] = new Vector2(float.Parse(subTemp[0]), float.Parse(subTemp[1]));
            } else {
                rValue[i] = Vector2.zero;
            }
        }

        return rValue;
    }

    ///////////////////////////////////////////////////////////////////////////
    // Finds the specified component on the game object or one of its parents.
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        T comp = go.GetComponent<T>();
        if (comp == null) {
            Transform t = go.transform.parent;

            while (t != null && comp == null) {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }
        return comp;
    }

    static public T FindInParents<T>(Transform trans) where T : Component
    {
        if (trans == null) return null;
        return trans.GetComponentInParent<T>();
    }

    static public T FindInChildrens<T>(Transform trans) where T : Component
    {
        if (trans == null) return null;
        return trans.GetComponentInChildren<T>();
    }


    /// Destroy the specified object, immediately if in edit mode.
    static public void Destroy(UnityEngine.Object obj)
    {
        if (obj) {
            if (obj is Transform) {
                Transform t = (obj as Transform);
                GameObject go = t.gameObject;

                if (Application.isPlaying) {
                    t.SetParent(null);
                    UnityEngine.Object.Destroy(go);
                } else UnityEngine.Object.DestroyImmediate(go);
            } else if (obj is GameObject) {
                GameObject go = obj as GameObject;
                Transform t = go.transform;

                if (Application.isPlaying) {
                    t.SetParent(null);
                    UnityEngine.Object.Destroy(go);
                } else UnityEngine.Object.DestroyImmediate(go);
            } else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    /// Convenience extension that destroys all children of the transform.
    static public void DestroyChildren(this Transform t)
    {
        bool isPlaying = Application.isPlaying;

        while (t.childCount != 0) {
            Transform child = t.GetChild(0);

            if (isPlaying) {
                child.SetParent(null);
                UnityEngine.Object.Destroy(child.gameObject);
            } else UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }

    // None extension Type
    static public void DestroyChildrenN(Transform t)
    {
        bool isPlaying = Application.isPlaying;

        while (t.childCount != 0) {
            Transform child = t.GetChild(0);

            if (isPlaying) {
                child.SetParent(null);
                UnityEngine.Object.Destroy(child.gameObject);
            } else UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }

    /// Destroy the specified object immediately, unless not in the editor, in which case the regular Destroy is used instead.
    static public void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null) {
            if (Application.isEditor) UnityEngine.Object.DestroyImmediate(obj);
            else UnityEngine.Object.Destroy(obj);
        }
    }

    public static float CompareToColorAlpha(Color col1, Color col2)
    {
        float r = Mathf.Abs(col1.r - col2.r);
        float g = Mathf.Abs(col1.g - col2.g);
        float b = Mathf.Abs(col1.b - col2.b);
        float a = Mathf.Abs(col1.r - col2.r);
        return (r + g + b + a);
    }
    public static float CompareToColor(Color col1, Color col2)
    {
        float r = Mathf.Abs(col1.r - col2.r);
        float g = Mathf.Abs(col1.g - col2.g);
        float b = Mathf.Abs(col1.b - col2.b);
        //float a = Mathf.Abs(col1.r - col2.r);
        return (r + g + b);
    }

    /// <summary>
    /// 현재실행중인 애니메이션 클립의 길이를 가져오는 함수
    /// 주의사항 : 비교하는 스트링이 클립의 이름과 같아야함 (애니메이션 클립이름 확인필요)
    /// </summary>
    public static float GetAniTime(Animator animator, string name)
    {
        if (animator == null) return 0;
        if (animator.runtimeAnimatorController == null) return 0;

        float time = 0f;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        int length = clips.Length;

        for (int i = 0; i < length; i++) {
            if (clips[i].name.CompareTo(name) == 0) {
                time = clips[i].length;
            }
        }
        //Debug.Log(string.Format("GetAniTime : {0}/{1}->{2}", animator.ToString(), name, time));
        return time;
    }

    /// Reset all animator parameters to their default
    public static void AnimationResetParameters(Animator animator)
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        for (int i = 0; i < parameters.Length; i++) {
            AnimatorControllerParameter parameter = parameters[i];
            switch (parameter.type) {
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(parameter.name, parameter.defaultInt);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(parameter.name, parameter.defaultFloat);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.name, parameter.defaultBool);
                    break;
            }
        }
    }

    /// Reset all animator parameters to their default
    public static string[] AnimationGetParameters(Animator animator)
    {
        if (animator == null) return null;

        List<string> nameList = new List<string>();
        AnimatorControllerParameter[] parameters = animator.parameters;
        for (int i = 0; i < parameters.Length; i++) {
            AnimatorControllerParameter parameter = parameters[i];
            nameList.Add(parameter.name);
        }
        return nameList.ToArray();
    }


    ////////////////////////////////////////////////////////////////////////
    // Hex to Color : 6자리 또는 8자리
    static public Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = 255;

        if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        //Debug.Log(string.Format("r={0}, g={1}, b={2}, a={3}", r, g, b, a));
        return new Color32(r, g, b, a);
    }

    static public string ColorToHex(Color col)
    {
        return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", ToByte(col.r), ToByte(col.g), ToByte(col.b), ToByte(col.a));
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    //////////////////////////////////////////////////////////////
    // ASCII(Byte) to Int : ASCII -> Char -> Int (X)
    // ASCII(Byte) to Int : ASCII -> Char -> String -> Int (O)
    static public int ASCIIToInt(Byte ascii)
    {
        if (ascii == 0) return 0;
        return System.Convert.ToInt32(((Char)ascii).ToString());
    }
    

    //////////////////////////////////////////////////////////////
    // 영문, 한글 문자열 자르기
    public static string CheckByte(string str, int iLength)
    {
        string strResult = "";
        int lenSum = 0;

        // str의 byte수가 자를 byte수보다 클때 실행
        if (iLength < System.Text.Encoding.Default.GetBytes(str).Length) {
            for (int i = 0; i < iLength; i++) {
                string chr = str.Substring(i, 1);
                lenSum += System.Text.Encoding.Default.GetBytes(chr).Length;

                if (lenSum > iLength) break;
                else strResult += chr;
            }
            strResult += "...";
        } else {
            strResult = str;
        }

        return strResult;
    }

    // 한글 영문 구분해서 길이 가져오기 
    public static float CheckStringCount(string str, float one, float two)
    {
        float lenSum = 0;

        for (int i = 0; i < str.Length; i++) {
            string chr = str.Substring(i, 1);
            if (System.Text.Encoding.Default.GetBytes(chr).Length == 1) {
                lenSum += one;
            } else {
                lenSum += two;
            }
        }

        return lenSum;
    }

    ////////////////////////////////////////////////////////////////////////////
    // 배열 섞기, 랜덤 번호 생성 
    //static System.Random random;
    //static int seed = -1;
    //static int seedPlus = 0;

    static public void SwapArrayElements<T>(this T[] arr) 
    {
        //seed = DateTime.Now.Millisecond + (100 * seedPlus++);
        //random = new System.Random(seed);
        //for (int i = 0; i < arr.Length; i++) {
        //    TSwap(arr, i, random.Next(0, arr.Length-1));
        //}

        for (int i = 0; i < arr.Length; i++) { 
            TSwap(arr, i, Rand.Instance.NextInt(0, arr.Length));
        }
    }

    public static void TSwap<T>(T[] _arr, int num1, int num2) 
    {
        T tmp = _arr[num1];
        _arr[num1] = _arr[num2];
        _arr[num2] = tmp; 
    }

    // max를 포함하지 않음
    public static int GetRandomNum(int min, int max)
    {
        //seed = DateTime.Now.Millisecond + (10 * seedPlus++);
        //random = new System.Random(seed);
        //return random.Next(min, max);  
        return Rand.Instance.NextInt(min, max);
    }

    // 하위레이어모두 바꾸기
    public static void ChangeLayers(GameObject go, int layer)
    {
        go.layer = layer;

        Transform trans = go.transform;
        for (int i = 0; i < trans.childCount; ++i) {
            GameObject obj = trans.GetChild(i).gameObject;
            if (obj != null) {
                ChangeLayers(obj, layer);
            }
        }
    }
    

    /////////////////////////////////////////////////////////////////////////////////////////////
    // BlendMode
    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
    {
        if (material == null) return;

        switch (blendMode) {
            case BlendMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}



/// <summary>
/// Implements presudo-random number generator using Xorshift128 algorithm.
/// </summary>
public class Rand
{
    private const int a = 5;
    private const int b = 14;
    private const int c = 1;

    private const uint DefaultY = 273326509;
    private const uint DefaultZ = 3579807591;
    private const uint DefaultW = 842502087;

    private const uint PositiveMask = 0x7FFFFFFF;
    private const uint BoolModuloMask = 0x1;
    private const uint ByteModuloMask = 0xFF;

    private const double One_div_uintMaxValuePlusOne = 1.0 / ((double)uint.MaxValue + 1.0);
    private const double TwoPi = System.Math.PI * 2.0;

    private static Rand _seedGenerator;

    private uint _x;
    private uint _y;
    private uint _z;
    private uint _w;

    public static Rand Instance;

    static Rand()
    {
        _seedGenerator = new Rand(System.Environment.TickCount);
        Instance = new Rand();
    }

    /// <summary>
    /// Creates random number generator using randomized seed.
    /// </summary>
    public Rand()
    {
        ResetSeed(_seedGenerator.NextInt());
    }

    /// <summary>
    /// Creates random number generator using specified seed.
    /// </summary>
    public Rand(int seed)
    {
        ResetSeed(seed);
    }

    /// <summary>
    /// Resets generator using specified seed.
    /// </summary>
    public void ResetSeed(int seed)
    {
        _x = (uint)((seed * 1183186591) + (seed * 1431655781) + (seed * 338294347) + (seed * 622729787));
        _y = DefaultY;
        _z = DefaultZ;
        _w = DefaultW;
    }

    /// <summary>
    /// Gets generator inner state represented by four uints. Can be used for generator serialization.
    /// </summary>
    public void GetState(out uint x, out uint y, out uint z, out uint w)
    {
        x = _x;
        y = _y;
        z = _z;
        w = _w;
    }

    /// <summary>
    /// Sets generator inner state from four uints. Can be used for generator deserialization.
    /// </summary>
    public void SetState(uint x, uint y, uint z, uint w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>
    /// Generates a random integer in the range [int.MinValue,int.MaxValue].
    /// </summary>
    public int NextInt()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (int)(_w);
    }

    /// <summary>
    /// Generates a random integer in the range [0,max)
    /// </summary>
    public int NextInt(int max)
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (int)((double)_w * One_div_uintMaxValuePlusOne * (double)max);
    }

    /// <summary>
    /// Generates a random integer in the range [min,max). max must be >= min.
    /// </summary>
    public int NextInt(int min, int max)
    {
        if (min > max) {
            return 0;
        }

        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

        int range = unchecked(max - min);
        if (range >= 0) // No overflow
			{
            return min + (int)((double)_w * One_div_uintMaxValuePlusOne * (double)range);
        }

        long longMin = (long)min;
        return (int)(longMin + (long)((double)_w * One_div_uintMaxValuePlusOne * (double)((long)max - longMin)));
    }

    /// <summary>
    /// Generates a random integer in the range [min,max]. max must be >= min.
    /// The method simply calls NextInt(min,max+1), thus largest allowable value for max is int.MaxValue-1.
    /// </summary>
    public int NextIntInclusive(int min, int max)
    {
        return NextInt(min, max + 1);
    }

    /// <summary>
    /// Generates a random integer in the range [0,int.MaxValue].
    /// </summary>
    public int NextPositiveInt()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (int)(_w & PositiveMask);
    }

    /// <summary>
    /// Generates a random unsigned integer in the range [0,uint.MaxValue].
    /// </summary>
    public uint NextUInt()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return _w;
    }

    /// <summary>
    /// Generates a random double in the range [0,1).
    /// </summary>
    public double NextDouble()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (double)_w * One_div_uintMaxValuePlusOne;
    }

    /// <summary>
    /// Generates a random double in the range [min,max).
    /// </summary>
    public double NextDouble(double min, double max)
    {
        if (min > max) {
            return 0.0;
        }

        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

        return min + (max - min) * ((double)_w * One_div_uintMaxValuePlusOne);
    }

    /// <summary>
    /// Generates a random float in the range [0,1).
    /// </summary>
    public float NextFloat()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (float)((double)_w * One_div_uintMaxValuePlusOne);
    }

    /// <summary>
    /// Generates a random float in the range [min,max).
    /// </summary>
    public float NextFloat(float min, float max)
    {
        if (min > max) {
            return 0.0f;
        }

        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

        return min + (max - min) * (float)((double)_w * One_div_uintMaxValuePlusOne);
    }

    /// <summary>
    /// Generates a random bool.
    /// </summary>
    public bool NextBool()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (_w & BoolModuloMask) == 0;
    }

    /// <summary>
    /// Generates a random byte.
    /// </summary>
    public byte NextByte()
    {
        uint t = _x ^ (_x << a);
        _x = _y;
        _y = _z;
        _z = _w;
        _w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
        return (byte)(_w & ByteModuloMask);
    }
}
