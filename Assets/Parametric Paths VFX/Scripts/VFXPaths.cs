using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPaths : BasePathConsumer
{
    public int pointsPerFrame = 100;
    public float initialDelay;
    public float delay = 1f;
    public bool automatic = true;

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


    public void DrawPath()
    {
        // Construct Path
        
        var shape = new Shape();
        shape.StartPoints = new List<Vector3>();
        shape.EndPoints = new List<Vector3>();
        Vector3 prevPoint = Vector3.negativeInfinity;
        Vector3 point = Vector3.negativeInfinity;
        for (int i=0; i<pointsPerFrame; i++)
        {
            var pos = Vector3.zero;
            var rot = Quaternion.identity;
            ProcessModules(ref pos, ref rot);

            point = _initialPosition + (pos * Scale);

            if (prevPoint != Vector3.negativeInfinity)
            {
                shape.StartPoints.Add(point);
                shape.EndPoints.Add(prevPoint);
            }
            prevPoint = point;
        }
        
        // Encode path into textures
        
        var pixelData = new Color[shape.StartPoints.Count * 2];        
        for (var i = 0; i < shape.StartPoints.Count; i++)
        {
            pixelData[i] = new Color(shape.StartPoints[i].x, shape.StartPoints[i].y, shape.StartPoints[i].z, 1);
            pixelData[i + shape.StartPoints.Count] = new Color(shape.EndPoints[i].x, shape.EndPoints[i].y, shape.EndPoints[i].z, 1);
        }
        var texture = new Texture2D(pixelData.Length / 2, 2, TextureFormat.RGBAFloat, false);
        texture.wrapMode = TextureWrapMode.Mirror;
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixelData);
        texture.Apply();
        
        // Send textures to VFX
        
        _vfx.SetTexture("Positions", texture);
        _vfx.SetInt("Count", texture.width);
    }

}