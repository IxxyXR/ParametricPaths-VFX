using UnityEngine;

public class Lissajous : BasePathModule
{
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float xFactor = 4f;
    public float yFactor = 5f;
    public float zFactor = 6f;
    public float radius = .02f;

    Vector3 CalcLissajous(float theta)
    {
        return new Vector3(
            offsetX + radius * Mathf.Sin(xFactor * theta),
            offsetY + radius * Mathf.Sin(yFactor * theta + Mathf.PI / 2f),
            radius * Mathf.Sin(zFactor * theta + Mathf.PI)
        );
    }

    Vector3 CalcLissajousDeriv(float theta)
    {
        return new Vector3(
            offsetX + radius * Mathf.Cos(xFactor * theta),
            offsetY + radius * Mathf.Cos(yFactor * theta + Mathf.PI / 2f),
            radius * Mathf.Cos(zFactor * theta + Mathf.PI)
        );
    }

    
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        var newPos = CalcLissajous(t);
        newPos = rot * newPos;
        pos += newPos;
        
        // This is totally wrong but at least it does something that looks plausible
        // i.e. it's better than not rotating.
        // Note to self. Shouldn't have quit college before learning calculus.
        rot = Quaternion.LookRotation(
            CalcLissajousDeriv(t),
            rot * Vector3.forward
        );
    }

}