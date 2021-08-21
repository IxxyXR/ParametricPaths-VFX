using UnityEngine;
using UnityEngine.Serialization;

public class Rotate : BasePathModule
{
    public Vector3 angles = Vector3.zero;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        rot = Quaternion.Euler(angles) * rot;
    }
}