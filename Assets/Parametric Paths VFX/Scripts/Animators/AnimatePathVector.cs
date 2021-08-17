using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnimatePathVector : BaseAnimator
{

    [Serializable] public enum AxisChoices {X, Y, Z}
    public AxisChoices Axis;

    public override void DoAnimation(float time)
    {
        if (PathModule==null) return;
        float value = CalcValue(time);
        FieldInfo fieldInfo = PathModule.GetType().GetField(FieldName);
        if (fieldInfo == null) return;
        Vector3 v = (Vector3)fieldInfo.GetValue(PathModule);
        switch (Axis)
        {
            case AxisChoices.X:
                v = new Vector3(value, v.y, v.z);
                break;
            case AxisChoices.Y:
                v = new Vector3(v.x, value, v.z);
                break;
            case AxisChoices.Z:
                v = new Vector3(v.x, v.y, value);
                break;
        }
        fieldInfo.SetValue(PathModule, v);
    }
}
