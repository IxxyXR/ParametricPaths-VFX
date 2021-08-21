using UnityEngine;

public class SquareWaveTimeWarp : BasePathModule
{
    public float frequency = 1f;
    public float amplitude = 1f;
    public float phase = 0f;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        t = (((t + phase) * frequency) % 1f > 0.5f) ? 0 : amplitude;
    }
}