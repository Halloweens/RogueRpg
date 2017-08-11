using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public sealed class Ref<T>
{
    private Func<T> getter;
    private Action<T> setter;

    public Ref(Func<T> getter, Action<T> setter)
    {
        this.getter = getter;
        this.setter = setter;
    }

    public T Value
    {
        get { return getter(); }
        set { setter(value); }
    }
}

public static class Utility
{
    public static int ParseToInt(string text)
    {
        return Int32.Parse(text);
    }
}

public static class Math
{
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }
}

[System.Serializable]
public struct Vector2i
{
    public int x, y;

    public readonly static Vector2i zero = new Vector2i(0, 0);

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2i operator +(Vector2i lhs, Vector2i rhs)
    {
        return new Vector2i(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    public float DistTo(Vector2i dest)
    {
        return Mathf.Sqrt(Mathf.Pow(dest.x - x, 2) + Mathf.Pow(dest.y - y, 2));
    }

    public static float CalculateDist(Vector2i origin, Vector2i dest)
    {
         return Mathf.Sqrt(Mathf.Pow(dest.x - origin.x, 2) + Mathf.Pow(dest.y - origin.y, 2));
    }
}