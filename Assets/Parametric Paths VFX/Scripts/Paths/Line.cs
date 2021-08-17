﻿using UnityEngine;

public class Line : BasePathModule
{
    public float repeats = 1f;
    public Vector3 distance = Vector3.forward * 0.02f;
    public EasingFunction.Ease easing = EasingFunction.Ease.Linear;

    public float PingPong(float t)
    {
        float P = 0.5f;
        return (1 / P) * (P - Mathf.Abs(t % (2 * P) - P));
    }
    
    public override void CalcTransforms(float t, ref Quaternion rot, ref Vector3 pos)
    {
        float easedTime = EasingFunction.GetEasingFunction(easing).Invoke(0, 1, PingPong(t * repeats));
        var offset = Vector3.Lerp(Vector3.zero, distance, easedTime);
        offset = rot * offset;
        pos += offset;
        rot = Quaternion.LookRotation(distance);
    }
}