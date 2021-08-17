using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseAnimator : MonoBehaviour
{
    public string FieldName;
    public float frequency = .1f;
    public float phase;
    public Vector2 Range = Vector2.up;
    public EasingFunction.Ease easing = EasingFunction.Ease.Linear;
    
    [NonSerialized] public BasePathModule PathModule;
    
    protected float CalcValue(float inputTime)
    {
        float t = (Mathf.Sin((phase + inputTime) * Mathf.PI * 2f * frequency) + 1f) / 2f;
        t = EasingFunction.GetEasingFunction(easing).Invoke(0, 1, t);
        float value = Mathf.Lerp(Range.x, Range.y, t);
        return value;
    }
    
    public abstract void DoAnimation(float time);
}
