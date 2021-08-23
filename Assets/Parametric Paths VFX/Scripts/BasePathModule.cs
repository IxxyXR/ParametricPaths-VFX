using UnityEngine;

public abstract class BasePathModule : MonoBehaviour
{
    
    protected static float PingPong(float t)
    {
        float P = 0.5f;
        return 1 / P * (P - Mathf.Abs((t/2f + 1) % (2 * P) - P));
    }

    public bool active = true;
    public abstract void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos);
}