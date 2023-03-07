using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    public static Vector3 GetPoint(Transform tranform, Vector3 p0 , Vector3 p1 , Vector3 p2 , float t)
    {
        float OneMinusT = 1f - t;
        var result = OneMinusT * OneMinusT * p0 + 2 * OneMinusT * t * p1 + t * t * p2;
        return tranform.TransformPoint(result);
        //return result;
    }

    public static Vector3 FirstDerivative(Vector3 p0 , Vector3 p1 , Vector3 p2 , float t)
    {
        float OneMinusT = 1f - t;
        var result = 2 * OneMinusT * (p1 - p0) + 2 * t * (p2 - p1);
        return result;
    }

    public static Vector3 GetVelocity(Transform transform, Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return transform.TransformPoint(FirstDerivative(p0, p1, p2, t));
    }
    
    public static Vector3 GetDirection(Transform transform, Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return GetVelocity(transform, p0, p1, p2, t).normalized;
    }
}
