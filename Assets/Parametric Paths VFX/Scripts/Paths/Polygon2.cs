using UnityEngine;
using UnityEngine.Serialization;

public class Polygon2 : BasePathModule
{
    public float turns = 1f;
    public Vector3 axis = Vector3.up;
    public float radius = .02f;
    public int sides = 4;
    public EasingFunction.Ease easing = EasingFunction.Ease.Linear;

    private Vector3 GetPolygonVertex(int index)
    {
        float angle = 2 * (Mathf.PI * index) / sides;
        return new Vector3(
            radius * Mathf.Cos(angle),
            0,
            radius * Mathf.Sin(angle)
        );

    }
    public override void CalcTransforms(ref float t, ref Quaternion rot, ref Vector3 pos)
    {
        t *= turns;
        Quaternion rotation = rot * Quaternion.LookRotation(Vector3.forward, axis);
        // Quaternion rotation = Quaternion.Euler(0, -180f *  (t * Mathf.PI / 4f), 0);
        float timePerSide = 1f / sides;
        int currentSide = Mathf.FloorToInt(t * sides);
        int nextSide = currentSide + 1 % sides;
        float sidePosition = Mathf.InverseLerp(timePerSide * currentSide, timePerSide * nextSide, t);
        float easedPosition = EasingFunction.GetEasingFunction(easing).Invoke(0, 1, sidePosition);
        pos += rotation * Vector3.Lerp(GetPolygonVertex(currentSide), GetPolygonVertex(nextSide), easedPosition);
        var sideAngle = 360f / sides;
        rot = Quaternion.AngleAxis(sideAngle * currentSide + (sideAngle / 2f), -(rot * axis));
    }
}