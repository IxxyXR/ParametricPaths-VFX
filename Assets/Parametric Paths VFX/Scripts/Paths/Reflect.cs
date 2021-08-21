using UnityEngine;

public class Reflect : BasePathModule
{
    public bool x;
    public bool y;
    public bool z;
    public float interval = 1f;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        if (t % interval > 0.5f)
        {
            pos = new Vector3(
                pos.x * (x ? -1 : 1),
                pos.y * (y ? -1 : 1),
                pos.z * (z ? -1 : 1)
            );
        }
    }
}