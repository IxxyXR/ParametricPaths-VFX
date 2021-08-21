using UnityEngine;

public class SawtoothWaveTimeWarp : BasePathModule
{
    public float frequency = 1f;
    public float amplitude = 1f;
    public float phase = 0f;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        t = (((t + phase) * 1f/frequency) % 1f) * amplitude;
    }
}