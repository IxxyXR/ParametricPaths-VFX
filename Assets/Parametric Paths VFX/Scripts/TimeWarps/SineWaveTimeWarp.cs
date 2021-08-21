using UnityEngine;

public class SineWaveTimeWarp : BasePathModule
{
    public float frequency = 1f;
    public float amplitude = 1f;
    public float phase = 0f;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        t = ((Mathf.Sin((t + phase) * Mathf.PI * 2f * frequency) + 1f) / 2f) * amplitude;
    }
}