using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class LivePath : MonoBehaviour
{
    public GameObject Path;
    public float Rate = 0.01f;
    public float Scale = 1;
    
    [ReadOnly] public float _time = 0;
    private Vector3 _initialPosition;
    private VisualEffect vfx;
    
    void Start()
    {
        _initialPosition = Vector3.zero;
        vfx = gameObject.GetComponent<VisualEffect>();
    }
    
    void Update()
    {
        _time += Rate;
        float time = _time;
        BasePathModule previousPathModule = null;
        foreach (var component in Path.GetComponents<MonoBehaviour>())
        {
            if (component is BasePathModule)
            {
                previousPathModule = component as BasePathModule;
            }
            if (component.GetType().IsSubclassOf(typeof(BaseAnimator)) && previousPathModule!=null)
            {
                var animator = component as BaseAnimator;
                if (animator!=null && animator.active)
                {
                    animator.PathModule = previousPathModule;
                    animator.DoAnimation(time);
                }
            }
        }
        
        var pos = Vector3.zero;
        var rot = Quaternion.identity;
        foreach (var module in Path.GetComponents<BasePathModule>())
        {
            if (module.active)
            {
                module.CalcTransforms(ref time, ref rot, ref pos);
            }
        }
        transform.localPosition = _initialPosition + (pos * Scale);
        transform.localRotation = rot;

        if (vfx != null)
        {
            vfx.SendEvent("Spawn");
        }
    }

    private void OnDrawGizmos()
    {
        Handles.PositionHandle(transform.position, transform.rotation);
    }
}
