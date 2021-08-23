using UnityEngine;

public class TimeWarp : BaseWave
{
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        t = (CalcWave(t) + amplitude) / 2f;
    }

}