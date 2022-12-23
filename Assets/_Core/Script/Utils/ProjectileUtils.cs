using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于计算抛物线曲线 投射物速度等的辅助类
//修改自插件 Projectile Manager
public class ProjectileUtils : MonoBehaviour
{
    //给定起始点和终点和发射角度 获取发射速度
    //如果是不可能的情况 velocity 的长度为 0
    public static Vector3 GetVelocity(Vector3 start, Vector3 end, float angle)
    {
        float speed = CalculateSpeedFromAngle(start, end, angle);
        Vector3 velocity = ConvertSpeedToVector3(start, end, speed, angle);

        return velocity;
    }


    private static float CalculateSpeedFromAngle(Vector3 start, Vector3 end, float angle)
    {
        Vector2 shootPos = new Vector2(start.x,start.z);
        Vector2 hitPos = new Vector2(end.x, end.z);
        float x = Vector2.Distance(shootPos, hitPos);
        float g = Physics.gravity.y;
        float y0 = start.y;
        float y = end.y;
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float tan = Mathf.Tan(rad);

        float v0Sq = g * x * x / (2 * cos * cos * (y - y0 - x * tan));
        if (v0Sq <= 0.0f)
        {
            return 0.0f;
        }
        return Mathf.Sqrt(v0Sq);
    }
    private static Vector3 ConvertSpeedToVector3( Vector3 start, Vector3 end, float speed, float angle)
    {
        Vector3 shootPos = start;
        Vector3 hitPos = end;
        shootPos.y = 0f;
        hitPos.y = 0f;

        Vector3 dir = (hitPos - shootPos).normalized;
        Quaternion Rot3D = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = speed * Vector3.right;
        vec = Rot3D * Quaternion.AngleAxis(angle, Vector3.forward) * vec;

        return vec;
    }
}
