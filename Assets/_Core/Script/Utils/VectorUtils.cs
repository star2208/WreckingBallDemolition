using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一些向量辅助方法
public static class VectorUtils
{
    //获取弧度
    public static float GetRad(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x);
    }

    //获取角度
    public static float GetDeg(this Vector2 v)
    {
        return v.GetRad() * Mathf.Rad2Deg;
    }

    //限制最大长度
    public static void ClampMag ( ref this Vector2 v, float mag)
    {
        float m = Mathf.Clamp(v.magnitude, 0.0f, mag);
        v.SetMag(m);
        
    }

    //获取 x 到 y 之间的随机值
    public static float Random( ref this Vector2 v)
    {
        return UnityEngine.Random.Range(v.x, v.y);
    }

    //获取 x 到 y 之间的随机值
    public static int Random(ref this Vector2Int v)
    {
        return UnityEngine.Random.Range(v.x, v.y+1);
    }

    //返回 value 在 [x,y] 范围内的百分比
    public static float PercentOf ( ref this Vector2 v, float value)
    {
        return (value - v.x) / (v.y - v.x);
    }

    //返回 value 指定的 [x,y] 范围内的百分比的值
    public static float PercentBy (ref this Vector2 v, float percent)
    {
        return v.x + (v.y - v.x) * percent;
    }

    //设置长度
    public static void SetMag( ref this Vector2 v, float mag)
    {
        float rad = v.GetRad();
        v.Set(Mathf.Cos(rad) * mag, Mathf.Sin(rad) * mag);
    }

    //设置弧度
    public static void SetRad(ref this Vector2 v, float rad)
    {
        float m = v.magnitude;
        v.Set(Mathf.Cos(rad) * m, Mathf.Sin(rad) * m);
    }

    //设置角度
    public static void SetDeg( ref this Vector2 v, float deg)
    {
        float m = v.magnitude;
        float rad = deg * Mathf.Deg2Rad;
        v.Set(Mathf.Cos(rad) * m, Mathf.Sin(rad) * m);
    }

    //设置长度和弧度
    public static void SetRadMag(ref this Vector2 v, float rad, float mag)
    {
        v.Set(Mathf.Cos(rad) * mag, Mathf.Sin(rad) * mag);
    }
}
