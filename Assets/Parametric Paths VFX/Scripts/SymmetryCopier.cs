using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SymmetryCopier : MonoBehaviour
{
    [Header("Symmetry")]
    public PointSymmetry.Family family;
    public int n = 3;
    public float radius = 1f;

    [Header("Transform Before")]
    public Vector3 Position = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;
    public Vector3 Scale = Vector3.one;
    
    [Header("Transform Each")]
    public Vector3 PositionEach = Vector3.zero;
    public Vector3 RotationEach = Vector3.zero;
    public Vector3 ScaleEach = Vector3.one;
    public bool ApplyAfter = true;

    private VisualEffect vfx;
    
    private int numClones = 0;
    
    public static Quaternion ExtractRotation(Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;
 
        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;
 
        return Quaternion.LookRotation(forward, upwards);
    }
 
    public static Vector3 ExtractPosition(Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }
 
    public static Vector3 ExtractScale(Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }

    void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Init();
        }
    }

    void Init()
    {
        if (vfx == null)
        {
            vfx = gameObject.GetComponentInChildren<VisualEffect>();
            vfx.gameObject.SetActive(false);
        }
        
        var sym = new PointSymmetry(family, n, radius);

        if (numClones == sym.matrices.Count)
        {
            var transformBefore = Matrix4x4.TRS(Position, Quaternion.Euler(Rotation), Scale);
            var cumulativeTransform = Matrix4x4.TRS(PositionEach, Quaternion.Euler(RotationEach), ScaleEach);
            var currentCumulativeTransform = cumulativeTransform;
            var vfxClones = gameObject.GetComponentsInChildren<VisualEffect>();
            for (var i = 0; i < sym.matrices.Count; i++)
            {
                var tr = vfxClones[i].transform.parent;
                var m = sym.matrices[i];
                var matrix = (ApplyAfter ? currentCumulativeTransform * m : m * currentCumulativeTransform) *
                             transformBefore;
                tr.localScale = ExtractScale(matrix);
                tr.rotation = ExtractRotation(matrix);
                tr.position = ExtractPosition(matrix);
                currentCumulativeTransform *= cumulativeTransform;
            }
        }
        else
        {
            var prevVfxClones = gameObject.GetComponentsInChildren<VisualEffect>();
            foreach (var clone in prevVfxClones)
            {
                Destroy(clone.transform.parent.gameObject);
            }
            var transformBefore = Matrix4x4.TRS(Position, Quaternion.Euler(Rotation), Scale);
            var cumulativeTransform = Matrix4x4.TRS(PositionEach, Quaternion.Euler(RotationEach), ScaleEach);
            var currentCumulativeTransform = cumulativeTransform;
            for (var i = 0; i < sym.matrices.Count; i++)
            {
                var m = sym.matrices[i];
                var empty = new GameObject();
                empty.transform.parent = transform;
                var vfxCopy = Instantiate(vfx, empty.transform);
                vfxCopy.gameObject.SetActive(true);
                vfxCopy.GetComponent<VisualEffect>().SetInt("InstanceCount", sym.matrices.Count);
                vfxCopy.GetComponent<VisualEffect>().SetInt("InstanceID", i);
                var matrix = (ApplyAfter ? currentCumulativeTransform * m : m * currentCumulativeTransform) *
                             transformBefore;
                empty.transform.localScale = ExtractScale(matrix);
                empty.transform.rotation = ExtractRotation(matrix);
                empty.transform.position = ExtractPosition(matrix);
                currentCumulativeTransform *= cumulativeTransform;
            }

            numClones = sym.matrices.Count;
        }
    }

}
