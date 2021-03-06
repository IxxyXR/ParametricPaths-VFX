using UnityEngine;
using UnityEngine.Serialization;

public class Scaling : BasePathModule
{
    public float startScale = 1f;
    public float emdScale = .1f;
    public float speed = .1f;
    public EasingFunction.Ease easing = EasingFunction.Ease.Linear;

    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        float easedTime = EasingFunction.GetEasingFunction(easing).Invoke(0, 1, PingPong(t * speed));
        pos *= Mathf.Lerp(startScale, emdScale, easedTime);
    }
}