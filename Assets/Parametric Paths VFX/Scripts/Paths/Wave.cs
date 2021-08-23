using System;
using NaughtyAttributes;
using UnityEngine;

public class Wave : BaseWave
{

    public float length = 1f;
    public bool pingPong;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        var newPos = new Vector3(
            (pingPong ? PingPong(t) : t % 1f) * length,
            CalcWave(t),
            0
        );
        pos += (rot * newPos);
    }

}