using UnityEngine;
using UnityEngine.Serialization;

public class Translate : BasePathModule
{
    public Vector3 offset = Vector3.zero;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        pos += offset;
    }
}