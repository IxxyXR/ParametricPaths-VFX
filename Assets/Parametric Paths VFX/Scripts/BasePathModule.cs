using UnityEngine;

public abstract class BasePathModule : MonoBehaviour
{

    public bool active = true;
    public abstract void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos);
}