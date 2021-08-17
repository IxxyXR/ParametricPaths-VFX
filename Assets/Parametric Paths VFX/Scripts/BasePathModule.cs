using UnityEngine;

public abstract class BasePathModule : MonoBehaviour
{
    public abstract void CalcTransforms(float t, ref Quaternion rot, ref Vector3 pos);
}