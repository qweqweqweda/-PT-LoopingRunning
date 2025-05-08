using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CV_Play
{
    public const float map_PosX_Top = 100;
    public const float map_PosY_Botton = -100;

    public static float GetDistance(Vector3 posA, Vector3 posB)
    {
        return Vector3.Distance(posA, posB);
    }

    public static float GetDistance(Vector3 posA, float radiusA, Vector3 posB, float radiusB)
    {
        float dis = Vector3.Distance(posA, posB);

        dis -= radiusA;
        dis -= radiusB;

        if (dis <= 0)
            dis = 0;

        return dis;
    }

    public static float GetConvertedPosZ(float posY)
    {
        return posY * 0.1f;
    }
}