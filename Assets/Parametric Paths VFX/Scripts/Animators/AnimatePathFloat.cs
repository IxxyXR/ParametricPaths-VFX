using System.Reflection;
using UnityEngine;


public class AnimatePathFloat : BaseAnimator
{
    public override void DoAnimation(float time)
    {
        if (PathModule==null) return;
        float value = CalcValue(time);
        FieldInfo fieldInfo = PathModule.GetType().GetField(FieldName);
        if (fieldInfo == null) return;
        fieldInfo.SetValue(PathModule, value);
    }
}
