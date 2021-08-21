using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class BasePathConsumer : MonoBehaviour
{
    public GameObject Path;
    public float Scale = 1;
    public float Rate = 0.01f;
    [ReadOnly] public float _time = 0;
    protected Vector3 _initialPosition;
    protected VisualEffect _vfx;

    protected void ProcessModules(ref Vector3 pos, ref Quaternion rot)
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
            else if (component.GetType().IsSubclassOf(typeof(BaseAnimator)) && previousPathModule != null)
            {
                var animator = component as BaseAnimator;
                if (animator != null && animator.active)
                {
                    animator.PathModule = previousPathModule;
                    animator.DoAnimation(time);
                }
            }
        }

        foreach (var module in Path.GetComponents<BasePathModule>())
        {
            if (module.active)
            {
                module.CalcTransforms(ref time, ref rot, ref pos);
            }
        }
    }
}