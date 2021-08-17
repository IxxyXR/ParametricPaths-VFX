using UnityEngine;
using UnityEngine.Serialization;

public class Supershape : BasePathModule
{
    public int turns = 1;
    public Vector3 axis = Vector3.up;
    public float m = 5f;
    public float n1 = 1f;
    public float n2 = 1f;
    public float n3 = 1f;
    public float radius = .02f;
    
    public override void CalcTransforms(float t, ref Quaternion rot, ref Vector3 pos)
    {
        var rotatedAxis = rot * axis;
        float angle = t * 360f * turns;
        Quaternion rotation = Quaternion.AngleAxis(angle, rotatedAxis);
        pos += rotation * Vector3.forward * SupershapeRadius(angle) * radius;
        rot = rotation;
    }
    
    float SupershapeRadius(float angle)
    {
        float phi = (angle / 360f) * Mathf.PI * 2f;
        float t1 = Mathf.Pow(Mathf.Abs(Mathf.Cos(m/4f * phi)), n2);
        float t2 = Mathf.Pow(Mathf.Abs(Mathf.Sin(m/4f * phi)), n3);
        return 1f/Mathf.Pow(t1 + t2, 1f/n1);
    }

}