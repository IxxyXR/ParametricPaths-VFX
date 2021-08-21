using UnityEngine;
using UnityEngine.Serialization;

public class Circle : BasePathModule
{
    public int turns = 1;
    public Vector3 axis = Vector3.up;
    public float radius = .02f;
    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        // Rotate the axis 
        Vector3 rotatedAxis = rot * axis;
        // Create a rotation around the new axis
        Quaternion rotation = Quaternion.AngleAxis(t * 360 * turns, rotatedAxis);

        var tangent = new Vector3(rotatedAxis.y, rotatedAxis.z, rotatedAxis.x).normalized; 
        pos += rotation * tangent * radius;
        rot = rotation;
    }
}