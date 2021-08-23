using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseAnimator : MonoBehaviour
{
    public bool active = true;
    public string FieldName;
    public float frequency = .1f;
    public float phase;
    public Vector2 Range = Vector2.up;
    public EasingFunction.Ease easing = EasingFunction.Ease.Linear;
    public bool pingPong = true;
    
    [NonSerialized] public BasePathModule PathModule;
    
    protected float PingPong(float t)
    {
        float P = 0.5f;
        return 1 / P * (P - Mathf.Abs((t + 1) % (2 * P) - P)) / 2f;
    }

    protected float CalcValue(float inputTime)
    {
        
        float t = (phase + inputTime) * frequency;
        t = pingPong ? PingPong(t) : t % 1f;
        // float t = (Mathf.Sin((phase + inputTime) * Mathf.PI * 2f * frequency) + 1f) / 2f;
        t = EasingFunction.GetEasingFunction(easing).Invoke(0, 1, t);
        float value = Mathf.Lerp(Range.x, Range.y, t);
        return value;
    }
    
    public abstract void DoAnimation(float time);
}
