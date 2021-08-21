using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;


public class VFXPaths : MonoBehaviour
{
    public GameObject Path;
    public float Rate = 0.01f;
    public float Scale = 1;
    public int pointsPerFrame = 100;
    
    public float initialDelay = 0;
    public float delay = 1f;
    public bool automatic = true;

    [ReadOnly] public float _time = 0;
    private Vector3 _initialPosition;
    private VisualEffect _vfx;
    
    public struct Shape
    {
        public List<Vector3> StartPoints;
        public List<Vector3> EndPoints;
    }
    
    void Start()
    {
        _vfx = gameObject.GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (automatic && !IsInvoking(nameof(DrawPath)))
        {
            InvokeRepeating(nameof(DrawPath), initialDelay, delay);                            
        }
        else if (!automatic && IsInvoking(nameof(DrawPath)))
        {
            CancelInvoke();
        }
    }

    private Shape BuildPath()
    {
        var shape = new Shape();
        shape.StartPoints = new List<Vector3>();
        shape.EndPoints = new List<Vector3>();
        Vector3 prevPoint = Vector3.negativeInfinity;
        Vector3 point = Vector3.negativeInfinity;
        for (int i=0; i<pointsPerFrame; i++)
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
                else if (component.GetType().IsSubclassOf(typeof(BaseAnimator)) && previousPathModule!=null)
                {
                    var animator = component as BaseAnimator;
                    if (animator != null && animator.active)
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
            point = _initialPosition + (pos * Scale);

            if (prevPoint != Vector3.negativeInfinity)
            {
                shape.StartPoints.Add(point);
                shape.EndPoints.Add(prevPoint);
            }
            prevPoint = point;
        }            
        return shape;
    }


    
    private Color[] BuildColorArray(Shape shape)
    {
        // Create a 2px high texture from start and end pairs
        // Row 0 is start points, row 1 is end points
        var pixelData = new Color[shape.StartPoints.Count * 2];        
        for (var i = 0; i < shape.StartPoints.Count; i++)
        {
            pixelData[i] = new Color(shape.StartPoints[i].x, shape.StartPoints[i].y, shape.StartPoints[i].z, 1);
            pixelData[i + shape.StartPoints.Count] = new Color(shape.EndPoints[i].x, shape.EndPoints[i].y, shape.EndPoints[i].z, 1);
        }
        return pixelData;
    }

    public void DrawPath()
    {
        var shape = BuildPath();
        var colorArray = BuildColorArray(shape);
        var texture = new Texture2D(colorArray.Length / 2, 2, TextureFormat.RGBAFloat, false);
        texture.wrapMode = TextureWrapMode.Mirror;
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorArray);
        texture.Apply();
        _vfx.SetTexture("Positions", texture);
        _vfx.SetInt("Count", texture.width);
    }

}