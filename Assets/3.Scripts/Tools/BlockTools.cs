using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BlockTools {

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj)
        {
            if (obj is Transform)
            {
                Transform t = (obj as Transform);
                GameObject go = t.gameObject;

                if (Application.isPlaying)
                {
                    t.SetParent(null);
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                Transform t = go.transform;

                if (Application.isPlaying)
                {
                    t.SetParent(null);
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }
  
    public static iVector3 IndexToiVector3(int index)
    {
        return new iVector3(index % 5, index / 5, 0);
    }
    public static int iVector3ToIndex(iVector3 pos)
    {
        return pos.y * 5 + pos.x;
    }
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static bool ValidPos(iVector3 pos)
    {
        if (pos.x >= 0 && pos.x <= 4 && pos.y >= 0 && pos.y <= 4 & pos.z == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
[Serializable]
public struct iVector3
{
    public int x;
    public int y;
    public int z;

   
    public iVector3(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public static iVector3 One
    {
        get { return new iVector3(1, 1, 1); }
    }
    public static iVector3 Zero
    {
        get { return new iVector3(0, 0, 0); }
    }
    public iVector3 Left
    {
        get { return new iVector3(x - 1, y, z); }
    }
    public iVector3 Right
    {
        get { return new iVector3(x + 1, y, z); }
    }
    public iVector3 Up
    {
        get { return new iVector3(x, y + 1, z); }
    }
    public iVector3 Down
    {
        get { return new iVector3(x, y - 1, z); }
    }
    public bool IsEquals(iVector3 v3)
    {
        if (x == v3.x && y == v3.y && z == v3.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public string ToString2()
    {
        return x.ToString() + "," + y.ToString() + "," + z.ToString();
    }
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
        if (x == v2.x && y == v2.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}