using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class LivePath : BasePathConsumer
{
    
    void Start()
    {
        _initialPosition = Vector3.zero;
        _vfx = gameObject.GetComponent<VisualEffect>();
    }

    void Update()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;
        ProcessModules(ref pos, ref rot);
        
        transform.localPosition = _initialPosition + (pos * Scale);
        transform.localRotation = rot;

        if (_vfx != null)
        {
            _vfx.SendEvent("Spawn");
        }
    }

    private void OnDrawGizmos()
    {
        Handles.PositionHandle(transform.position, transform.rotation);
    }
}
